using CombatRoutine;
using Common;
using Common.Define;

namespace LittleNightmare.Summoner
{
    public static class SMNSpellHelper
    {
        public static Spell Titan()
        {
            return Core.Get<IMemApiSpell>().CheckActionChange(SpellsDefine.SummonTopaz.GetSpell().Id).GetSpell();
        }

        public static Spell Ifrit()
        {
            return Core.Get<IMemApiSpell>().CheckActionChange(SpellsDefine.SummonRuby.GetSpell().Id).GetSpell();
        }

        public static Spell Garuda()
        {
            return Core.Get<IMemApiSpell>().CheckActionChange(SpellsDefine.SummonEmerald.GetSpell().Id).GetSpell();
        }

        public static Spell BahamutPhoneix()
        {
            return Core.Get<IMemApiSpell>().CheckActionChange(SpellsDefine.Aethercharge.GetSpell().Id).GetSpell();
        }

        public static Spell EnkindleDemi()
        {
            return Core.Get<IMemApiSpell>().CheckActionChange(SpellsDefine.EnkindleBahamut.GetSpell().Id).GetSpell();
        }
        public static Spell BaseSingle()
        {
            return Core.Get<IMemApiSpell>().CheckActionChange(SpellsDefine.Ruin.GetSpell().Id).GetSpell();
        }

        public static Spell BaseAoE()
        {
            return Core.Get<IMemApiSpell>().CheckActionChange(SpellsDefine.Outburst.GetSpell().Id).GetSpell();
        }

        public static Spell BaseSummonSingle()
        {
            return Core.Get<IMemApiSpell>().CheckActionChange(SpellsDefine.Gemshine.GetSpell().Id).GetSpell();
        }

        public static Spell BaseSummonAoE()
        {
            return Core.Get<IMemApiSpell>().CheckActionChange(SpellsDefine.PreciousBrilliance.GetSpell().Id).GetSpell();
        }
    }
}