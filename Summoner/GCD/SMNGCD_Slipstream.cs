using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_Slipstream : ISlotResolver
    {
        public Spell GetSpell()
        {
            if (!SummonerRotationEntry.QT.GetQt("AOE")) return SMNData.Spells.Slipstream.GetSpell();
            if (!SMNSettings.Instance.SmartAoETarget) return SMNData.Spells.Slipstream.GetSpell();
            var canTargetObjects = TargetHelper.GetMostCanTargetObjects(SMNData.Spells.Slipstream, 2);

            return canTargetObjects != null && canTargetObjects.IsValid()
                ? new Spell(SMNData.Spells.Slipstream, canTargetObjects)
                : SMNData.Spells.Slipstream.GetSpell();
        }

        public SlotMode SlotMode { get; } = SlotMode.Gcd;

        public int Check()
        {
            if (!GetSpell().IsReadyWithCanCast())
            {
                return -10;
            }

            if (Core.Me.HasAura(SMNData.Buffs.GarudasFavor))
            {
                if (MoveHelper.IsMoving() && !Core.Me.HasAura(SMNData.Buffs.Swiftcast))
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
            slot.Add(spell);
        }
    }
}