using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.AILoop;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;

namespace LittleNightmare.Summoner
{
    public class OpenerSMN90FastEnergyDrain : IOpener
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

            if (!Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Bahamut))
            {
                return -8;
            }

            if (!SMNData.Spells.SummonBahamut.GetSpell().IsReadyWithCanCast())
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

        public List<Action<Slot>> Sequence { get; } =
        [
            Step0,
            Step1,
            Step2,
            Step3,
            Step4,
            Step5,
        ];

        public Action CompeltedAction { get; set; }

        private static void Step0(Slot slot)
        {
            SummonerRotationEntry.SMNHintManager.TriggerHint("FastEnergyDrainOpener");
            slot.Add(new Spell(SMNData.Spells.SearingLight, SpellTargetType.Self));
            slot.Add(new Spell(SMNData.Spells.SummonBahamut, SpellTargetType.Target));
            slot.Add(new Spell(SMNData.Spells.EnergyDrain, SpellTargetType.Target));
        }

        private static void Step1(Slot slot)
        {
            slot.Add(new Spell(SMNHelper.BaseSingle().Id, SpellTargetType.Target));
            if (SummonerRotationEntry.QT.GetQt("爆发药"))
            {
                slot.Add(Spell.CreatePotion());
            }

            if (Core.Me.Level >= 86)
            {
                SMNBattleData.Instance.OpenerSummon();
            }
        }

        private static void Step2(Slot slot)
        {
            slot.Add(new Spell(SMNHelper.BaseSingle().Id, SpellTargetType.Target));
        }

        private static void Step3(Slot slot)
        {
            slot.Add(new Spell(SMNHelper.BaseSingle().Id, SpellTargetType.Target));
            slot.Add(new Spell(SMNData.Spells.EnkindleBahamut, SpellTargetType.Target));
        }

        private static void Step4(Slot slot)
        {
            slot.Add(new Spell(SMNHelper.BaseSingle().Id, SpellTargetType.Target));
            slot.Add(new Spell(SMNData.Spells.Deathflare, SpellTargetType.Target));
            slot.Add(new Spell(SMNData.Spells.Fester, SpellTargetType.Target));
        }

        private static void Step5(Slot slot)
        {
            slot.Add(new Spell(SMNHelper.BaseSingle().Id, SpellTargetType.Target));
            slot.Add(new Spell(SMNData.Spells.Fester, SpellTargetType.Target));
        }

        public uint Level { get; } = 90;

        public void InitCountDown(CountDownHandler countDownHandler)
        {
            if (!Core.Resolve<JobApi_Summoner>().HasPet)
            {
                countDownHandler.AddAction(30000, SMNData.Spells.SummonCarbuncle, SpellTargetType.Self);
            }

            countDownHandler.AddAction(1500, SMNHelper.BaseSingle().Id, SpellTargetType.Target);
        }
    }
}