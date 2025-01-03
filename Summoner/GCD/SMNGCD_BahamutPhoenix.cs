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
        public Spell GetSpell()
        {
            return SMNHelper.BahamutPhoneix();
        }

        public SlotMode SlotMode { get; } = SlotMode.Gcd;

        public int Check()
        {
            var spell = GetSpell();
            if (!(Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Bahamut)
                 || Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Phoneix)
                 || Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.SolarBahamut)))
            {
                return -11;
            }

            if (!spell.IsReadyWithCanCast())
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
            slot.Add(spell);
        }
    }
}