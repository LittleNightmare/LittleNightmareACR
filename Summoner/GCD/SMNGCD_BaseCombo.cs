using CombatRoutine;
using Common;
using Common.Define;
using Common.Helper;
using Common.Language;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_BaseCombo : ISlotResolver
    {
        public Spell GetSpell()
        {
            if (!Qt.GetQt("AOE".Loc())) return SMNSpellHelper.BaseSingle();

            var target = Core.Me.GetCurrTarget();
            if (SMNSettings.Instance.SmartAoETarget)
            {
                var canTargetObjects = TargetHelper.GetMostCanTargetObjects(SMNSpellHelper.BaseAoE().Id);
                if (canTargetObjects.IsValid)
                {
                    target = canTargetObjects;
                }
            }
            
            if (TargetHelper.CheckNeedUseAOE(target, 25, 5, 3))
            {
                return new Spell(SMNSpellHelper.BaseAoE().Id, target);
            }

            return SMNSpellHelper.BaseSingle();
        }
        public SlotMode SlotMode { get; } = SlotMode.Gcd;
        public int Check()
        {
            if (!GetSpell().IsReady()) return -10;
            if (Core.Get<IMemApiMove>().IsMoving())
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