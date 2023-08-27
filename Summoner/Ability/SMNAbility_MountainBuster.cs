using CombatRoutine;
using Common;
using Common.Define;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_MountainBuster : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;
    public int Check()
    {
        if (Core.Me.HasMyAura(AurasDefine.TitansFavor))
        {
            return 0;
        }
        return -1;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDefine.MountainBuster.GetSpell());
    }
}