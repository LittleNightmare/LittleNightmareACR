using CombatRoutine;
using Common;
using Common.Define;
using Common.Helper;
using Common.Language;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_BahamutPhoenixGCD : ISlotResolver
    {
        public Spell GetSpell()
        {
            if (!Qt.GetQt("AOE".Loc())) return SMNSpellHelper.BaseSingle();
            var aoeCount = 0;
            if (Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Bahamut))
            {
                aoeCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5);
            }
            if (Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Phoneix))
            {
                aoeCount = TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 8);
            }
            if (aoeCount >= 3)
            {
                return SMNSpellHelper.BaseAoE();
            }
            return SMNSpellHelper.BaseSingle();
        }
        public SlotMode SlotMode { get; } = SlotMode.Gcd;
        public int Check()
        {
            if (!GetSpell().IsReady())
            {
                return -10;
            }
            if(Core.Get<IMemApiSummoner>().TranceTimer > 0)
            {
                return -10;
            }
            if (Core.Get<IMemApiSummoner>().PetTimer > 0)
            {
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