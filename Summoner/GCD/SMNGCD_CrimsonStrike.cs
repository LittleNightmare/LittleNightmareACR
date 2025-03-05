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
            if (!GetSpell().IsReadyWithCanCast())
            {
                return -11;
            }

            if (!Core.Me.HasAura(SMNData.Buffs.IfritsFavorII))
            {
                return -10;
            }

            if (!SMNHelper.WithinActionAttackRange(SMNData.Spells.CrimsonStrike))
            {
                return -4;
            }

            if (!MoveHelper.IsMoving()) return 0;

            if (SMNSettings.Instance.SlideUseCrimonCyclone) return 1;

            return -1;
        }

        public void Build(Slot slot)
        {
            var spell = GetSpell();
            slot.Add(spell);
        }
    }
}