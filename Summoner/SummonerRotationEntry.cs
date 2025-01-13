using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using LittleNightmare.Summoner.Ability;
using LittleNightmare.Summoner.GCD;
using LittleNightmare.Summoner.Triggers;

namespace LittleNightmare.Summoner
{
    public class SummonerRotationEntry : IRotationEntry
    {
        public Jobs TargetJob = Jobs.Summoner;

        public string OverlayTitle = "LNM Summoner";

        public AcrType AcrType = AcrType.Both;

        public int MinLevel = 1;

        public int MaxLevel = 100;

        public string Description = "召唤通用ACR，需要下载JiaXX的ACR才能获得完整功能，处于测试状态，有问题请及时告诉我\n" +
                                    //"这个ACR与逆光的相比，提供多一点点的自定义设置。这可以增强日常体验，让ACR更符合各位召唤师的习惯\n" +
                                    "选项的介绍请查看 设置";

        public string AuthorName { get; set; } = "LittleNightmare";

        public static SummonerOverlay QT { get; private set; }

        public static HintManager SMNHintManager { get; private set; }


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
            new SlotResolverData(new SMNAbility_Aether(), SlotMode.OffGcd),
            // 灼热之闪
            new SlotResolverData(new SMNAbility_SearingFlash(), SlotMode.OffGcd),
            // 苏生之炎
            new SlotResolverData(new SMNAbility_Rekindle(), SlotMode.OffGcd),
            // 山崩
            new SlotResolverData(new SMNAbility_MountainBuster(), SlotMode.OffGcd),
            // 醒梦
            new SlotResolverData(new SMNAbility_LucidDreaming(), SlotMode.OffGcd),
            
            // 下面是自动减伤，目前考虑高难先不加，检测当前高难自动关闭
            // TODO 准备一个变量，只能通过时间轴控制，用来在高难中开启减伤
            new SlotResolverData(new SMNAbility_Addle(), SlotMode.OffGcd),
        };

        public Rotation Build(string settingFolder)
        {
            SMNSettings.Build(settingFolder);
            BuildHints();
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
                    new SMNTriggerActionIfritMode(),
                    new SMNTriggerActionAdjustACRSimpleSettings()
                    // new SMNTriggerActionPreCastSwiftcast()
                )
                .AddTriggerCondition(
                    new SMNTriggerGaugeCheck(),
                    new SMNTriggersActionPetCheck(),
                    new SMNTriggersActionAttunementCheck(),
                    new SMNTriggersActionSummonTimeCheck(),
                    new SMNTriggersPotionCheck())
                .AddCanUseHighPrioritySlotCheck(CanUseHighPrioritySlotCheck);
        }

        private IOpener theBalanceOpener90 = new OpenerSMN90();
        private IOpener theBalanceOpener = new OpenerSMN100();
        private IOpener FastEnergyDrainOpener = new OpenerSMN90FastEnergyDrain();

        IOpener? GetOpener(uint level)
        {
            if (level < 70) return null;
            return SMNSettings.Instance.SelectedOpener switch
            {
                SMNSettings.OpenerType.TheBalance => theBalanceOpener,
                SMNSettings.OpenerType.FastEnergyDrain => FastEnergyDrainOpener,
                SMNSettings.OpenerType.TheBalance90 => theBalanceOpener90,
                _ => null,
            };
        }


        public int CanUseHighPrioritySlotCheck(SlotMode slotMode, Slot slot)
        {
            foreach (var item in slot.Actions)
            {
                var spell = item.Spell;
                if (!spell.IsReadyWithCanCast()) return -1;
                if (!spell.CanCast()) return -2;
                switch (slotMode)
                {
                    case SlotMode.Gcd:
                        // TODO: 火神冲2段,好像不用搞了，等国际服的更新同步以后就不用了
                        if (spell.CastTime.TotalSeconds > 0)
                        {
                            if (MoveHelper.IsMoving() && !Core.Me.HasAura(SMNData.Buffs.Swiftcast))
                            {
                                return -3;
                            }
                        }

                        if (spell.Id != SMNData.Spells.Ruin4) return 0;
                        if (!Core.Me.HasAura(SMNData.Buffs.FurtherRuin))
                        {
                            return -4;
                        }

                        break;
                    case SlotMode.OffGcd:
                        if (spell.Charges < 1)
                        {
                            return -5;
                        }

                        if (spell != SMNData.Spells.RadiantAegis.GetSpell()) return 0;
                        if (Core.Resolve<JobApi_Summoner>().SummonTimerRemaining != 0 &&
                            SMNHelper.InAnyDemi)
                        {
                            return -6;
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
            //QT.SetUpdateAction(OnUIUpdate);
        }

        public static void BuildHints(bool reset = false)
        {
            SMNHintManager = new HintManager(SMNSettings.Instance.SMNHints);
            if (reset)
            {
                SMNSettings.Instance.SMNHints = new();
            }

            SMNHintManager.AddHint("Welcome", new Hint("", toast2TimeInMs: 5000, useTTS: true));
            SMNHintManager.AddHint("引用检测", new Hint("", toast2TimeInMs: 5000, useTTS: true, toast2Style: 2));

            SMNHintManager.AddHint("AOE自动关闭提示", new Hint("检测到当前位置未解锁AOE，已将AOE自动关闭。如果您坚信是误报，请暂时每次战斗中手动在QT选项中开启，并随后向我反馈", useTTS: true));

            // ttK提示
            SMNHintManager.AddHint("TTK", new Hint("目标濒死，关闭爆发"));
            SMNHintManager.AddHint("TTKFinal", new Hint("目标濒死，开启最终爆发"));
            // 起手提示
            SMNHintManager.AddHint("TheBalanceOpener100", new Hint("进入TheBalance起手", showToast2: false));
            SMNHintManager.AddHint("TheBalanceOpener90", new Hint("进入TheBalance90起手", showToast2: false));
            SMNHintManager.AddHint("FastEnergyDrainOpener", new Hint("进入FastEnergyDrain起手", showToast2: false));

            SMNHintManager.AddHint("减伤", new Hint(""));
            SMNHintManager.AddHint("即刻", new Hint("", showInChat: false));
            SMNHintManager.AddHint("复活", new Hint("", showToast2: false, toast2TimeInMs: 5000));
            SMNHintManager.AddHint("停手", new Hint("特殊状态，已自动停手"));


        }

        public void OnUIUpdate()
        {
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