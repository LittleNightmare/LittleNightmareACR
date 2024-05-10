using CombatRoutine;
using Common;
using Common.Define;
using Common.Helper;
using Common.Language;
using LittleNightmare.Summoner.Ability;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_RuinIV : ISlotResolver
    {
        public Spell GetSpell()
        {
            if (!Qt.GetQt("AOE".Loc())) return SpellsDefine.Ruin4.GetSpell();
            if (SMNSettings.Instance.SmartAoETarget)
            {
                var canTargetObjects = TargetHelper.GetMostCanTargetObjects(SpellsDefine.Ruin4);
                return canTargetObjects.IsValid ? new Spell(SpellsDefine.Ruin4, canTargetObjects) : SpellsDefine.Ruin4.GetSpell();
            }
            return SpellsDefine.Ruin4.GetSpell();
        }
        public SlotMode SlotMode { get; } = SlotMode.Gcd;
        public int Check()
        {
            if (!SpellsDefine.Ruin4.IsReady())
            {
                return -10;
            }
            if (Core.Me.HasMyAura(AurasDefine.FurtherRuin))
            {
                if (SMNSettings.Instance.UseRuinIIIFirst && !Core.Get<IMemApiMove>().IsMoving() && !SpellsDefine.EnergyDrain.CoolDownInGCDs(1) && !Qt.GetQt("最终爆发"))
                {
                    if (SMNSpellHelper.BahamutPhoneix().CoolDownInGCDs(1))
                    {
                        return 0;
                    }
                    return -2;
                }

                if (SMNSettings.Instance.SlideUseCrimonCyclone && Core.Me.HasMyAura(AurasDefine.IfritsFavor) && Core.Me.DistanceMelee(Core.Me.GetCurrTarget()) <= 0)
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
            if (spell == null)
                return;
            slot.Add(spell);
        }
    }
}