using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.View.JobView;
using System.Numerics;
using AEAssist;
using AEAssist.MemoryApi;
using AEAssist.Helper;
using Dalamud.Interface.Textures.TextureWraps;
using ImGuiNET;

namespace LittleNightmare
{
    public class HotKeyResolver_NormalSpellCustom : IHotkeyResolver
    {
        private readonly HotKeyResolver_NormalSpell _resolver;
        public delegate bool CustomConditionDelegate();
        private readonly CustomConditionDelegate _customCondition;

        public HotKeyResolver_NormalSpellCustom(HotKeyResolver_NormalSpell resolver, CustomConditionDelegate condition)
        {
            _resolver = resolver;
            _customCondition = condition;
        }

        // 使用表达式主体定义来简化属性
        public uint SpellId => _resolver.SpellId;
        public SpellTargetType TargetType => _resolver.TargetType;
        public bool UseHighPri => _resolver.UseHighPri;

        // 使用表达式主体定义来简化方法
        public void Draw(Vector2 size) => _resolver.Draw(size);
        public void DrawExternal(Vector2 size, bool isActive)
        {
            SpellHelper.DrawSpellInfo(SpellId.GetSpell(), size, _customCondition(), Action);
        }

        private void Action(Spell spell, Vector2 hotKeySize, bool isActive)
        {
            if (isActive)
            {
                ImGui.SetCursorPos(new Vector2(0.0f, 0.0f));
                IDalamudTextureWrap textureWrap;
                if (Core.Resolve<MemApiIcon>().TryGetTexture("Resources\\Spells\\Icon\\iconframe.png", out textureWrap))
                    ImGui.Image(textureWrap.ImGuiHandle, hotKeySize);
            }
            else
            {
                ImGui.SetCursorPos(new Vector2(0.0f, 0.0f));
                IDalamudTextureWrap textureWrap;
                if (Core.Resolve<MemApiIcon>().TryGetTexture("Resources\\Spells\\Icon\\icona_frame_disabled.png", out textureWrap))
                    ImGui.Image(textureWrap.ImGuiHandle, hotKeySize);
            }

        }

        public void Run() => _resolver.Run();

        public int Check() => _resolver.Check();
    }
}
