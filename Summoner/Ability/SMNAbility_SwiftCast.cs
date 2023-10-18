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
        if (SMNSettings.Instance.SwiftCastMode is 0 or 3 && skillTarget.IsValid)
        {
            return 0;
        }
        if (SMNSettings.Instance.SwiftCastMode is 1 or 3)
        {
            if (Core.Me.HasMyAura(AurasDefine.GarudasFavor))
            {
                return 0;
            }
            
        }
        if (SMNSettings.Instance.SwiftCastMode is 2 or 3)
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