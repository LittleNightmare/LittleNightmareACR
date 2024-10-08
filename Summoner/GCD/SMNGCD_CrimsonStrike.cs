using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_CrimsonStrike : ISlotResolver
    {
        public Spell GetSpell()
        {
            return SMNData.Spells.CrimsonStrike.GetSpell();
        }

        public SlotMode SlotMode { get; } = SlotMode.Gcd;

        public int Check()
        {
            if (!GetSpell().Id.IsReady())
            {
                return -10;
            }

            if (Core.Resolve<MemApiSpell>().GetLastComboSpellId() == SMNData.Spells.CrimsonCyclone)
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