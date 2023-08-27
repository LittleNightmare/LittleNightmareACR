using CombatRoutine;
using Common;
using Common.Define;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_RuinIV : ISlotResolver
    {
        public Spell GetSpell()
        {
            return SpellsDefine.Ruin4.GetSpell();
        }
        public SlotMode SlotMode { get; } = SlotMode.Gcd;
        public int Check()
        {
            if (!SpellsDefine.Ruin4.IsReady())
            {
                return -10;
            }
            if (Core.Me.HasMyAura(AurasDefine.FurtherRuin))
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