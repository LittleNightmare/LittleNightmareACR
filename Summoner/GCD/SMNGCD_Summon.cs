using CombatRoutine;
using Common;
using Common.Define;
using Common.Language;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_Summon : ISlotResolver
    {
        public Spell? GetSpell()
        {
            var targetAction = SMNBattleData.Instance.Summon[0];
            if (SMNBattleData.Instance.CustomSummon.Count > 0)
            {
                targetAction =  SMNBattleData.Instance.CustomSummon[0];
            }
            // 50级后，召唤三神有AOE技能，关键技能是 内力迸发
            if (Core.Me.ClassLevel < 50)
            {
                return targetAction;
            }
            if (targetAction.Id is 
                SpellsDefine.SummonIfrit or 
                SpellsDefine.SummonTitan or 
                // SpellsDefine.SummonGaruda or
                SpellsDefine.SummonRuby or
                SpellsDefine.SummonTopaz or
                SpellsDefine.SummonEmerald)
                return targetAction;
            if (!Qt.GetQt("AOE".Loc())) return targetAction;
            if (!SMNSettings.Instance.SmartAoETarget) return targetAction;
            var canTargetObjects = TargetHelper.GetMostCanTargetObjects(targetAction.Id);
            return canTargetObjects.IsValid ? new Spell(targetAction.Id, canTargetObjects) : targetAction;
        }
        public SlotMode SlotMode { get; } = SlotMode.Gcd;
        public int Check()
        {
            if (!GetSpell().IsReady()) return -10;
            if (!Core.Get<IMemApiSummoner>().HasPet)
            {
                return -10;
            }
            if (Core.Get<IMemApiSummoner>().PetTimer > 0)
            {
                return -9;
            }
            if (!Qt.GetQt("三神召唤".Loc()))
            {
                return -8;
            }
            if (Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Titan)
                || Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Ifrit)
                || Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Garuda))
            {
                if (Core.Me.HasAura(AurasDefine.IfritsFavor))
                {
                    return -2;
                }
                if (Core.Get<IMemApiSummoner>().TranceTimer > 0)
                {
                    if (Core.Get<IMemApiSummoner>().ActivePetType == ActivePetType.Titan && SMNBattleData.Instance.TitanGemshineTimes == 0)
                    {
                        return 0;
                    }
                    if (Core.Get<IMemApiSummoner>().ActivePetType == ActivePetType.Ifrit && SMNBattleData.Instance.IfritGemshineTimes == 0)
                    {
                        return 0;
                    }
                    if (Core.Get<IMemApiSummoner>().ActivePetType == ActivePetType.Garuda && SMNBattleData.Instance.GarudaGemshineTimes == 0)
                    {
                        return 0;
                    }
                    return -8;
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