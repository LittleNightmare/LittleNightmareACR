using CombatRoutine;
using Common;
using Common.Define;
using Common.Language;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_BahamutPhoenix : ISlotResolver
    {
        public Spell GetSpell()
        {
            if (Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Bahamut) || Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Phoneix))
            {
                return SMNSpellHelper.BahamutPhoneix();
            }
            return null;
        }
        public SlotMode SlotMode { get; } = SlotMode.Gcd;
        public int Check()
        {
            if (!GetSpell().IsReady())
            {
                return -10;
            }
            if (!Core.Me.InCombat)
            {
                return -9;
            }
            if (Core.Get<IMemApiSummoner>().PetTimer > 0)
            {
                return -8;
            }
            if (!Qt.GetQt("爆发".Loc()))
            {
                return -2;
            }
            if (!Qt.GetQt("巴哈凤凰".Loc()))
            {
                return -2;
            }
            return 0;
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