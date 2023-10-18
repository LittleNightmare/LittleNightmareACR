using CombatRoutine;
using Common;
using Common.Define;
using Common.Language;

namespace LittleNightmare.Summoner
{
    public class SMNBattleData
    {
        public static SMNBattleData Instance = new();
        public List<Spell> Summon = new();
        public List<Spell> CustomSummon = new();
        // 剩余次数
        public int TitanGemshineTimes = 4;
        public int IfritGemshineTimes = 2;
        public int GarudaGemshineTimes = 4;
        public int TitanGemshineTimesCustom = 4;
        public int IfritGemshineTimesCustom = 2;
        public int GarudaGemshineTimesCustom = 4;
        // public bool In90Opener = false;

        public void UpdateSummon()
        {
            Summon.Clear();
            TitanGemshineTimes = 4 - (4 - TitanGemshineTimesCustom);
            IfritGemshineTimes = 2 - (2 - IfritGemshineTimesCustom);
            GarudaGemshineTimes = 4 - (4- GarudaGemshineTimesCustom);
        }

        public void OpenerSummon()
        {
            Summon.Clear();
            Summon.Add(SMNSpellHelper.Titan());
            Summon.Add(SMNSpellHelper.Garuda());
            Summon.Add(SMNSpellHelper.Ifrit());
        }

        public void UsedSummon()
        {
            if (Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Titan) && !Summon.Contains(SMNSpellHelper.Titan()))
            {
                Summon.Add(SMNSpellHelper.Titan());
            }
            if (Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Ifrit) && !Summon.Contains(SMNSpellHelper.Ifrit()))
            {
                Summon.Add(SMNSpellHelper.Ifrit());
            }
            if (Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Garuda) && !Summon.Contains(SMNSpellHelper.Garuda()))
            {
                Summon.Add(SMNSpellHelper.Garuda());
            }

            if (Core.Get<IMemApiSummoner>().InBahamut || Core.Get<IMemApiSummoner>().InPhoenix)
            {
                TitanGemshineTimes = 4 - (4 - TitanGemshineTimesCustom);
                IfritGemshineTimes = 2 - (2 - IfritGemshineTimesCustom);
                GarudaGemshineTimes = 4 - (4 - GarudaGemshineTimesCustom);
                
            }

            if (!Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Titan))
            {
                Summon.Remove(SMNSpellHelper.Titan());
                CustomSummon.Remove(SMNSpellHelper.Titan());
                if (Core.Get<IMemApiSummoner>().ActivePetType != ActivePetType.Titan)
                {
                    TitanGemshineTimes = 0;
                    if (CustomSummon.Count <= 0)
                    {
                        TitanGemshineTimesCustom = 4;
                    }
                }
            }
            if (!Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Ifrit))
            {
                Summon.Remove(SMNSpellHelper.Ifrit());
                CustomSummon.Remove(SMNSpellHelper.Ifrit());
                if (Core.Get<IMemApiSummoner>().ActivePetType != ActivePetType.Ifrit)
                {
                    IfritGemshineTimes = 0;
                    if(CustomSummon.Count <= 0)
                    {
                        IfritGemshineTimesCustom = 2;
                    }
                }
            }
            if (!Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Garuda))
            {
                Summon.Remove(SMNSpellHelper.Garuda());
                CustomSummon.Remove(SMNSpellHelper.Garuda());
                if (Core.Get<IMemApiSummoner>().ActivePetType != ActivePetType.Garuda)
                {
                    GarudaGemshineTimes = 0;
                    if (CustomSummon.Count <= 0)
                    {
                        GarudaGemshineTimesCustom = 4;
                    }
                }
            }

            switch (Core.Get<IMemApiSummoner>().ActivePetType)
            {
                case ActivePetType.Titan:
                    TitanGemshineTimes = Core.Get<IMemApiSummoner>().ElementalAttunement - (4 - TitanGemshineTimesCustom);
                    break;
                case ActivePetType.Ifrit:
                    IfritGemshineTimes = Core.Get<IMemApiSummoner>().ElementalAttunement - (2 - IfritGemshineTimesCustom);
                    break;
                case ActivePetType.Garuda:
                    GarudaGemshineTimes = Core.Get<IMemApiSummoner>().ElementalAttunement - (4 - GarudaGemshineTimesCustom);
                    break;
            }
        }

        public void TitanFirst()
        {
            if (Summon.Contains(SMNSpellHelper.Titan()))
            {
                Summon.Remove(SMNSpellHelper.Titan());
                Summon.Insert(0, SMNSpellHelper.Titan());
            }
        }
        public void IfritFirst()
        {
            if (Summon.Contains(SMNSpellHelper.Ifrit()))
            {
                Summon.Remove(SMNSpellHelper.Ifrit());
                Summon.Insert(0, SMNSpellHelper.Ifrit());
            }
        }
        public void GarudaFirst()
        {
            if (Summon.Contains(SMNSpellHelper.Garuda()))
            {
                Summon.Remove(SMNSpellHelper.Garuda());
                Summon.Insert(0, SMNSpellHelper.Garuda());
            }
        }

        public bool CastSwiftCastCouldCoverTargetSpell()
        {
            var leftGCD = GCDLeftUntilNextSwiftCasted();
            return leftGCD is >= 0 and < 4 && Qt.GetQt("预读风神即刻咏唱".Loc()) &&
                   (Core.Get<IMemApiSummoner>().ActivePetType != ActivePetType.Ifrit || Instance.IfritGemshineTimes <= 0);
        }

        public int GCDLeftUntilNextSwiftCasted()
        {
            var targetSpell = SMNSpellHelper.Garuda();

            if (!SpellsDefine.Slipstream.IsUnlock()) return -1;

            var GCDLeft = Core.Get<IMemApiSummoner>().TranceTimer > 0 ? Core.Get<IMemApiSummoner>().ElementalAttunement : 0;
            var list = Instance.CustomSummon.Count > 0 ? Instance.CustomSummon : Instance.Summon;
            foreach (var pet in list)
            {

                if (pet == SMNSpellHelper.Titan())
                {
                    GCDLeft += Instance.TitanGemshineTimes + 1;
                }

                if (pet == SMNSpellHelper.Ifrit())
                {
                    GCDLeft += Instance.IfritGemshineTimes + 3;
                }

                if (pet == SMNSpellHelper.Garuda())
                {
                    GCDLeft += 1;
                }

                if (pet == targetSpell)
                {
                    return GCDLeft;
                }

            }
            return -1;
        }

        public void Reset()
        {
            Instance = new();
        }

    }
}