using CombatRoutine;
using Common;
using Common.Define;
using Common.Language;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_SearingLight : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;
    public int Check()
    {
        if (!SpellsDefine.SearingLight.IsReady())
        {
            return -10;
        }
        if (!Core.Me.InCombat)
        {
            return -9;
        }
        if (!Core.Get<IMemApiSummoner>().HasPet)
        {
            return -8;
        }
        if (!Qt.GetQt("爆发".Loc()))
        {
            return -7;
        }
        if (!Qt.GetQt("灼热之光".Loc()))
        {
            return -7;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDefine.SearingLight.GetSpell());
    }
}