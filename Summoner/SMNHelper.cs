using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;

namespace LittleNightmare.Summoner
{
    public static class SMNHelper
    {
        public static Spell Titan()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.SummonTopaz).GetSpell();
        }

        public static Spell Ifrit()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.SummonRuby.GetSpell().Id).GetSpell();
        }

        public static Spell Garuda()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.SummonEmerald.GetSpell().Id).GetSpell();
        }

        public static Spell BahamutPhoneix()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.Aethercharge).GetSpell();
        }

        public static Spell EnkindleDemi()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.EnkindleBahamut).GetSpell();
        }

        public static Spell BaseSingle()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.Ruin).GetSpell();
        }

        public static Spell BaseAoE()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.Outburst).GetSpell();
        }

        public static Spell BaseSummonSingle()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.Gemshine).GetSpell();
        }

        public static Spell BaseSummonAoE()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.PreciousBrilliance).GetSpell();
        }

        public static Spell Aether()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.Fester).GetSpell();
        }

        public static Spell AetherAOE()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(SMNData.Spells.Painflare).GetSpell();
        }

        public static bool InBahamut => Core.Resolve<JobApi_Summoner>().ActivePetType == ActivePetType.Bahamut;

        public static bool InPhoenix => Core.Resolve<JobApi_Summoner>().ActivePetType == ActivePetType.Phoneix;

        public static bool InSolarBahamut =>
            Core.Resolve<JobApi_Summoner>().ActivePetType == ActivePetType.SolarBahamut;

        public static bool InAnyDemi => InBahamut || InPhoenix || InSolarBahamut;

        /// <summary>
        /// 检查是否亚灵神快好了
        /// </summary>
        /// <param name="numberOfCoolDownInGCDs">在numberOfCoolDownInGCDs个GCD内转好</param>
        /// <returns></returns>
        public static bool DemiCoolDownAlmostOver(int numberOfCoolDownInGCDs = 3)
        {
            return SMNHelper.BahamutPhoneix().Id.CoolDownInGCDs(numberOfCoolDownInGCDs);
        }

        public static bool WithinActionAttackRange(uint spell) => Core.Resolve<MemApiSpell>().GetActionInRangeOrLoS(spell) != 566;

        public static IBattleChara? GetDeadChara(bool useRangeTargets = false)
        {
            var targets = useRangeTargets ? ECHelper.Objects.OfType<IBattleChara>().Where(r => r is { IsDead: true, ObjectKind: ObjectKind.Player }).ToList() : PartyHelper.DeadAllies;
            var target = targets.FirstOrDefault(r => !r.HasAura(SMNData.Buffs.Raise) && r.IsTargetable && r.IsValid() && r.Name.TextValue != Core.Me.Name.TextValue && Core.Me.Distance(r) < 30);
            var currentTarget = Core.Me.GetCurrTarget();
            if (currentTarget is { ObjectKind: ObjectKind.Player, IsDead: true, IsTargetable: true }
                && currentTarget.IsValid()
                && !currentTarget.HasAura(SMNData.Buffs.Raise)
                && currentTarget.Name.TextValue != Core.Me.Name.TextValue
                && Core.Me.Distance(currentTarget) < 30)
                target = currentTarget;
            return target;
        }
    }
}