using CombatRoutine;
using Common;
using Common.Define;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_Slipstream : ISlotResolver
    {
        public Spell GetSpell()
        {
            return SpellsDefine.Slipstream.GetSpell();
        }
        public SlotMode SlotMode { get; } = SlotMode.Gcd;
        public int Check()
        {
            if (!GetSpell().IsReady())
            {
                return -10;
            }
            if (Core.Me.HasMyAura(AurasDefine.GarudasFavor))
            {
                if (Core.Get<IMemApiMove>().IsMoving() && !Core.Me.HasMyAura(AurasDefine.Swiftcast))
                {
                    return -2;
                }
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