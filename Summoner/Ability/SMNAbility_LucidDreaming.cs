using CombatRoutine;
using Common;
using Common.Define;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_LucidDreaming : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;
    public int Check()
    {
        if (!SpellsDefine.LucidDreaming.IsReady())
        {
            return -10;
        }
        if (Core.Me.CurrentMana <= 8000)
        {
            return 0;
        }
        return -1;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDefine.LucidDreaming.GetSpell());
    }
}