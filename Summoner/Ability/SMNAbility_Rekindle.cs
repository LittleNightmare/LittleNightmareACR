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
        return new Spell(SMNData.Spells.Rekindle.GetSpell().Id, SpellTargetType.TargetTarget);
    }
    public int Check()
    {
        if (!SMNData.Spells.Rekindle.IsReady())
        {
            return -10;
        }
        if (!SMNHelper.InPhoenix)
        {
            return -9;
        }
        if (Core.Resolve<JobApi_Summoner>().SummonTimerRemaining > 0)
        {
            if (Core.Resolve<JobApi_Summoner>().SummonTimerRemaining <= Core.Resolve<MemApiSpell>().GetGCDDuration(false) * 2 )
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