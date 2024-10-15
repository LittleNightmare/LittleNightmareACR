using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using FFXIVClientStructs;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace LittleNightmare.Summoner.GCD
{
    public class SMNGCD_Resurrection : ISlotResolver
    {
        public SlotMode SlotMode { get; } = SlotMode.Gcd;

        public int Check()
        {
            if (SMNSettings.Instance.SwiftCastMode != 0)
            {
                return -10;
            }

            if (!SMNData.Spells.Resurrection.IsReady()) return -2;
            if (!Core.Me.HasAura(SMNData.Buffs.Swiftcast))
            {
                return -9;
            }

            if (Core.Me.CurrentMp < 2400)
            {
                return -8;
            }

            var skillTarget = PartyHelper.DeadAllies.FirstOrDefault(r => !r.HasAura(SMNData.Buffs.Raise));
            if (skillTarget != null && skillTarget.IsValid())
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