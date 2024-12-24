using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_RuinIV : ISlotResolver
    {
        public Spell GetSpell()
        {
            if (!SummonerRotationEntry.QT.GetQt("AOE")) return SMNData.Spells.Ruin4.GetSpell();
            if (!SMNSettings.Instance.SmartAoETarget) return SMNData.Spells.Ruin4.GetSpell();
            var canTargetObjects = TargetHelper.GetMostCanTargetObjects(SMNData.Spells.Ruin4, 2);
            return canTargetObjects?.IsValid() == true
                ? new Spell(SMNData.Spells.Ruin4, canTargetObjects)
                : SMNData.Spells.Ruin4.GetSpell();
        }

        public SlotMode SlotMode { get; } = SlotMode.Gcd;

        public int Check()
        {
            if (!SMNData.Spells.Ruin4.GetSpell().IsReadyWithCanCast())
            {
                return -10;
            }

            if (Core.Me.HasAura(SMNData.Buffs.FurtherRuin))
            {
                if (SMNSettings.Instance.UseRuinIIIFirst && !Core.Resolve<MemApiMove>().IsMoving() &&
                    !SMNData.Spells.EnergyDrain.CoolDownInGCDs(1) && !SummonerRotationEntry.QT.GetQt("最终爆发"))
                {
                    if (SMNHelper.BahamutPhoneix().Id.CoolDownInGCDs(1))
                    {
                        return 0;
                    }

                    return -2;
                }

                if (SMNSettings.Instance.SlideUseCrimonCyclone && Core.Me.HasAura(SMNData.Buffs.IfritsFavor) &&
                    Core.Me.Distance(Core.Me.GetCurrTarget(), AEAssist.Define.DistanceMode.IgnoreTargetHitbox) <= 0)
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