using CombatRoutine;
using Common;
using Common.Define;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_CrimsonStrike : ISlotResolver
    {
        public Spell GetSpell()
        {
            return SpellsDefine.CrimsonStrike.GetSpell();
        }
        public SlotMode SlotMode { get; } = SlotMode.Gcd;
        public int Check()
        {
            if (!GetSpell().IsReady())
            {
                return -10;
            }
            if (Core.Get<IMemApiSpell>().GetLastComboSpellId() == SpellsDefine.CrimsonCyclone)
            {
                return 0;
            }
            return -1;
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