using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_Summon : ISlotResolver
    {
        public Spell? GetSpell()
        {
            var targetAction = SMNBattleData.Instance.Summon[0];
            if (SMNBattleData.Instance.CustomSummon.Count > 0)
            {
                targetAction = SMNBattleData.Instance.CustomSummon[0];
            }

            // 50级后，召唤三神有AOE技能，关键技能是 内力迸发
            if (Core.Me.Level < 50)
            {
                return targetAction;
            }

            if (targetAction.Id is
                SMNData.Spells.SummonIfrit or
                SMNData.Spells.SummonTitan or
                // SMNData.Spells.SummonGaruda or
                SMNData.Spells.SummonRuby or
                SMNData.Spells.SummonTopaz or
                SMNData.Spells.SummonEmerald)
                return targetAction;
            if (!SummonerRotationEntry.QT.GetQt("AOE")) return targetAction;
            if (!SMNSettings.Instance.SmartAoETarget) return targetAction;
            var canTargetObjects = TargetHelper.GetMostCanTargetObjects(targetAction.Id, 2);
            return canTargetObjects != null ? new Spell(targetAction.Id, canTargetObjects) : targetAction;
        }

        public SlotMode SlotMode { get; } = SlotMode.Gcd;

        public int Check()
        {
            if (SMNBattleData.Instance.Summon.Count == 0)
            {
                return -12;
            }
            // 不能用GetSpell，一个个对比下
            if (!SMNHelper.Titan().Id.IsReady() && !SMNHelper.Ifrit().Id.IsReady() &&
                !SMNHelper.Garuda().Id.IsReady()) return -11;
            if (!Core.Resolve<JobApi_Summoner>().HasPet)
            {
                return -10;
            }

            if (Core.Resolve<JobApi_Summoner>().SummonTimerRemaining > 0)
            {
                return -9;
            }

            if (!SummonerRotationEntry.QT.GetQt("三神召唤"))
            {
                return -8;
            }

            if (Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Titan)
                || Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Ifrit)
                || Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Garuda))
            {
                // 在开启三神召唤后，并且如果启用了最终爆发，那么就疯狂召唤三神，中间不管三神带来的技能，中间这些技能没有哪个单独威力高过召唤三神本身的
                if (SummonerRotationEntry.QT.GetQt("最终爆发") && SMNSettings.Instance.FastPassSummon)
                {
                    return 1;
                }
                if (Core.Me.HasAura(SMNData.Buffs.IfritsFavor))
                {
                    return -2;
                }
                //TODO: 检查三神召唤后，是否会延后巴哈/凤凰的召唤


                if (Core.Resolve<JobApi_Summoner>().AttunementAdjust > 0)
                {
                    if (Core.Resolve<JobApi_Summoner>().ActivePetType == ActivePetType.Titan &&
                        SMNBattleData.Instance.TitanGemshineTimes == 0)
                    {
                        return 0;
                    }

                    if (Core.Resolve<JobApi_Summoner>().ActivePetType == ActivePetType.Ifrit &&
                        SMNBattleData.Instance.IfritGemshineTimes == 0)
                    {
                        return 0;
                    }

                    if (Core.Resolve<JobApi_Summoner>().ActivePetType == ActivePetType.Garuda &&
                        SMNBattleData.Instance.GarudaGemshineTimes == 0)
                    {
                        return 0;
                    }

                    return -7;
                }

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