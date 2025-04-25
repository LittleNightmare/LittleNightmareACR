using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_SearingLight : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public int Check()
    {
        if (!SMNData.Spells.SearingLight.GetSpell().IsReadyWithCanCast())
        {
            return -10;
        }

        if (!Core.Me.InCombat())
        {
            return -9;
        }

        if (!Core.Resolve<JobApi_Summoner>().HasPet)
        {
            return -8;
        }

        if (!SummonerRotationEntry.QT.GetQt("爆发"))
        {
            return -7;
        }

        if (!SummonerRotationEntry.QT.GetQt("灼热之光"))
        {
            return -6;
        }
        if (GCDHelper.GetGCDCooldown() < 600)
        {
            return -5;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        //slot.Add(new SlotAction(SlotAction.WaitType.WaitForSndHalfWindow, 0, SMNData.Spells.SearingLight.GetSpell()));
        //slot.Add2NdWindowAbility(SMNData.Spells.SearingLight.GetSpell());
        slot.Add(SMNData.Spells.SearingLight.GetSpell());
    }
}