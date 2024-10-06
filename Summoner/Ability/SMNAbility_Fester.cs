using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_Fester : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public Spell GetSpell()
    {
        if (!SummonerRotationEntry.QT.GetQt("AOE")) return SMNData.Spells.Fester.GetSpell();

        if (SMNSettings.Instance.SmartAoETarget)
        {
            var canTargetObjects = TargetHelper.GetMostCanTargetObjects(SMNData.Spells.Painflare);
            if (canTargetObjects != null && canTargetObjects.IsValid())
                return new Spell(SMNData.Spells.Painflare, canTargetObjects);
        }else
        {
            var currentTarget = Core.Me.GetCurrTarget();
            if (currentTarget != null && TargetHelper.GetNearbyEnemyCount(currentTarget, 25, 5) >= 3)
                return SMNData.Spells.Painflare.GetSpell();
        }
        return SMNData.Spells.Fester.GetSpell();
    }
    public int Check()
    {
        if (Core.Resolve<JobApi_Summoner>().AetherflowStacks == 0)
        {
            return -10;
        }
        var spell = GetSpell();
        if (!spell.Id.IsReady())
        {
            return -10;
        }
        if (SummonerRotationEntry.QT.GetQt("最终爆发"))
        {
            return 0;
        }

        if (SMNSettings.Instance.PreventDoubleFester)
        {
            if (Core.Resolve<MemApiSpellCastSuccess>().IsRecentlyUsed(GetSpell().Id)
                && !SMNData.Spells.EnergyDrain.CoolDownInGCDs(1))
            {
                return -2;
            }
        }
        
        if (!SMNData.Spells.SearingLight.IsLevelEnough())
        {
            return 0;
        }

        if (Core.Me.HasAura(AurasDefine.SearingLight))
        {
            // 应该不需要这个
            // if (!Core.Me.HasMyAuraWithTimeleft(AurasDefine.SearingLight, Core.Resolve<MemApiSpell>().GetGCDDuration(false) * 2))
            // {
            //     return 0;
            // }

            // 在灼热之光持续时间内，如果能量吸收马上冷却完成，还是直接用吧
            if (SMNData.Spells.EnergyDrain.CoolDownInGCDs(2))
            {
                return 0;
            }
            if (SMNHelper.InBahamut || SMNHelper.InPhoenix || SMNHelper.InSolarBahamut)
            {
                // 等待使用巴哈或凤凰的能力技，有设计等待时间
                if (!SMNHelper.EnkindleDemi().RecentlyUsed() && SMNHelper.EnkindleDemi().Id.IsReady())
                {
                    return -2;
                }
            }

            return 0;
        }

        return -1;
    }

    public void Build(Slot slot)
    {
        var spell = GetSpell();
        // if (spell == null)
        //     return;
        slot.Add(spell);
    }
}