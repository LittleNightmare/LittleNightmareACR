using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;

namespace LittleNightmare.Summoner.Ability;

public class SMNAbility_SwiftCast : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public int Check()
    {
        if (!SpellsDefine.Swiftcast.GetSpell().IsReadyWithCanCast())
        {
            return -10;
        }

        var skillTarget = PartyHelper.DeadAllies.FirstOrDefault(r => !r.HasAura(SMNData.Buffs.Raise));

        if (SMNSettings.Instance.SwiftCastMode is 0 or 3 && skillTarget != null && skillTarget.IsValid() && skillTarget.IsTargetable)
        {
            SummonerRotationEntry.SMNHintManager.TriggerHint("即刻", customContent: "准备即刻，目的: 拉人", customTTS:"准备即刻拉人");
            return 0;
        }

        if (SMNSettings.Instance.SwiftCastMode is 1 or 3)
        {
            if (Core.Me.HasAura(SMNData.Buffs.GarudasFavor))
            {
                SummonerRotationEntry.SMNHintManager.TriggerHint("即刻", customContent: "准备即刻，目的: 风圈", customTTS: "准备即刻风圈");
                return 1;
            }
        }

        if (SMNSettings.Instance.SwiftCastMode is 2 or 3)
        {
            if (Core.Resolve<JobApi_Summoner>().ActivePetType == ActivePetType.Ifrit)
            {
                SummonerRotationEntry.SMNHintManager.TriggerHint("即刻", customContent: "准备即刻，目的: 火读条", customTTS: "准备即刻火");
                return 2;
            }
        }

        return -1;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDefine.Swiftcast.GetSpell());
    }
}