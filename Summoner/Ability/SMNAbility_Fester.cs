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
        if (Core.Get<IMemApiSpellCastInfo>().LastSpellId == spell.Id && !Qt.GetQt("最终爆发"))
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
                if (SMNSpellHelper.EnkindleDemi().RecentlyUsed())
                {
                    return 0;
                }
            }

            if (SpellsDefine.EnergyDrain.IsReady() || SpellsDefine.EnergySiphon.IsReady())
            {
                return 0;
            }
        }
        if (Qt.GetQt("最终爆发"))
        {
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