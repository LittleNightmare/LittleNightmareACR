using CombatRoutine;
using Common;
using Common.Define;
using Common.Helper;
using Common.Language;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_Gemshine : ISlotResolver
    {
        public Spell GetSpell()
        {
            if (Qt.GetQt("AOE".Loc()))
            {
                var aoeCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
                if (aoeCount >= 3)
                {
                    return SMNSpellHelper.BaseSummonAoE();
                }
            }

            return SMNSpellHelper.BaseSummonSingle();
        }
        public SlotMode SlotMode { get; } = SlotMode.Gcd;
        public int Check()
        {
            if (!Qt.GetQt("宝石耀".Loc()))
            {
                return -10;
            }
            // 宝石耀和宝石辉应该状态是相同的，就不单独判断了
            if (!GetSpell().IsReady()) return -10;
            if (Core.Get<IMemApiSummoner>().ElementalAttunement <= 0) return -1;
            if (Core.Get<IMemApiSummoner>().ActivePetType == ActivePetType.Titan && SMNBattleData.Instance.TitanGemshineTimes > 0)
            {
                return 0;
            }
            if (Core.Get<IMemApiSummoner>().ActivePetType == ActivePetType.Ifrit && SMNBattleData.Instance.IfritGemshineTimes > 0)
            {
                if (Core.Get<IMemApiMove>().IsMoving() && !Core.Me.HasMyAura(AurasDefine.Swiftcast))
                {
                    return -2;
                }
                return 0;
            }
            if (Core.Get<IMemApiSummoner>().ActivePetType == ActivePetType.Garuda && SMNBattleData.Instance.GarudaGemshineTimes > 0)
            {
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