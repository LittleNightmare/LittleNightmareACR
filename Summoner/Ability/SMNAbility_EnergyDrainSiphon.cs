using CombatRoutine;
using Common;
using Common.Define;
using Common.Helper;
using Common.Language;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_EnergyDrainSiphon : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public Spell GetSpell()
    {
        if (!Qt.GetQt("AOE".Loc())) return SpellsDefine.EnergyDrain.GetSpell();
        if (SMNSettings.Instance.SmartAoETarget)
        {
            var canTargetObjects = TargetHelper.GetMostCanTargetObjects(SpellsDefine.EnergySiphon, 2);
            if (canTargetObjects.IsValid)
                return new Spell(SpellsDefine.EnergySiphon.GetSpell().Id, canTargetObjects);
        }else if (TargetHelper.CheckNeedUseAOE(Core.Me.GetCurrTarget(), 25, 5, 2))
        {
            return SpellsDefine.EnergySiphon.GetSpell();
        }

        return SpellsDefine.EnergyDrain.GetSpell();
    }
    public int Check()
    {
        if (!GetSpell().IsReady())
        {
            return -10;
        }
        if (!Core.Get<IMemApiSummoner>().HasAetherflowStacks)
        {
            return 0;
        }
        return -1;
    }

    public void Build(Slot slot)
    {
        slot.Add(GetSpell());
    }
}