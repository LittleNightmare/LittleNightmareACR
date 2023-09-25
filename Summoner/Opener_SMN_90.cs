using CombatRoutine;
using CombatRoutine.Opener;
using CombatRoutine.Setting;
using Common;
using Common.Define;
using Common.Helper;
using Common.Language;

namespace LittleNightmare.Summoner
{

    public class OpenerSMN90 : IOpener
    {
        public int StartCheck()
        {
            if (PartyHelper.NumMembers <= 4 && !Core.Me.GetCurrTarget().IsDummy())
            {
                return -10;
            }
            if (!Core.Get<IMemApiSummoner>().HasPet)
            {
                return -9;
            }
            if (!Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Bahamut))
            {
                return -8;
            }
            if (!SpellsDefine.SummonBahamut.IsReady())
            {
                return -7;
            }
            if (!SpellsDefine.SearingLight.IsReady())
            {
                return -6;
            }
            if (AI.Instance.BattleData.CurrBattleTimeInMs > 5)
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
        };

        public Action CompeltedAction { get; set; }

        private static void Step0(Slot slot)
        {
            slot.Add(new Spell(SpellsDefine.SummonBahamut, SpellTargetType.Target));
            slot.Add2NdWindowAbility(new Spell(SpellsDefine.SearingLight, SpellTargetType.DefaultByCode));
        }
        private static void Step1(Slot slot)
        {
            slot.Add(new Spell(SMNSpellHelper.BaseSingle().Id, SpellTargetType.Target));
            if (Qt.GetQt("爆发药".Loc()))
            {
                slot.Add(Spell.CreatePotion());
            }
            if (Core.Me.ClassLevel >= 86)
            {
                SMNBattleData.Instance.OpenerSummon();
                SMNBattleData.Instance.In90Opener = true;
            }
        }
        private static void Step2(Slot slot)
        {
            slot.Add(new Spell(SMNSpellHelper.BaseSingle().Id, SpellTargetType.Target));
        }
        private static void Step3(Slot slot)
        {
            slot.Add(new Spell(SMNSpellHelper.BaseSingle().Id, SpellTargetType.Target));
            slot.Add(new Spell(SpellsDefine.EnergyDrain, SpellTargetType.Target));
            slot.Add(new Spell(SpellsDefine.EnkindleBahamut, SpellTargetType.Target));
        }
        private static void Step4(Slot slot)
        {
            slot.Add(new Spell(SMNSpellHelper.BaseSingle().Id, SpellTargetType.Target));
            slot.Add(new Spell(SpellsDefine.Deathflare, SpellTargetType.Target));
            slot.Add(new Spell(SpellsDefine.Fester, SpellTargetType.Target));
        }
        private static void Step5(Slot slot)
        {
            slot.Add(new Spell(SMNSpellHelper.BaseSingle().Id, SpellTargetType.Target));
            slot.Add(new Spell(SpellsDefine.Fester, SpellTargetType.Target));
        }

        public uint Level { get; } = 90;
        public void InitCountDown(CountDownHandler countDownHandler)
        {
            if (!Core.Get<IMemApiSummoner>().HasPet)
            {
                countDownHandler.AddAction(30000, SpellsDefine.SummonCarbuncle, SpellTargetType.DefaultByCode);
            }
            countDownHandler.AddAction(1500, SMNSpellHelper.BaseSingle().Id, SpellTargetType.Target);
        }
    }
}