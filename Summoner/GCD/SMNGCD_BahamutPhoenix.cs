using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_BahamutPhoenix : ISlotResolver
    {
        public Spell? GetSpell()
        {
            if (Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Bahamut)
                || Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Phoneix)
                || Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.SolarBahamut))
            {
                return SMNHelper.BahamutPhoneix();
            }

            return null;
        }

        public SlotMode SlotMode { get; } = SlotMode.Gcd;

        public int Check()
        {
            var spell = GetSpell();
            if (spell == null)
            {
                return -11;
            }

            if (!spell.Id.IsReady())
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
                return -3;
            }

            if (!SummonerRotationEntry.QT.GetQt("巴哈凤凰"))
            {
                return -2;
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