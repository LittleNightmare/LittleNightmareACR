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
            if (Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Titan)
                || Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Ifrit)
                || Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Garuda))
            {
                if (SMNBattleData.Instance.CustomSummon.Count > 0 && !SMNBattleData.Instance.In90Opener)
                {
                    return SMNBattleData.Instance.CustomSummon[0];
                }
                return SMNBattleData.Instance.Summon[0];
            }
            return null;
        }
        public SlotMode SlotMode { get; } = SlotMode.Gcd;
        public int Check()
        {
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
                if (Core.Get<IMemApiMove>().IsMoving() && Core.Me.HasAura(AurasDefine.IfritsFavor) && Core.Me.HasAura(AurasDefine.FurtherRuin))
                {
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