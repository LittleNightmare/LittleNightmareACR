using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_DemiOffGCD : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public Spell GetSpell()
    {
        var targetAction = SMNHelper.EnkindleDemi();
        if (!SMNHelper.EnkindleDemi().Id.IsReady() && SMNHelper.InBahamut)
        {
            targetAction = SMNData.Spells.Deathflare.GetSpell();
        }

        if (!SMNHelper.EnkindleDemi().Id.IsReady() && SMNHelper.InSolarBahamut)
        {
            targetAction = SMNData.Spells.Sunflare.GetSpell();
        }

        if (!SummonerRotationEntry.QT.GetQt("AOE")) return targetAction;
        if (!SMNSettings.Instance.SmartAoETarget) return targetAction;
        var canTargetObjects = TargetHelper.GetMostCanTargetObjects(targetAction.Id, 2);
        return canTargetObjects != null && canTargetObjects.IsValid()
            ? new Spell(targetAction.Id, canTargetObjects)
            : targetAction;
    }

    public int Check()
    {
        if (!GetSpell().Id.IsReady())
        {
            return -10;
        }

        if (!(SMNHelper.InBahamut || SMNHelper.InPhoenix || SMNHelper.InSolarBahamut))
        {
            return -9;
        }

        if (Core.Resolve<JobApi_Summoner>().SummonTimerRemaining > 0)
        {
            if (Core.Resolve<JobApi_Summoner>().SummonTimerRemaining <=
                Core.Resolve<MemApiSpell>().GetGCDDuration(false) * 2 || SummonerRotationEntry.QT.GetQt("最终爆发"))
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