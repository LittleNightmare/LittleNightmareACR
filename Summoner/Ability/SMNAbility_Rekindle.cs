using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_Rekindle : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public Spell GetSpell()
    {
        // TODO: 添加更多的自定义目标，比方说最低血量，或者最低血量百分比
        return new Spell(SMNData.Spells.Rekindle.GetSpell().Id, SMNSettings.Instance.RekindleTarget);
    }

    public int Check()
    {
        if (!SMNData.Spells.Rekindle.GetSpell().IsReadyWithCanCast())
        {
            return -10;
        }

        if (GCDHelper.GetGCDCooldown() < 600)
        {
            return -6;
        }

        if (!SMNHelper.InPhoenix)
        {
            return -9;
        }

        if (Core.Resolve<JobApi_Summoner>().SummonTimerRemaining > 0)
        {
            if (Core.Resolve<JobApi_Summoner>().SummonTimerRemaining <=
                Core.Resolve<MemApiSpell>().GetGCDDuration(false) * 2)
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