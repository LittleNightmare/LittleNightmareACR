using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_BaseCombo : ISlotResolver
    {
        public Spell GetSpell()
        {
            if (!SummonerRotationEntry.QT.GetQt("AOE")) return SMNHelper.BaseSingle();
            var currentTarget = Core.Me.GetCurrTarget();

            if (SMNSettings.Instance.SmartAoETarget)
            {
                var canTargetObjects = TargetHelper.GetMostCanTargetObjects(SMNHelper.BaseAoE().Id);
                if (canTargetObjects != null && canTargetObjects.IsValid())
                {
                    return new Spell(SMNHelper.BaseAoE().Id, canTargetObjects);
                }
            }
            else if (currentTarget != null &&
                     TargetHelper.GetNearbyEnemyCount(currentTarget, 25, 5) >= 3)
            {
                return SMNHelper.BaseAoE();
            }

            return SMNHelper.BaseSingle();
        }

        public SlotMode SlotMode { get; } = SlotMode.Gcd;

        public int Check()
        {
            if (!GetSpell().IsReadyWithCanCast()) return -10;
            if (MoveHelper.IsMoving())
            {
                return -1;
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