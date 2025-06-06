using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_LucidDreaming : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public int Check()
    {
        if (!SpellsDefine.LucidDreaming.GetSpell().IsReadyWithCanCast())
        {
            return -10;
        }

        if (GCDHelper.GetGCDCooldown() < 600)
        {
            return -6;
        }

        if (Core.Me.CurrentMp <= SMNSettings.Instance.MPThreshold)
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