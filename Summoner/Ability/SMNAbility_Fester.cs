using CombatRoutine;
using Common;
using Common.Define;
using Common.Helper;
using Common.Language;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_Fester : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public Spell GetSpell()
    {
        if (!Qt.GetQt("AOE".Loc())) return SpellsDefine.Fester.GetSpell();
        var target = Core.Me.GetCurrTarget();

        if (SMNSettings.Instance.SmartAoETarget)
        {
            var canTargetObjects = TargetHelper.GetMostCanTargetObjects(SpellsDefine.Painflare);
            if (canTargetObjects.IsValid)
                target = canTargetObjects;
        }
        
        if (TargetHelper.CheckNeedUseAOE(target, 25, 5, 3))
        {
            return new Spell(SpellsDefine.Painflare, target);
        }
        return SpellsDefine.Fester.GetSpell();
    }
    public int Check()
    {
        if (Core.Get<IMemApiSummoner>().Aetherflow == 0)
        {
            return -10;
        }
        var spell = GetSpell();
        if (!spell.IsReady())
        {
            return -10;
        }
        if (Qt.GetQt("最终爆发"))
        {
            return 0;
        }

        if (SMNSettings.Instance.PreventDoubleFester)
        {
            if (Core.Get<IMemApiSpellCastSucces>().IsRecentlyUsed(GetSpell().Id)
                && !SpellsDefine.EnergyDrain.CoolDownInGCDs(1))
            {
                return -2;
            }
        }
        
        if (!SpellsDefine.SearingLight.IsLevelEnough())
        {
            return 0;
        }

        if (Core.Me.HasMyAura(AurasDefine.SearingLight))
        {
            // 应该不需要这个
            // if (!Core.Me.HasMyAuraWithTimeleft(AurasDefine.SearingLight, Core.Get<IMemApiSpell>().GetGCDDuration(false) * 2))
            // {
            //     return 0;
            // }

            // 在灼热之光持续时间内，如果能量吸收马上冷却完成，还是直接用吧
            if (SpellsDefine.EnergyDrain.CoolDownInGCDs(2))
            {
                return 0;
            }
            if (Core.Get<IMemApiSummoner>().InBahamut || Core.Get<IMemApiSummoner>().InPhoenix)
            {
                // 等待使用巴哈或凤凰的能力技，有设计等待时间
                if (!SMNSpellHelper.EnkindleDemi().RecentlyUsed() && SMNSpellHelper.EnkindleDemi().IsReady())
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