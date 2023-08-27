using System.Diagnostics;
using CombatRoutine;
using Common;
using Common.Define;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_SwiftCast : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;
    public int Check()
    {
        if (!SpellsDefine.Swiftcast.IsReady())
        {
            return -10;
        }
        var skillTarget = PartyHelper.DeadAllies.FirstOrDefault(r => !r.HasAura(AurasDefine.Raise));
        if (SMNSettings.Instance.即刻咏唱模式 == 0 && skillTarget.IsValid)
        {
            return 0;
        }
        if ((SMNSettings.Instance.即刻咏唱模式 == 1 || SMNBattleData.Instance.In90Opener))
        {
            if (Core.Me.HasMyAura(AurasDefine.GarudasFavor) || SMNBattleData.Instance.CastSwiftCastCouldCoverTargetSpell())
            {
                return 0;
            }
            
        }
        if (SMNSettings.Instance.即刻咏唱模式 == 2)
        {
            if (Core.Get<IMemApiSummoner>().ActivePetType == ActivePetType.Ifrit)
            {
                return 0;
            }
        }
        return -1;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDefine.Swiftcast.GetSpell());
    }

}