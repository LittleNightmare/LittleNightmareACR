using System.Diagnostics;
using System.Reflection;
using CombatRoutine;
using CombatRoutine.Chat;
using CombatRoutine.Setting;
using Common;
using Common.Define;
using Common.Helper;
using Common.Language;

namespace LittleNightmare.Summoner;

public class SMNRotationEventHandler : IRotationEventHandler
{
    public void OnResetBattle()
    {
        SMNBattleData.Instance.Reset();
        if (SMNSettings.Instance.JobViewSave.AutoReset)
        {
            Qt.Reset();
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

        if (spell.Id == SMNSpellHelper.BahamutPhoneix().Id)
        {
            SMNBattleData.Instance.UpdateSummon();
        }

        // switch (spell.Id)
        // {
            // case SpellsDefine.SummonPhoenix:
            //     SMNBattleData.Instance.In90Opener = false;
            //     break;

            // case SpellsDefine.TopazRuin:
            // case SpellsDefine.TopazRuinIi:
            // case SpellsDefine.TopazRuinIii:
            // case SpellsDefine.TopazRite:
            // case SpellsDefine.TopazOutburst:
            // case SpellsDefine.TopazDisaster:
            //     SMNBattleData.Instance.TitanGemshineTimes = Core.Get<IMemApiSummoner>().ElementalAttunement - (4 - SMNBattleData.Instance.TitanGemshineTimesCustom);
            //     //
            //     // ChatHelper.Print.Echo($"TitanGemshineTimes:{SMNBattleData.Instance.TitanGemshineTimes}");
            //     break;
            //
            // case SpellsDefine.RubyRuin:
            // case SpellsDefine.RubyRuinIi:
            // case SpellsDefine.RubyRuinIii:
            // case SpellsDefine.RubyRite:
            // case SpellsDefine.RubyOutburst:
            // case SpellsDefine.RubyDisaster:
            //     SMNBattleData.Instance.IfritGemshineTimes = Core.Get<IMemApiSummoner>().ElementalAttunement - (2 - SMNBattleData.Instance.IfritGemshineTimesCustom);
            //     // ChatHelper.Print.Echo($"IfritGemshineTimes:{SMNBattleData.Instance.IfritGemshineTimes}");
            //     break;
            //
            // case SpellsDefine.EmeraldRuin:
            // case SpellsDefine.EmeraldRuinIi:
            // case SpellsDefine.EmeraldRuinIii:
            // case SpellsDefine.EmeraldRite:
            // case SpellsDefine.EmeraldOutburst:
            // case SpellsDefine.EmeraldDisaster:
            //     SMNBattleData.Instance.GarudaGemshineTimes = Core.Get<IMemApiSummoner>().ElementalAttunement - (4 - SMNBattleData.Instance.GarudaGemshineTimesCustom);
            //     // ChatHelper.Print.Echo($"GarudaGemshineTimes:{SMNBattleData.Instance.GarudaGemshineTimes}");
            //     break;
            //
            // default:
            //     AI.Instance.BattleData.LimitAbility = false;
            //     break;
        // }
        // 两次矫正，主要出现莫名奇妙不重置次数的问题，这里多加一个保险
        // switch (Core.Get<IMemApiSummoner>().ActivePetType)
        // {
        //     case ActivePetType.Titan:
        //         SMNBattleData.Instance.TitanGemshineTimes = Core.Get<IMemApiSummoner>().ElementalAttunement - (4 - SMNBattleData.Instance.TitanGemshineTimesCustom);
        //         break;
        //     case ActivePetType.Ifrit:
        //         SMNBattleData.Instance.IfritGemshineTimes = Core.Get<IMemApiSummoner>().ElementalAttunement - (2 - SMNBattleData.Instance.IfritGemshineTimesCustom);
        //         break;
        //     case ActivePetType.Garuda:
        //         SMNBattleData.Instance.GarudaGemshineTimes = Core.Get<IMemApiSummoner>().ElementalAttunement - (4 - SMNBattleData.Instance.GarudaGemshineTimesCustom);
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
        if (Core.Get<IMemApiSummoner>().InBahamut || Core.Get<IMemApiSummoner>().InPhoenix)
        {
            AI.Instance.BattleData.Limit2Ability = true;
        }
        else
        {
            AI.Instance.BattleData.LimitAbility = false;
            AI.Instance.BattleData.Limit2Ability = false;
        }
    }

    public void OnEnterRotation()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version.ToString();
        LogHelper.Print("LittleNightmare召唤 当前版本: ".Loc() + version);
        LogHelper.Print("反馈问题如果找不到我，可以访问下列地址去提\nhttps://github.com/LittleNightmare/LittleNightmareACR/issues/new".Loc());
    }
}