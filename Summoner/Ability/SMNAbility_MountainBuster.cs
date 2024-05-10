using CombatRoutine;
using Common;
using Common.Define;
using Common.Language;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_MountainBuster : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public Spell GetSpell()
    {
        if (!Qt.GetQt("AOE".Loc())) return SpellsDefine.MountainBuster.GetSpell();
        if (!SMNSettings.Instance.SmartAoETarget) return SpellsDefine.MountainBuster.GetSpell();
        var canTargetObjects = TargetHelper.GetMostCanTargetObjects(SpellsDefine.MountainBuster, 2);
        return canTargetObjects.IsValid ? new Spell(SpellsDefine.MountainBuster, canTargetObjects) : SpellsDefine.MountainBuster.GetSpell();
    }
    public int Check()
    {
        if (!SpellsDefine.MountainBuster.IsReady())
        {
            return -10;
        }
        if (Core.Me.HasMyAura(AurasDefine.TitansFavor))
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