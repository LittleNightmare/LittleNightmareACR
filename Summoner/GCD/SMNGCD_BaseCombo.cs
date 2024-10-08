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

            if (SMNSettings.Instance.SmartAoETarget)
            {
                var canTargetObjects = TargetHelper.GetMostCanTargetObjects(SMNHelper.BaseAoE().Id);
                if (canTargetObjects != null && canTargetObjects.IsValid())
                {
                    return new Spell(SMNHelper.BaseAoE().Id, canTargetObjects);
                }
            }
            else if (Core.Me.GetCurrTarget() != null &&
                     TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5) >= 3)
            {
                return SMNHelper.BaseAoE();
            }

            return SMNHelper.BaseSingle();
        }

        public SlotMode SlotMode { get; } = SlotMode.Gcd;

        public int Check()
        {
            if (!GetSpell().Id.IsReady()) return -10;
            if (Core.Resolve<MemApiMove>().IsMoving())
            {
                return -1;
            }

            return 0;
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