using CombatRoutine;
using Common;
using Common.Define;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_Rekindle : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public Spell GetSpell()
    {
        return new Spell(SpellsDefine.Rekindle.GetSpell().Id, SpellTargetType.TargetTarget);
    }
    public int Check()
    {
        if (!SpellsDefine.Rekindle.IsReady())
        {
            return -10;
        }
        if (!Core.Get<IMemApiSummoner>().InPhoenix)
        {
            return -9;
        }
        if (Core.Get<IMemApiSummoner>().PetTimer > 0)
        {
            if (Core.Get<IMemApiSummoner>().PetTimer <= Core.Get<IMemApiSpell>().GetGCDDuration(false) * 2 )
            {
                return 0;
            }
        }
        return -1;
    }

    public void Build(Slot slot)
    {
        var spell = GetSpell();
        // if (spell == null)
        //     return;
        slot.Add(spell);
    }
}