﻿using CombatRoutine;
using CombatRoutine.Opener;
using CombatRoutine.TriggerModel;
using CombatRoutine.View.JobView;
using Common;
using Common.Define;
using Common.Helper;
using Common.Language;
using LittleNightmare.Summoner;
using LittleNightmare.Summoner.Ability;
using LittleNightmare.Summoner.GCD;
using LittleNightmare.Summoner.Triggers;

namespace LittleNightmare
{
    public class SummonerRotationEntry : IRotationEntry
    {
        public string AuthorName => "LittleNightmare";

        public Jobs TargetJob => Jobs.Summoner;

        public string OverlayTitle => "LNM Summoner";

        public AcrType AcrType => AcrType.Both;

        public int MinLevel => 1;

        public int MaxLevel => 90;

        public string Description => "召唤通用ACR，与逆光的大体相同，处于摸鱼状态，推荐用逆光的，毕竟不知道什么时候就摸了\n" +
                                     "这个ACR与逆光的相比，提供多一点点的自定义设置。这可以增强日常体验，让ACR更符合各位召唤师的习惯\n" +
                                     "选项的介绍请查看 设置";

        public static JobViewWindow JobViewWindow;


        public List<ISlotResolver> SlotResolvers = new()
        {
            // 宝石兽召唤
            new SMNGCD_SummonCarbuncle(),
            // 进巴哈不死鸟
            new SMNGCD_BahamutPhoenix(),
            // 复活
            new SMNGCD_Resurrection(),
            // 巴哈不死鸟喷喷
            new SMNGCD_BahamutPhoenixGCD(),
            // 火神冲锋二段
            new SMNGCD_CrimsonStrike(),
            // 火神冲锋一段
            new SMNGCD_CrimsonCyclone(),
            // 风神读条
            new SMNGCD_Slipstream(),
            // 宝石辉宝石耀
            new SMNGCD_Gemshine(),
            // 三神召唤
            new SMNGCD_Summon(),
            // 毁4
            new SMNGCD_RuinIV(),
            // 毁123
            new SMNGCD_BaseCombo(),

            // 即刻咏唱
            new SMNAbility_SwiftCast(),
            // 团辅
            new SMNAbility_SearingLight(),
            // 能量吸收/
            new SMNAbility_EnergyDrainSiphon(),
            // 龙神迸发 不死鸟迸发  死星核爆
            new SMNAbility_DemiOffGCD(),
            // 溃烂爆发
            new SMNAbility_Fester(),
            // 苏生之炎
            new SMNAbility_Rekindle(),
            // 山崩
            new SMNAbility_MountainBuster(),
            // 醒梦
            new SMNAbility_LucidDreaming(),
        };

        public Rotation Build(string settingFolder)
        {
            SMNSettings.Build(settingFolder);
            return new Rotation(this, () => SlotResolvers)
                .AddOpener(GetOpener)
                .SetRotationEventHandler(new SMNRotationEventHandler())
                .AddSettingUIs(new SMNSettingView())
                .AddSlotSequences()
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
        private void Check(TriggerNodeBase triggerNodeBase)
        {

        }


        private void Upgrade(TriggerLine obj)
        {
            // Check(obj.TriggerRoot);
        }

        public int CanUseHighPrioritySlotCheck(SlotMode slotMode, Spell spell)
        {
            if (!spell.IsReady()) return -1;
            if(spell.CanCast() < 0) return -1;
            switch (slotMode)
            {
                case SlotMode.Gcd:
                    // TODO: 火神冲2段
                    if (spell.CastTime.TotalSeconds > 0)
                    {
                        if (Core.Get<IMemApiMove>().IsMoving() && !Core.Me.HasMyAura(AurasDefine.Swiftcast))
                        {
                            return -1;
                        }
                    }

                    if (spell.Id != SpellsDefine.Ruin4) return 0;
                    if (!Core.Me.HasMyAura(AurasDefine.FurtherRuin))
                    {
                        return -1;
                    }
                    return 0;
                case SlotMode.OffGcd:
                    if (spell.Charges < 1)
                    {
                        return -1;
                    }

                    if (spell != SpellsDefine.RadiantAegis.GetSpell()) return 0;
                    if (Core.Get<IMemApiSummoner>().PetTimer != 0 && (Core.Get<IMemApiSummoner>().InBahamut || Core.Get<IMemApiSummoner>().InPhoenix))
                    {
                        return -1;
                    }
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slotMode), slotMode, null);
            }
        }

        public bool BuildQt(out JobViewWindow jobViewWindow)
        {
            jobViewWindow = new SummonerOverlay(SMNSettings.Instance.JobViewSave, SMNSettings.Instance.Save, OverlayTitle);
            JobViewWindow = jobViewWindow;
            return true;
        }
    }
}