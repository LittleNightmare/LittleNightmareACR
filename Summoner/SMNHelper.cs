using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;

namespace LittleNightmare.Summoner
{
    public static class SMNHelper
    {
        public static Spell Titan()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.SummonTopaz.GetSpell().Id).GetSpell();
        }

        public static Spell Ifrit()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.SummonRuby.GetSpell().Id).GetSpell();
        }

        public static Spell Garuda()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.SummonEmerald.GetSpell().Id).GetSpell();
        }

        public static Spell BahamutPhoneix()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.Aethercharge.GetSpell().Id).GetSpell();
        }

        public static Spell EnkindleDemi()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.EnkindleBahamut.GetSpell().Id).GetSpell();
        }
        public static Spell BaseSingle()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.Ruin.GetSpell().Id).GetSpell();
        }

        public static Spell BaseAoE()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.Outburst.GetSpell().Id).GetSpell();
        }

        public static Spell BaseSummonSingle()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.Gemshine.GetSpell().Id).GetSpell();
        }

        public static Spell BaseSummonAoE()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.PreciousBrilliance.GetSpell().Id).GetSpell();
        }

        public static bool InBahamut => Core.Resolve<JobApi_Summoner>().ActivePetType == ActivePetType.Bahamut;

        public static bool InPhoenix => Core.Resolve<JobApi_Summoner>().ActivePetType == ActivePetType.Phoneix;

        public static bool InSolarBahamut => Core.Resolve<JobApi_Summoner>().ActivePetType == ActivePetType.SolarBahamut;
    }
}