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
            // 暂时不支持启用技能不位移，除非还有个扩展目标圈的判断，因为第二段火神冲需要靠近释放
            var onTargetRing = Core.Me.DistanceMelee(Core.Me.GetCurrTarget()) <= 0;
            if (Core.Get<IMemApiMove>().IsMoving())
            {
                if (!onTargetRing)
                {
                    return -2;
                }
                if (SMNSettings.Instance.SlideUseCrimonCyclone) return 0;
            }

            if (SMNBattleData.Instance.IfritGemshineTimes > 0 && (onTargetRing || Qt.GetQt("自动火神冲".Loc())))
            {
                // 先冲锋再读条
                if (SMNSettings.Instance.IfritMode == 0)
                {
                    return 0;
                }
                // 先读条再冲锋，此时还有宝石技能,此时不会移动，不考虑自动火神冲; 这里好像会导致读条后不冲锋，
                if (SMNSettings.Instance.IfritMode == 1)
                {
                    return -2;
                }
                // "读条-冲锋-读条"
                if (SMNSettings.Instance.IfritMode == 2 && SMNBattleData.Instance.IfritGemshineTimes < 2)
                {
                    return 0;
                }

                return -2;
            }

            if (onTargetRing || Qt.GetQt("自动火神冲".Loc()))
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