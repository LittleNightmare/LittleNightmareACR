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
            
            // var isEnableCustom = SMNBattleData.Instance.CustomSummon.Count > 0;
            var timesLeft = Core.Get<IMemApiSummoner>().ActivePetType switch
            {
                ActivePetType.Titan => SMNBattleData.Instance.TitanGemshineTimes,
                ActivePetType.Ifrit => SMNBattleData.Instance.IfritGemshineTimes,
                ActivePetType.Garuda => SMNBattleData.Instance.GarudaGemshineTimes,
                _ => 0
            };
            if (Core.Get<IMemApiSummoner>().ActivePetType != ActivePetType.None)
            {
                if (timesLeft <= 0)
                {
                    return -2;
                }
                if (Core.Get<IMemApiMove>().IsMoving() && !Core.Me.HasMyAura(AurasDefine.Swiftcast) && Core.Get<IMemApiSummoner>().ActivePetType == ActivePetType.Ifrit)
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