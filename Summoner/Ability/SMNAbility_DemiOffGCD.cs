using CombatRoutine;
using Common;
using Common.Define;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_DemiOffGCD : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public Spell GetSpell()
    {
        if (!SMNSpellHelper.EnkindleDemi().IsReady() && Core.Get<IMemApiSummoner>().InBahamut)
        {
            return SpellsDefine.Deathflare.GetSpell();
        }
        return SMNSpellHelper.EnkindleDemi();

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
        if (spell == null)
            return;
        slot.Add(spell);
    }
}