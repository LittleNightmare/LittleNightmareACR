using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_Resurrection : ISlotResolver
    {
        public SlotMode SlotMode { get; } = SlotMode.Gcd;

        public int Check()
        {
            //TODO 增加读条模式，或者用hotkey触发？直接放到hotkey可能导致高优先级一直卡着不放弃？
            if (SMNSettings.Instance.SwiftCastMode is not (0 or 3))
            {
                return -10;
            }
            // 这里用isReady的结果一直是false，只能用unlock判断，不确定为啥
            if (!SMNData.Spells.Resurrection.IsUnlock()) return -2;
            if (!Core.Me.HasAura(SMNData.Buffs.Swiftcast))
            {
                return -9;
            }

            if (Core.Me.CurrentMp < 2400)
            {
                return -8;
            }

            var skillTarget = PartyHelper.DeadAllies.FirstOrDefault(r => !r.HasAura(SMNData.Buffs.Raise));
            if (skillTarget != null && skillTarget.IsValid() && skillTarget.IsTargetable)
            {
                if (new Spell(SMNData.Spells.Resurrection, skillTarget).IsReadyWithCanCast()) return 0;
            }

            return -1;
        }

        public void Build(Slot slot)
        {
            // TODO：这里可能会导致没有add，这会导致报错。但说实话，正常不应该出现这种情况。就check过了，到实际调用中间tmd的人活了
            var skillTarget = PartyHelper.DeadAllies.FirstOrDefault(r => !r.HasAura(SMNData.Buffs.Raise));
            if (skillTarget != null && skillTarget.IsValid())
            {
                SummonerRotationEntry.SMNHintManager.TriggerHint("复活", customContent: $"复活: {skillTarget.Name}");
                slot.Add(new Spell(SMNData.Spells.Resurrection, skillTarget));
            }
        }
    }
}