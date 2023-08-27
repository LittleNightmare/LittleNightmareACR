using CombatRoutine;
using Common;
using Common.Define;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_SummonCarbuncle : ISlotResolver
    {
        public Spell GetSpell()
        {
            return SpellsDefine.SummonCarbuncle.GetSpell();
        }
        public SlotMode SlotMode { get; } = SlotMode.Gcd;
        public int Check()
        {
            if (Core.Get<IMemApiSummoner>().HasPet)
            {
                return -10;
            }

            if (!GetSpell().IsReady())
            {
                return -10;
            }
            if (Core.Get<IMemApiSummoner>().TranceTimer > 0)
            {
                return -2;
            }
            if (Core.Get<IMemApiSummoner>().PetTimer > 0)
            {
                return -2;
            }
            if (Core.Get<IMemApiMove>().IsMoving())
            {
                return -1;
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