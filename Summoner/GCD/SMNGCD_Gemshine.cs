using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_Gemshine : ISlotResolver
    {
        public Spell GetSpell()
        {
            if (!SummonerRotationEntry.QT.GetQt("AOE")) return SMNHelper.BaseSummonSingle();
            if (SMNSettings.Instance.SmartAoETarget)
            {
                var canTargetObjects = TargetHelper.GetMostCanTargetObjects(SMNHelper.BaseSummonAoE().Id);
                if (canTargetObjects != null && canTargetObjects.IsValid())
                {
                    return new Spell(SMNHelper.BaseSummonAoE().Id, canTargetObjects);
                }
            } else
            {
                var currentTarget = Core.Me.GetCurrTarget();
                if (currentTarget != null && TargetHelper.GetNearbyEnemyCount(currentTarget, 25, 5) >= 3)
                {
                    return SMNHelper.BaseSummonAoE();
                }
            }

            return SMNHelper.BaseSummonSingle();
        }
        public SlotMode SlotMode { get; } = SlotMode.Gcd;
        public int Check()
        {
            if (!SummonerRotationEntry.QT.GetQt("宝石耀"))
            {
                return -10;
            }
            // 宝石耀和宝石辉应该状态是相同的，就不单独判断了
            if (!GetSpell().Id.IsReady()) return -10;
            if (Core.Resolve<JobApi_Summoner>().AttunementAdjust <= 0) return -8;
            
            // var isEnableCustom = SMNBattleData.Instance.CustomSummon.Count > 0;
            var timesLeft = Core.Resolve<JobApi_Summoner>().ActivePetType switch
            {
                ActivePetType.Titan => SMNBattleData.Instance.TitanGemshineTimes,
                ActivePetType.Ifrit => SMNBattleData.Instance.IfritGemshineTimes,
                ActivePetType.Garuda => SMNBattleData.Instance.GarudaGemshineTimes,
                _ => 0
            };
            if (Core.Resolve<JobApi_Summoner>().ActivePetType is ActivePetType.Titan or ActivePetType.Ifrit or ActivePetType.Garuda)
            {
                // 其实这里主要是给自定义用的，如果正常情况，归零时已经为None了
                if (timesLeft <= 0)
                {
                    return -3;
                }
                if (Core.Resolve<MemApiMove>().IsMoving() && !Core.Me.HasAura(SMNData.Buffs.Swiftcast) && Core.Resolve<JobApi_Summoner>().ActivePetType == ActivePetType.Ifrit)
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
            // if (spell == null)
            //     return;
            slot.Add(spell);
        }
    }
}