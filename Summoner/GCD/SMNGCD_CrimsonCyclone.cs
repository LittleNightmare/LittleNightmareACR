using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_CrimsonCyclone : ISlotResolver
    {
        public Spell GetSpell()
        {
            return SMNData.Spells.CrimsonCyclone.GetSpell();
        }

        public SlotMode SlotMode { get; } = SlotMode.Gcd;

        public int Check()
        {
            if (!GetSpell().IsReadyWithCanCast())
            {
                return -11;
            }

            if (!Core.Me.HasAura(SMNData.Buffs.IfritsFavor))
            {
                return -10;
            }

            if (!SMNData.Spells.CrimsonCyclone.IsUnlock())
            {
                return -9;
            }
            // 暂时不支持启用技能不位移，除非还有个扩展目标圈的判断，因为第二段火神冲需要靠近释放

            //var onTargetRing = Core.Me.DistanceMelee(Core.Me.GetCurrTarget()) <= 0;
            var onTargetRing =
                Core.Me.Distance(Core.Me.GetCurrTarget(), AEAssist.Define.DistanceMode.IgnoreTargetHitbox) <= 0;
            if (Core.Resolve<MemApiMove>().IsMoving())
            {
                if (!onTargetRing)
                {
                    return -4;
                }

                if (SMNSettings.Instance.SlideUseCrimonCyclone) return 0;
            }

            if (SMNBattleData.Instance.IfritGemshineTimes > 0 &&
                (onTargetRing || SummonerRotationEntry.QT.GetQt("自动火神冲")))
            {
                // 先冲锋再读条
                if (SMNSettings.Instance.IfritMode == 0 
                    || (SummonerRotationEntry.QT.GetQt("最终爆发") && SMNSettings.Instance.ModifyIfritMode && SMNSettings.Instance.FastPassSummon))
                {
                    return 0;
                }

                // 先读条再冲锋，此时还有宝石技能,此时不会移动，不考虑自动火神冲; 这里好像会导致读条后不冲锋，但实际没有
                if (SMNSettings.Instance.IfritMode == 1)
                {
                    return -3;
                }

                // "读条-冲锋-读条"
                if (SMNSettings.Instance.IfritMode == 2 && SMNBattleData.Instance.IfritGemshineTimes < 2)
                {
                    return 0;
                }

                return -2;
            }
            // 到这里这里正常应该火神读条都放完了
            if (onTargetRing || SummonerRotationEntry.QT.GetQt("自动火神冲"))
            {
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