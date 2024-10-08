using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_SummonCarbuncle : ISlotResolver
    {

        public Spell GetSpell()
        {
            return SMNData.Spells.SummonCarbuncle.GetSpell();
        }
        public int Check()
        {
            if (Core.Resolve<JobApi_Summoner>().HasPet)
            {
                return -10;
            }

            if (!GetSpell().Id.IsReady())
            {
                return -10;
            }
            if (Core.Resolve<JobApi_Summoner>().AttunmentTimerRemaining > 0)
            {
                return -3;
            }
            if (Core.Resolve<JobApi_Summoner>().SummonTimerRemaining > 0)
            {
                return -2;
            }
            if (Core.Resolve<MemApiMove>().IsMoving())
            {
                return -1;
            }
            return 0;
        }

        public void Build(Slot slot)
        {
            var spell = GetSpell();
            if (spell == null)
                return;
            slot.Add(spell);
        }
    }
}