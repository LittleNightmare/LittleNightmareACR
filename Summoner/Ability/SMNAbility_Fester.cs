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
            if (Core.Me.GetBuffTimespanLeft(AurasDefine.SearingLight).Milliseconds <=
                Core.Get<IMemApiSpell>().GetGCDDuration(false) * 2)
                return 0;
            var pet = Core.Get<IMemApiSummoner>().ActivePetType;
            if (pet is ActivePetType.Bahamut or ActivePetType.Phoneix)
            {
                if (SMNSpellHelper.EnkindleDemi().RecentlyUsed() || !SMNSpellHelper.EnkindleDemi().IsReady())
                {
                    // 稍微延迟一下，等一下团副上齐了再用，应该不会导致延后能量吸收
                    if (!SpellsDefine.EnergyDrain.CoolDownInGCDs(1))
                    {
                        return -2;
                    }
                    return 0;
                }
            }
            // 在灼热之光持续时间内，如果能量吸收马上冷却完成，这里已经不是巴哈或凤凰
            if (SpellsDefine.EnergyDrain.CoolDownInGCDs(1))
            {
                return 0;
            }
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