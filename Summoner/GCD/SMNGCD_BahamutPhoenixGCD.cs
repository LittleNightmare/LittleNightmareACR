using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Function;
using AEAssist.Helper;
using AEAssist.JobApi;
using System;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_BahamutPhoenixGCD : ISlotResolver
    {
        public Spell GetSpell()
        {
            if (!SummonerRotationEntry.QT.GetQt("AOE")) return SMNHelper.BaseSingle();

            var canTargetObjects = TargetHelper.GetMostCanTargetObjects(SMNHelper.BaseAoE().Id);
            if (SMNSettings.Instance.SmartAoETarget)
            {
                if (canTargetObjects != null && canTargetObjects.IsValid())
                {
                   return new Spell(SMNHelper.BaseAoE().Id, canTargetObjects);
                }
                    
            }
            else
            {
                var damageRange = 0;
                if (SMNHelper.InBahamut)
                {
                    damageRange = 5;
                }
                if (SMNHelper.InPhoenix || SMNHelper.InSolarBahamut)
                {
                    damageRange = 8;
                }
                var currentTarget = Core.Me.GetCurrTarget();
                if (currentTarget != null && TargetHelper.GetNearbyEnemyCount(currentTarget, 25, damageRange) >= 3)
                {
                    return SMNHelper.BaseAoE();
                }
                
            }
            
            return SMNHelper.BaseSingle();
        }
        public SlotMode SlotMode { get; } = SlotMode.Gcd;
        public int Check()
        {
            if (!GetSpell().Id.IsReady())
            {
                return -10;
            }
            if (Core.Resolve<JobApi_Summoner>().AttunmentTimerRemaining > 0)
            {
                return -10;
            }
            if (Core.Resolve<JobApi_Summoner>().SummonTimerRemaining > 0)
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