using CombatRoutine;
using CombatRoutine.Opener;
using Common.Define;
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

        private SummonerOverlay _overlay = new();

        public List<ISlotResolver> SlotResolvers = new()
        {
            // 宝石兽召唤
            new SMNGCD_SummonCarbuncle(),
            // 进巴哈不死鸟
            new SMNGCD_BahamutPhoenix(),
            // 巴哈不死鸟喷喷
            new SMNGCD_BahamutPhoenixGCD(),
            // 火神冲锋二段
            new SMNGCD_CrimsonStrike(),
            // 复活
            new SMNGCD_Resurrection(),
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
            // 龙神迸发 不死鸟迸发  死星核爆
            new SMNAbility_DemiOffGCD(),
            // 苏生之炎
            new SMNAbility_Rekindle(),
            // 山崩
            new SMNAbility_MountainBuster(),
            // 溃烂爆发
            new SMNAbility_Fester(),
            // 能量吸收/
            new SMNAbility_EnergyDrainSiphon(),
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
                    new SMNTriggerActionSwiftCastMode()
                    // new SMNTriggerActionPreCastSwiftcast()
                );
        }

        private IOpener opener90 = new OpenerSMN90();

        IOpener? GetOpener(uint level)
        {
            if (level < 70) return null;
            return opener90;
        }

        public void DrawOverlay()
        {
            _overlay.Draw();
        }

        public void OnLanguageChanged(LanguageType languageType)
        {
           
        }
    }
}