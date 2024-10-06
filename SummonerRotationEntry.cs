using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using LittleNightmare.Summoner;
using LittleNightmare.Summoner.Ability;
using LittleNightmare.Summoner.GCD;
using LittleNightmare.Summoner.Triggers;

namespace LittleNightmare
{
    public class SummonerRotationEntry : IRotationEntry
    {

        public Jobs TargetJob = Jobs.Summoner;

        public string OverlayTitle = "LNM Summoner";

        public AcrType AcrType = AcrType.Both;

        public int MinLevel = 1;

        public int MaxLevel = 90;

        public string Description = "召唤通用ACR，与逆光的大体相同，处于摸鱼状态，推荐用逆光的，毕竟不知道什么时候就摸了\n" +
                                     "这个ACR与逆光的相比，提供多一点点的自定义设置。这可以增强日常体验，让ACR更符合各位召唤师的习惯\n" +
                                     "选项的介绍请查看 设置";

        public string AuthorName { get; set; } = "LittleNightmare";

        public static JobViewWindow QT { get; private set; }


        private readonly List<SlotResolverData> SlotResolvers = new()
            {
                // 宝石兽召唤
                new SlotResolverData(new SMNGCD_SummonCarbuncle(), SlotMode.Gcd),
                // 进巴哈不死鸟
                new SlotResolverData(new SMNGCD_BahamutPhoenix(), SlotMode.Gcd),
                // 复活
                new SlotResolverData(new SMNGCD_Resurrection(), SlotMode.Gcd),
                // 巴哈不死鸟喷喷
                new SlotResolverData(new SMNGCD_BahamutPhoenixGCD(), SlotMode.Gcd),
                // 火神冲锋二段
                new SlotResolverData(new SMNGCD_CrimsonStrike(), SlotMode.Gcd),
                // 火神冲锋一段
                new SlotResolverData(new SMNGCD_CrimsonCyclone(), SlotMode.Gcd),
                // 风神读条
                new SlotResolverData(new SMNGCD_Slipstream(), SlotMode.Gcd),
                // 宝石辉宝石耀
                new SlotResolverData(new SMNGCD_Gemshine(), SlotMode.Gcd),
                // 三神召唤
                new SlotResolverData(new SMNGCD_Summon(), SlotMode.Gcd),
                // 毁4
                new SlotResolverData(new SMNGCD_RuinIV(), SlotMode.Gcd),
                // 毁123
                new SlotResolverData(new SMNGCD_BaseCombo(), SlotMode.Gcd),

                // 即刻咏唱
                new SlotResolverData(new SMNAbility_SwiftCast(), SlotMode.OffGcd),
                // 团辅
                new SlotResolverData(new SMNAbility_SearingLight(), SlotMode.OffGcd),
                // 能量吸收/
                new SlotResolverData(new SMNAbility_EnergyDrainSiphon(), SlotMode.OffGcd),
                // 龙神迸发 不死鸟迸发  死星核爆
                new SlotResolverData(new SMNAbility_DemiOffGCD(), SlotMode.OffGcd),
                // 溃烂爆发
                new SlotResolverData(new SMNAbility_Fester(), SlotMode.OffGcd),
                // 苏生之炎
                new SlotResolverData(new SMNAbility_Rekindle(), SlotMode.OffGcd),
                // 山崩
                new SlotResolverData(new SMNAbility_MountainBuster(), SlotMode.OffGcd),
                // 醒梦
                new SlotResolverData(new SMNAbility_LucidDreaming(), SlotMode.OffGcd),
            };

        public Rotation Build(string settingFolder)
        {
            SMNSettings.Build(settingFolder);
            BuildQt();
            var rotation = new Rotation(SlotResolvers)
            {
                TargetJob = TargetJob,
                AcrType = AcrType,
                MinLevel = MinLevel,
                MaxLevel = MaxLevel,
                Description = Description,
            };
            return rotation
                .AddOpener(GetOpener)
                .SetRotationEventHandler(new SMNRotationEventHandler())
                //.AddSettingUIs(new SMNSettingView())
                //.AddSlotSequences()
                .AddTriggerAction(
                    new SMNTriggerActionQt(),
                    new SMNTriggerActionAutoCrimsonCyclone(),
                    new SMNTriggerActionBahamutPhoenix(),
                    new SMNTriggerActionCustomGemshineTimes(),
                    new SMNTriggerActionCustomSummon(),
                    new SMNTriggerActionGemshine(),
                    new SMNTriggerActionSearingLight(),
                    new SMNTriggerActionSummon(),
                    new SMNTriggerActionUseSummon(),
                    new SMNTriggerActionSwiftCastMode(),
                    new SMNTriggerActionIfritMode()
                // new SMNTriggerActionPreCastSwiftcast()
                )
                .AddTriggerCondition(
                    new SMNTriggerGaugeCheck(),
                    new SMNTriggersActionPetCheck(),
                    new SMNTriggersActionAttunementCheck(),
                    new SMNTriggersActionSummonTimeCheck())
                .AddCanUseHighPrioritySlotCheck(CanUseHighPrioritySlotCheck);
        }

        private IOpener theBalanceOpener = new OpenerSMN90();
        private IOpener FastEnergyDrainOpener = new OpenerSMN90FastEnergyDrain();

        IOpener? GetOpener(uint level)
        {
            if (level < 70) return null;
            return SMNSettings.Instance.SelectedOpener switch
            {
                SMNSettings.OpenerType.TheBalance => theBalanceOpener,
                SMNSettings.OpenerType.FastEnergyDrain => FastEnergyDrainOpener,
                _ => null,
            };
        }



        public int CanUseHighPrioritySlotCheck(SlotMode slotMode, Slot slot)
        {
            foreach (var item in slot.Actions)
            {
                var spell = item.Spell;
                if (!spell.Id.IsReady()) return -1;
                if (spell.CanCast() < 0) return -1;
                switch (slotMode)
                {
                    case SlotMode.Gcd:
                        // TODO: 火神冲2段
                        if (spell.CastTime.TotalSeconds > 0)
                        {
                            if (Core.Resolve<MemApiMove>().IsMoving() && !Core.Me.HasAura(SMNData.Buffs.Swiftcast))
                            {
                                return -1;
                            }
                        }

                        if (spell.Id != SMNData.Spells.Ruin4) return 0;
                        if (!Core.Me.HasAura(SMNData.Buffs.FurtherRuin))
                        {
                            return -1;
                        }
                        break;
                    case SlotMode.OffGcd:
                        if (spell.Charges < 1)
                        {
                            return -1;
                        }

                        if (spell != SMNData.Spells.RadiantAegis.GetSpell()) return 0;
                        if (Core.Resolve<JobApi_Summoner>().SummonTimerRemaining != 0 && (SMNHelper.InBahamut || SMNHelper.InPhoenix || SMNHelper.InSolarBahamut))
                        {
                            return -1;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(slotMode), slotMode, null);
                }
            }
            return 0;
        }

        public void BuildQt()
        {
            QT = new SummonerOverlay(SMNSettings.Instance.JobViewSave, SMNSettings.Instance.Save, OverlayTitle);
        }

        public IRotationUI GetRotationUI()
        {
            return QT;
        }

        public void OnDrawSetting()
        {
            SMNSettingView.Instance.Draw();
        }

        public void Dispose()
        {
            
        }
    }
}