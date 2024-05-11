using CombatRoutine;
using Common;
using Common.Define;
using Common.Language;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_DemiOffGCD : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public Spell GetSpell()
    {
        var targetAction = SMNSpellHelper.EnkindleDemi();
        if (!SMNSpellHelper.EnkindleDemi().IsReady() && Core.Get<IMemApiSummoner>().InBahamut)
        {
            targetAction =  SpellsDefine.Deathflare.GetSpell();
        }
        if (!Qt.GetQt("AOE".Loc())) return targetAction;
        if (!SMNSettings.Instance.SmartAoETarget) return targetAction;
        var canTargetObjects = TargetHelper.GetMostCanTargetObjects(targetAction.Id, 2);
        return canTargetObjects.IsValid ? new Spell(targetAction.Id, canTargetObjects) : targetAction;
    }
    public int Check()
    {
        if (!GetSpell().IsReady())
        {
            return -10;
        }

        if (!(Core.Get<IMemApiSummoner>().InBahamut || Core.Get<IMemApiSummoner>().InPhoenix))
        {
            return -9;
        }
        if (Core.Get<IMemApiSummoner>().PetTimer > 0)
        {
            if (Core.Get<IMemApiSummoner>().PetTimer <= Core.Get<IMemApiSpell>().GetGCDDuration(false) * 4 || Qt.GetQt("最终爆发"))
            {
                return 0;
            }
        }
        return -1;
    }

    public void Build(Slot slot)
    {
        var spell = GetSpell();
        slot.Add(spell);
    }
}