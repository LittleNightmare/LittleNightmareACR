using CombatRoutine;
using Common;
using Common.Define;

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
            if (!SpellsDefine.Resurrection.IsReady()) return -2;
            if (!Core.Me.HasMyAura(AurasDefine.Swiftcast))
            {
                return -9;
            }
            if (Core.Me.CurrentMana < 2400)
            {
                return -8;
            }
            var skillTarget = PartyHelper.DeadAllies.FirstOrDefault(r=>!r.HasAura(AurasDefine.Raise));
            if (skillTarget.IsValid)
            {
                return 0;
            }
            return -1;
        }

        public void Build(Slot slot)
        {
            var skillTarget = PartyHelper.DeadAllies.FirstOrDefault(r=>!r.HasAura(AurasDefine.Raise));
            slot.Add(new Spell(SpellsDefine.Resurrection,skillTarget));           
        }
    }
}