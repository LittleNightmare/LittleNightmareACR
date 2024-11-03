using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;

namespace LittleNightmare.Summoner.Ability
{
    public class SMNAbility_SearingFlash : ISlotResolver
    {
        public Spell GetSpell()
        {
            return SMNData.Spells.SearingFlash.GetSpell();
        }

        public SlotMode SlotMode { get; } = SlotMode.OffGcd;

        public int Check()
        {
            var spell = GetSpell();
            if (!spell.IsReadyWithCanCast())
            {
                return -10;
            }

            if (!spell.IsUnlock())
            {
                return -9;
            }

            if (!Core.Me.HasAura(SMNData.Buffs.RubysGlimmer))
            {
                return -8;
            }

            if (SummonerRotationEntry.QT.GetQt("最终爆发"))
            {
                return 0;
            }

            if (Core.Me.HasAura(AurasDefine.SearingLight))
            {
                if (SMNHelper.InAnyDemi)
                {
                    // 等待使用巴哈或凤凰的能力技
                    if (!SMNHelper.EnkindleDemi().RecentlyUsed() && SMNHelper.EnkindleDemi().IsReadyWithCanCast())
                    {
                        return -2;
                    }
                }

                return 0;
            }

            return -1;
        }

        public void Build(Slot slot)
        {
            var spell = GetSpell();
            slot.Add(spell);
        }
    }
}