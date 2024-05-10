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
        var target = Core.Me.GetCurrTarget();
        if (SMNSettings.Instance.SmartAoETarget)
        {
            CharacterAgent canTargetObjects = TargetHelper.GetMostCanTargetObjects(SpellsDefine.EnergySiphon);
            if (canTargetObjects.IsValid)
                target = canTargetObjects;
        }
        
        if (TargetHelper.CheckNeedUseAOE(target, 25, 5, 2))
        {
            return new Spell(SpellsDefine.EnergySiphon, target);
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