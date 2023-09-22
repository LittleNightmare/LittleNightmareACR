using CombatRoutine;
using Common;
using Common.Define;
using Common.Helper;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_Fester : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public Spell GetSpell()
    {
        if (!Qt.GetQt("AOE")) return SpellsDefine.Fester.GetSpell();
        if (TargetHelper.CheckNeedUseAOE(Core.Me.GetCurrTarget(), 25, 5, 3))
        {
            return SpellsDefine.Painflare.GetSpell();
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
        if (Core.Get<IMemApiSpellCastSucces>().IsRecentlyUsed(GetSpell().Id) 
            && !SpellsDefine.EnergyDrain.CoolDownInGCDs(1))
        {
            return -2;
        }
        if (!Core.Get<IMemApiSpell>().HasLearn(SpellsDefine.SearingLight))
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
            var isBahamutOrPhoenix = Core.Get<IMemApiSummoner>().TranceTimer <= 0 && Core.Get<IMemApiSummoner>().PetTimer > 0 && Core.Get<IMemApiSummoner>().ActivePetType == ActivePetType.None;
            if (isBahamutOrPhoenix)
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