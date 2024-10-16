using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using FFXIVClientStructs;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_Resurrection : ISlotResolver
    {
        public SlotMode SlotMode { get; } = SlotMode.Gcd;

        public int Check()
        {
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
                return 0;
            }

            return -1;
        }

        public void Build(Slot slot)
        {
            var skillTarget = PartyHelper.DeadAllies.FirstOrDefault(r => !r.HasAura(SMNData.Buffs.Raise));
            if (skillTarget != null)
            {
                slot.Add(new Spell(SMNData.Spells.Resurrection, skillTarget));
            }
        }
    }
}