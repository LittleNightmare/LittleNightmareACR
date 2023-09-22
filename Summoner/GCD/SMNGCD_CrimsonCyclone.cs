using CombatRoutine;
using CombatRoutine.Setting;
using Common;
using Common.Define;
using Common.Helper;
using Common.Language;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_CrimsonCyclone : ISlotResolver
    {
        public Spell GetSpell()
        {
            return SpellsDefine.CrimsonCyclone.GetSpell();
        }
        public SlotMode SlotMode { get; } = SlotMode.Gcd;
        public int Check()
        {
            if (!GetSpell().IsReady())
            {
                return -10;
            }
            if (!Core.Me.HasMyAura(AurasDefine.IfritsFavor))
            {
                return -10;
            }
            if (!SpellsDefine.CrimsonCyclone.IsUnlock())
            {
                return -9;
            }

            var onTargetRing = Core.Me.DistanceMelee(Core.Me.GetCurrTarget()) <= 0;
            if (Core.Get<IMemApiMove>().IsMoving())
            {
                if (!onTargetRing && !AI.Instance.LockPos)
                {
                    return -2;
                }
                if (SMNSettings.Instance.SlideUseCrimonCyclone) return 0;
            }

            if (SMNSettings.Instance.RubyGCDFirst && SMNBattleData.Instance.IfritGemshineTimes > 0)
            {
                return -2;
            }

            if (Qt.GetQt("自动火神冲".Loc()))
            {
                return 0;
            }
            if (onTargetRing || AI.Instance.LockPos)
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