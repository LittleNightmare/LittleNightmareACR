using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.MemoryApi;
using Dalamud.Interface.Textures.TextureWraps;
using ImGuiNET;
using System.Numerics;


namespace LittleNightmare
{
    public class HotKey_HighPrioritySlotsClear : IHotkeyResolver
    {
        public int Check() => 0;

        public void Draw(Vector2 size)
        {
            Vector2 size1 = size * 0.8f;
            ImGui.SetCursorPos(size * 0.1f);
            IDalamudTextureWrap textureWrap;
            if (!Core.Resolve<MemApiIcon>().TryGetTexture("Resources\\Spells\\Monk\\清理马桶.png", out textureWrap))
                return;
            ImGui.Image(textureWrap.ImGuiHandle, size1);
        }

        public void DrawExternal(Vector2 size, bool isActive)
        {
        }

        public void Run()
        {
            // 清理高优先级GCD技能队列
            if (AI.Instance.BattleData.HighPrioritySlots_GCD is { Count: > 0 })
                AI.Instance.BattleData.HighPrioritySlots_GCD.Clear();

            // 清理高优先级非GCD技能队列
            if (AI.Instance.BattleData.HighPrioritySlots_OffGCD is { Count: > 0 })
                AI.Instance.BattleData.HighPrioritySlots_OffGCD.Clear();
        }
    }
}