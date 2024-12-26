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
        
        if (TargetHelper.TargercastingIsbossaoe(target, 1))
        {
            return 1;
        }
        return -1;
    }

    public Spell Spell => SpellsDefine.Addle.GetSpell();

    public void Build(Slot slot)
    {
        slot.Add(Spell);
    }
}