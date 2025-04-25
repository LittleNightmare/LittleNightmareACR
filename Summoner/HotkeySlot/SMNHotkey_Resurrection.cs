using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Interface.Textures.TextureWraps;
using ImGuiNET;
using System.Numerics;

#nullable disable
namespace LittleNightmare.Summoner.HotkeySlot
{
    public class SMNHotkey_Resurrection : IHotkeyResolver
    {
        private const uint SpellId = SMNData.Spells.Resurrection;

        public int Check()
        {
            if (Core.Me.CurrentMp < 2400U)
                return -2;
            if (!SpellId.IsUnlock())
            {
                return -5;
            }

            if (!SpellsDefine.Swiftcast.GetSpell().IsReadyWithCanCast() && !Core.Me.HasAura(SMNData.Buffs.Swiftcast))
                ChatHelper.SendMessage("/e 即刻咏唱CD中，改为读条复活");

            var skillTarget = SMNHelper.GetDeadChara();
            if (skillTarget != null && skillTarget.IsValid() && skillTarget.IsTargetable)
            {
                return 0;
            }
            ChatHelper.SendMessage("/e 未找到有效目标");
            return -4;
        }

        public void Draw(Vector2 size)
        {
            Vector2 size1 = size * 0.8f;
            ImGui.SetCursorPos(size * 0.1f);
            IDalamudTextureWrap textureWrap;
            if (!Core.Resolve<MemApiIcon>().GetActionTexture(SMNData.Spells.Resurrection, out textureWrap))
                return;
            ImGui.Image(textureWrap.ImGuiHandle, size1);
        }

        public void DrawExternal(Vector2 size, bool isActive)
        {
            SpellHelper.DrawSpellInfo(SpellId.GetSpell(), size, isActive);
        }

        public void Run()
        {
            var slot = new Slot();
            if (!Core.Me.HasAura(SMNData.Buffs.Swiftcast))
            {
                if (SpellsDefine.Swiftcast.GetSpell().IsReadyWithCanCast())
                    slot.Add(SpellsDefine.Swiftcast.GetSpell());
            }
            var target = SMNHelper.GetDeadChara();
            //ChatHelper.SendMessage($"/e 复活hotkey拉队友，功德+2，复活了《{target?.Name}》");
            slot.Add(new Spell(SMNData.Spells.Resurrection, target));
            
            AI.Instance.BattleData.NextSlot = slot;
            SummonerRotationEntry.SMNHintManager.TriggerHint("复活", customContent: $"复活: {target?.Name}");
            //ChatHelper.SendMessage($"/e 成功复活");
            //AI.Instance.BattleData.HighPrioritySlots_OffGCD
            //AI.Instance.BattleData.HighPrioritySlots_GCD.Enqueue(slot);
        }
    }
}
