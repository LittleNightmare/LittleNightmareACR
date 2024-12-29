using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using System.Reflection;


namespace LittleNightmare.Summoner;

public class SMNRotationEventHandler : IRotationEventHandler
{
    public void OnResetBattle()
    {
        SMNBattleData.Instance.Reset();
        if (SMNSettings.Instance.AutoResetQt)
        {
            SummonerRotationEntry.QT.Reset();
        }   
    }

    public Task OnPreCombat()
    {
        return Task.CompletedTask;
    }

    public Task OnNoTarget()
    {
        return Task.CompletedTask;
    }

    public void AfterSpell(Slot slot, Spell spell)
    {
        // if (SMNBattleData.Instance.CustomSummon.Count > 0 && !SMNBattleData.Instance.In90Opener)
        // {
        //     if (spell.Id == SMNSpellHelper.Titan().Id || spell.Id == SMNSpellHelper.Ifrit().Id || spell.Id == SMNSpellHelper.Garuda().Id)
        //     {
        //         // 这里fuck模式不正常，试试换个逻辑实现
        //         // SMNBattleData.Instance.CustomSummon.Remove(spell);
        //     }
        // }

        if (spell.Id == SMNHelper.BahamutPhoneix().Id)
        {
            SMNBattleData.Instance.UpdateSummon();
        }

        if (SMNSettings.Instance.TTKControl)
        {
            var target = Core.Me.GetCurrTarget();
            if (target != null && !SMNBattleData.Instance.TTKTriggered)
            {
                if (!SMNBattleData.Instance.FinalBoss)
                {
                    if (TTKHelper.IsTargetTTK(target))
                    {
                        SummonerRotationEntry.SMNHintManager.TriggerHint("TTK");
                        SummonerRotationEntry.QT.SetQt("爆发", false);
                        SMNBattleData.Instance.TTKTriggered = true;
                    }
                }
                else
                {
                    if (TTKHelper.CheckFinalBurst(target))
                    {
                        SummonerRotationEntry.SMNHintManager.TriggerHint("TTKFinal");
                        SummonerRotationEntry.QT.SetQt("最终爆发", true);
                        SMNBattleData.Instance.TTKTriggered = true;
                    }
                }
            }
        }

        // switch (spell.Id)
        // {
        // case SMNData.Spells.SummonPhoenix:
        //     SMNBattleData.Instance.In90Opener = false;
        //     break;

        // case SMNData.Spells.TopazRuin:
        // case SMNData.Spells.TopazRuinIi:
        // case SMNData.Spells.TopazRuinIii:
        // case SMNData.Spells.TopazRite:
        // case SMNData.Spells.TopazOutburst:
        // case SMNData.Spells.TopazDisaster:
        //     SMNBattleData.Instance.TitanGemshineTimes = Core.Resolve<JobApi_Summoner>().ElementalAttunement - (4 - SMNBattleData.Instance.TitanGemshineTimesCustom);
        //     //
        //     // ChatHelper.Print.Echo($"TitanGemshineTimes:{SMNBattleData.Instance.TitanGemshineTimes}");
        //     break;
        //
        // case SMNData.Spells.RubyRuin:
        // case SMNData.Spells.RubyRuinIi:
        // case SMNData.Spells.RubyRuinIii:
        // case SMNData.Spells.RubyRite:
        // case SMNData.Spells.RubyOutburst:
        // case SMNData.Spells.RubyDisaster:
        //     SMNBattleData.Instance.IfritGemshineTimes = Core.Resolve<JobApi_Summoner>().ElementalAttunement - (2 - SMNBattleData.Instance.IfritGemshineTimesCustom);
        //     // ChatHelper.Print.Echo($"IfritGemshineTimes:{SMNBattleData.Instance.IfritGemshineTimes}");
        //     break;
        //
        // case SMNData.Spells.EmeraldRuin:
        // case SMNData.Spells.EmeraldRuinIi:
        // case SMNData.Spells.EmeraldRuinIii:
        // case SMNData.Spells.EmeraldRite:
        // case SMNData.Spells.EmeraldOutburst:
        // case SMNData.Spells.EmeraldDisaster:
        //     SMNBattleData.Instance.GarudaGemshineTimes = Core.Resolve<JobApi_Summoner>().ElementalAttunement - (4 - SMNBattleData.Instance.GarudaGemshineTimesCustom);
        //     // ChatHelper.Print.Echo($"GarudaGemshineTimes:{SMNBattleData.Instance.GarudaGemshineTimes}");
        //     break;
        //
        // default:
        //     AI.Instance.BattleData.LimitAbility = false;
        //     break;
        // }
        // 两次矫正，主要出现莫名奇妙不重置次数的问题，这里多加一个保险
        // switch (Core.Resolve<JobApi_Summoner>().ActivePetType)
        // {
        //     case ActivePetType.Titan:
        //         SMNBattleData.Instance.TitanGemshineTimes = Core.Resolve<JobApi_Summoner>().ElementalAttunement - (4 - SMNBattleData.Instance.TitanGemshineTimesCustom);
        //         break;
        //     case ActivePetType.Ifrit:
        //         SMNBattleData.Instance.IfritGemshineTimes = Core.Resolve<JobApi_Summoner>().ElementalAttunement - (2 - SMNBattleData.Instance.IfritGemshineTimesCustom);
        //         break;
        //     case ActivePetType.Garuda:
        //         SMNBattleData.Instance.GarudaGemshineTimes = Core.Resolve<JobApi_Summoner>().ElementalAttunement - (4 - SMNBattleData.Instance.GarudaGemshineTimesCustom);
        //         break;
        // }

        // LogHelper.Info($"SpellID:{spell.Name}Trace:{new StackTrace()}");
        // ChatHelp.Print.Echo($"SpellID:{spell.Name}Trace:{new StackTrace()}");
    }

    public void OnBattleUpdate(int currTime)
    {
        // 这里放after spell可以吗？
        SMNBattleData.Instance.UsedSummon();
        if (Core.Me.IsDead && SMNBattleData.Instance.CustomSummon.Count != 0)
        {
            SMNBattleData.Instance.CustomSummon.Clear();
        }
        // TODO: 要确认是最后一个BOSS，这里只能判断到最后一个任务，有时候，有各种奇葩的BOSS，二段变身，死而复生，打核心巴拉巴拉的
        if (LNMHelper.IsLastTask())
        {
            SMNBattleData.Instance.FinalBoss = true;
        }
        // FIXME: 不知道干啥用的
        //if (SMNHelper.InBahamut || SMNHelper.InPhoenix || SMNHelper.InSolarBahamut)
        //{
        //    AI.Instance.BattleData.a
        //}
        //else
        //{
        //    AI.Instance.BattleData.LimitAbility = false;
        //    AI.Instance.BattleData.Limit2Ability = false;
        //}
    }

    public void OnEnterRotation()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version?.ToString() ?? "Unknown version";
        SummonerRotationEntry.SMNHintManager.TriggerHint("Welcome", customContent: "LittleNightmare召唤 当前版本: " + version, customTTS: "欢迎使用Little Nightmare的召唤ACR");
    }

    public void OnSpellCastSuccess(Slot slot, Spell spell)
    {
    }

    public void OnExitRotation()
    {
        OnResetBattle();
    }

    public void OnTerritoryChanged()
    {
        OnResetBattle();
    }
}