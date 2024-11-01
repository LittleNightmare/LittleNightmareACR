using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_Aether : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public Spell GetSpell()
    {
        if (!SummonerRotationEntry.QT.GetQt("AOE")) return SMNHelper.Aether();

        if (SMNSettings.Instance.SmartAoETarget)
        {
            var canTargetObjects = TargetHelper.GetMostCanTargetObjects(SMNHelper.AetherAOE().Id);
            if (canTargetObjects != null && canTargetObjects.IsValid())
                return new Spell(SMNHelper.AetherAOE().Id, canTargetObjects);
        }
        else
        {
            var currentTarget = Core.Me.GetCurrTarget();
            if (currentTarget != null && TargetHelper.GetNearbyEnemyCount(currentTarget, 25, 5) >= 3)
                return SMNHelper.AetherAOE();
        }

        return SMNHelper.Aether();
    }

    public int Check()
    {
        if (Core.Resolve<JobApi_Summoner>().AetherflowStacks == 0)
        {
            return -9;
        }

        var spell = GetSpell();
        if (!spell.Id.IsUnlock())
        {
            return -11;
        }

        if (!spell.IsReadyWithCanCast())
        {
            return -10;
        }

        if (SummonerRotationEntry.QT.GetQt("最终爆发"))
        {
            return 1;
        }

        if (SMNSettings.Instance.PreventDoubleFester)
        {
            if ((Core.Resolve<MemApiSpellCastSuccess>().IsRecentlyUsed(SMNHelper.Aether().Id) 
                 || Core.Resolve<MemApiSpellCastSuccess>().IsRecentlyUsed(SMNHelper.AetherAOE().Id))
                && !SMNData.Spells.EnergyDrain.CoolDownInGCDs(1))
            {
                return -2;
            }
        }

        if (!SMNData.Spells.SearingLight.IsLevelEnough())
        {
            return 2;
        }

        if (!Core.Me.HasAura(AurasDefine.SearingLight)) return -1;
        // 应该不需要这个
        // if (!Core.Me.HasMyAuraWithTimeleft(AurasDefine.SearingLight, Core.Resolve<MemApiSpell>().GetGCDDuration(false) * 2))
        // {
        //     return 0;
        // }

        // 在灼热之光持续时间内，如果能量吸收马上冷却完成，还是直接用吧（说实话第二个应该不需要
        if (SMNData.Spells.EnergyDrain.CoolDownInGCDs(2) || SMNData.Spells.EnergyDrain.GetSpell().IsReadyWithCanCast())
        {
            return 3;
        }
        // 如果不是巴哈或凤凰状态，那么就直接用（在灼热之光持续时间内）TODO：这里考虑判断一下是否马上就要用巴哈或凤凰
        if (!SMNHelper.InBahamut && !SMNHelper.InPhoenix && !SMNHelper.InSolarBahamut) return 4;
        // 等待使用巴哈或凤凰的能力技，有设计等待时间
        if (!SMNHelper.EnkindleDemi().RecentlyUsed() && SMNHelper.EnkindleDemi().IsReadyWithCanCast())
        {
            return -2;
        }
        // 正常应该不会到这里
        return 0;

    }

    public void Build(Slot slot)
    {
        var spell = GetSpell();
        // if (spell == null)
        //     return;
        slot.Add(spell);
    }
}