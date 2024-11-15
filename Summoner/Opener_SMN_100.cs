using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;

namespace LittleNightmare.Summoner
{
    public class OpenerSMN100 : IOpener
    {
        public int StartCheck()
        {
            var currentTarget = Core.Me.GetCurrTarget();
            if (PartyHelper.Party.Count <= 4 && currentTarget != null && !currentTarget.IsDummy())
            {
                return -10;
            }

            if (!Core.Resolve<JobApi_Summoner>().HasPet)
            {
                return -9;
            }

            if (!Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.SolarBahamut))
            {
                return -8;
            }

            if (!SMNData.Spells.SummonSolarBahamut.GetSpell().IsReadyWithCanCast())
            {
                return -7;
            }

            if (!SMNData.Spells.SearingLight.GetSpell().IsReadyWithCanCast())
            {
                return -6;
            }
            if (AI.Instance.BattleData.CurrBattleTimeInMs > 5000)
            {
                return -5;
            }

            return 0;
        }

        public int StopCheck(int index)
        {
            return -1;
        }

        public List<Action<Slot>> Sequence { get; } = new List<Action<Slot>>()
        {
            Step0,
            Step1,
            Step2,
            Step3,
            Step4,
            Step5,
            Step6,
        };

        public Action CompeltedAction { get; set; }

        private static void Step0(Slot slot)
        {
            LogHelper.Print("进入TheBalance起手");
            slot.Add(new Spell(SMNData.Spells.SummonSolarBahamut, SpellTargetType.Target));
            if (SummonerRotationEntry.QT.GetQt("爆发药"))
            {
                slot.Add(new SlotAction(SlotAction.WaitType.WaitForSndHalfWindow, 0, Spell.CreatePotion()));
            }
        }

        private static void Step1(Slot slot)
        {
            slot.Add(new Spell(SMNHelper.BaseSingle().Id, SpellTargetType.Target));
            if (Core.Me.Level >= 86)
            {
                SMNBattleData.Instance.OpenerSummon();
            }

            slot.Add(
                new SlotAction(SlotAction.WaitType.WaitForSndHalfWindow, 0, SMNData.Spells.SearingLight.GetSpell()));
        }

        private static void Step2(Slot slot)
        {
            slot.Add(new Spell(SMNHelper.BaseSingle().Id, SpellTargetType.Target));
        }

        private static void Step3(Slot slot)
        {
            slot.Add(new Spell(SMNHelper.BaseSingle().Id, SpellTargetType.Target));
            slot.Add(new Spell(SMNData.Spells.EnergyDrain, SpellTargetType.Target));
        }

        private static void Step4(Slot slot)
        {
            slot.Add(new Spell(SMNHelper.BaseSingle().Id, SpellTargetType.Target));
            slot.Add(new Spell(SMNData.Spells.EnkindleSolarBahamut, SpellTargetType.Target));
            slot.Add(new Spell(SMNData.Spells.Necrotize, SpellTargetType.Target));
        }

        private static void Step5(Slot slot)
        {
            slot.Add(new Spell(SMNHelper.BaseSingle().Id, SpellTargetType.Target));
            slot.Add(new Spell(SMNData.Spells.Sunflare, SpellTargetType.Target));
            slot.Add(new Spell(SMNData.Spells.Necrotize, SpellTargetType.Target));
        }

        private static void Step6(Slot slot)
        {
            slot.Add(new Spell(SMNHelper.BaseSingle().Id, SpellTargetType.Target));
            slot.Add(new Spell(SMNData.Spells.SearingFlash, SpellTargetType.Target));
        }

        public uint Level { get; } = 100;

        public void InitCountDown(CountDownHandler countDownHandler)
        {
            if (!Core.Resolve<JobApi_Summoner>().HasPet)
            {
                countDownHandler.AddAction(30000, SMNData.Spells.SummonCarbuncle);
            }

            countDownHandler.AddAction(1500, SMNHelper.BaseSingle().Id, SpellTargetType.Target);
        }
    }
}