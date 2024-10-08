using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_EnergyDrainSiphon : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public Spell GetSpell()
    {
        if (!SummonerRotationEntry.QT.GetQt("AOE")) return SMNData.Spells.EnergyDrain.GetSpell();
        if (SMNSettings.Instance.SmartAoETarget)
        {
            var canTargetObjects = TargetHelper.GetMostCanTargetObjects(SMNData.Spells.EnergySiphon, 2);
            if (canTargetObjects != null && canTargetObjects.IsValid())
                return new Spell(SMNData.Spells.EnergySiphon.GetSpell().Id, canTargetObjects);
        }
        else
        {
            var currentTarget = Core.Me.GetCurrTarget();
            if (currentTarget != null && TargetHelper.GetNearbyEnemyCount(currentTarget, 25, 5) >= 3)
            {
                return SMNData.Spells.EnergySiphon.GetSpell();
            }
        }

        return SMNData.Spells.EnergyDrain.GetSpell();
    }

    public int Check()
    {
        if (!GetSpell().Id.IsReady())
        {
            return -10;
        }

        if (!Core.Resolve<JobApi_Summoner>().HasAetherflowStacks)
        {
            return 0;
        }

        return -1;
    }

    public void Build(Slot slot)
    {
        slot.Add(GetSpell());
    }
}