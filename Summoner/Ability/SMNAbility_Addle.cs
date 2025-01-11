using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_Addle : ISlotResolver
{
    

    public int Check()
    {
        if (!SMNSettings.Instance.AutoReduceDamage)
        {
            return -12;
        }
        if (Data.IsInHighEndDuty)
            return -11;
        if (!SpellsDefine.Addle.GetSpell().IsReadyWithCanCast())
        {
            return -10;
        }
        var target = Core.Me.GetCurrTarget();
        if (target == null)
        {
            return -9;
        }
        if (target.HasAura(SMNData.Buffs.Addle))
        {
            return -7;
        }
        
        if (TargetHelper.TargercastingIsbossaoe(target, SMNSettings.Instance.CastReduceTimeBeforeSeconds))
        {
            return 1;
        }
        return -1;
    }

    public Spell Spell => SpellsDefine.Addle.GetSpell();

    public void Build(Slot slot)
    {
#pragma warning disable CS8602 // 解引用可能出现空引用。
        SummonerRotationEntry.SMNHintManager.TriggerHint("减伤",$"昏乱 给 {Spell.GetTarget().CastActionId.GetSpell().LocalizedName}", customTTS: "自动昏乱已触发");
#pragma warning restore CS8602 // 解引用可能出现空引用。
        slot.Add(Spell);
    }
}