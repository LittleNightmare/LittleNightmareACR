using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.Helper;
using AEAssist.JobApi;


namespace LittleNightmare.Summoner
{
    public class SMNBattleData
    {
        public static SMNBattleData Instance = new();
        public List<Spell> Summon = new();
        public List<Spell> CustomSummon = new();

        public List<Spell> CustomSummonWaitList = new();

        // 剩余次数
        public int TitanGemshineTimes = 4;
        public int IfritGemshineTimes = 2;
        public int GarudaGemshineTimes = 4;
        public int TitanGemshineTimesCustom = 4;
        public int IfritGemshineTimesCustom = 2;
        public int GarudaGemshineTimesCustom = 4;
        // public bool In90Opener = false;

        public bool FinalBoss = false;

        public bool TTKTriggered = false;

        public void UpdateSummon()
        {
            Summon.Clear();
            TitanGemshineTimes = 4 - (4 - TitanGemshineTimesCustom);
            IfritGemshineTimes = 2 - (2 - IfritGemshineTimesCustom);
            GarudaGemshineTimes = 4 - (4 - GarudaGemshineTimesCustom);
        }

        public void OpenerSummon()
        {
            Summon.Clear();
            Summon.Add(SMNHelper.Titan());
            Summon.Add(SMNHelper.Garuda());
            Summon.Add(SMNHelper.Ifrit());
        }

        public void UsedSummon()
        {
            if (Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Titan) && !Summon.Contains(SMNHelper.Titan()))
            {
                Summon.Add(SMNHelper.Titan());
            }

            if (Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Ifrit) && !Summon.Contains(SMNHelper.Ifrit()))
            {
                Summon.Add(SMNHelper.Ifrit());
            }

            if (Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Garuda) &&
                !Summon.Contains(SMNHelper.Garuda()))
            {
                Summon.Add(SMNHelper.Garuda());
            }


            if (SMNHelper.InAnyDemi)
            {
                TitanGemshineTimes = 4 - (4 - TitanGemshineTimesCustom);
                IfritGemshineTimes = 2 - (2 - IfritGemshineTimesCustom);
                GarudaGemshineTimes = 4 - (4 - GarudaGemshineTimesCustom);
            }

            if (!Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Titan))
            {
                Summon.Remove(SMNHelper.Titan());
                CustomSummon.Remove(SMNHelper.Titan());
                if (Core.Resolve<JobApi_Summoner>().ActivePetType != ActivePetType.Titan)
                {
                    TitanGemshineTimes = 0;
                    if (CustomSummon.Count <= 0)
                    {
                        TitanGemshineTimesCustom = 4;
                    }
                }
            }

            if (!Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Ifrit))
            {
                Summon.Remove(SMNHelper.Ifrit());
                CustomSummon.Remove(SMNHelper.Ifrit());
                if (Core.Resolve<JobApi_Summoner>().ActivePetType != ActivePetType.Ifrit)
                {
                    IfritGemshineTimes = 0;
                    if (CustomSummon.Count <= 0)
                    {
                        IfritGemshineTimesCustom = 2;
                    }
                }
            }

            if (!Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Garuda))
            {
                Summon.Remove(SMNHelper.Garuda());
                CustomSummon.Remove(SMNHelper.Garuda());
                if (Core.Resolve<JobApi_Summoner>().ActivePetType != ActivePetType.Garuda)
                {
                    GarudaGemshineTimes = 0;
                    if (CustomSummon.Count <= 0)
                    {
                        GarudaGemshineTimesCustom = 4;
                    }
                }
            }

            if (CustomSummonWaitList.Count > 0)
            {
                if (Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Titan)
                    && Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Ifrit)
                    && Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Garuda))
                {
                    foreach (var summon in CustomSummonWaitList)
                    {
                        CustomSummon.Add(summon);
                    }

                    CustomSummonWaitList.Clear();
                }
            }

            switch (Core.Resolve<JobApi_Summoner>().ActivePetType)
            {
                case ActivePetType.Titan:
                    TitanGemshineTimes = Core.Resolve<JobApi_Summoner>().AttunementAdjust -
                                         (4 - TitanGemshineTimesCustom);
                    break;
                case ActivePetType.Ifrit:
                    IfritGemshineTimes = Core.Resolve<JobApi_Summoner>().AttunementAdjust -
                                         (2 - IfritGemshineTimesCustom);
                    break;
                case ActivePetType.Garuda:
                    GarudaGemshineTimes = Core.Resolve<JobApi_Summoner>().AttunementAdjust -
                                          (4 - GarudaGemshineTimesCustom);
                    break;
            }
        }

        public void TitanFirst()
        {
            if (Summon.Contains(SMNHelper.Titan()))
            {
                Summon.Remove(SMNHelper.Titan());
                Summon.Insert(0, SMNHelper.Titan());
            }
        }

        public void IfritFirst()
        {
            if (Summon.Contains(SMNHelper.Ifrit()))
            {
                Summon.Remove(SMNHelper.Ifrit());
                Summon.Insert(0, SMNHelper.Ifrit());
            }
        }

        public void GarudaFirst()
        {
            if (Summon.Contains(SMNHelper.Garuda()))
            {
                Summon.Remove(SMNHelper.Garuda());
                Summon.Insert(0, SMNHelper.Garuda());
            }
        }

        //public bool CastSwiftCastCouldCoverTargetSpell()
        //{
        //    var leftGCD = GCDLeftUntilNextSwiftCasted();
        //    return leftGCD is >= 0 and < 4 && SummonerRotationEntry.QT.GetQt("预读风神即刻咏唱") &&
        //           (Core.Resolve<JobApi_Summoner>().ActivePetType != ActivePetType.Ifrit || Instance.IfritGemshineTimes <= 0);
        //}

        // public int GCDLeftUntilNextSwiftCasted()
        // {
        //     var targetSpell = SMNSpellHelper.Garuda();
        //
        //     if (!SMNData.Spells.Slipstream.IsUnlock()) return -1;
        //
        //     var GCDLeft = Core.Resolve<JobApi_Summoner>().TranceTimer > 0 ? Core.Resolve<JobApi_Summoner>().ElementalAttunement : 0;
        //     var list = Instance.CustomSummon.Count > 0 ? Instance.CustomSummon : Instance.Summon;
        //     foreach (var pet in list)
        //     {
        //
        //         if (pet == SMNSpellHelper.Titan())
        //         {
        //             GCDLeft += Instance.TitanGemshineTimes + 1;
        //         }
        //
        //         if (pet == SMNSpellHelper.Ifrit())
        //         {
        //             GCDLeft += Instance.IfritGemshineTimes + 3;
        //         }
        //
        //         if (pet == SMNSpellHelper.Garuda())
        //         {
        //             GCDLeft += 1;
        //         }
        //
        //         if (pet == targetSpell)
        //         {
        //             return GCDLeft;
        //         }
        //
        //     }
        //     return -1;
        // }

        public void Reset()
        {
            Instance = new SMNBattleData();
        }
    }
}