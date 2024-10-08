using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_MountainBuster : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public Spell GetSpell()
    {
        if (!SummonerRotationEntry.QT.GetQt("AOE")) return SMNData.Spells.MountainBuster.GetSpell();
        if (!SMNSettings.Instance.SmartAoETarget) return SMNData.Spells.MountainBuster.GetSpell();
        var canTargetObjects = TargetHelper.GetMostCanTargetObjects(SMNData.Spells.MountainBuster, 2);
        return canTargetObjects != null && canTargetObjects.IsValid()
            ? new Spell(SMNData.Spells.MountainBuster, canTargetObjects)
            : SMNData.Spells.MountainBuster.GetSpell();
    }

    public int Check()
    {
        if (!SMNData.Spells.MountainBuster.IsReady())
        {
            return -10;
        }

        if (Core.Me.HasAura(SMNData.Buffs.TitansFavor))
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