using CombatRoutine.View;
using Common.GUI;
using Common.Language;
using ImGuiNET;

namespace LittleNightmare.Summoner
{
    public class SMNSettingView : ISettingUI
    {
        public string Name => "召唤";

        public void Draw()
        {
            ImGuiHelper.ToggleButton("自动火神冲".Loc(), ref SMNSettings.Instance.qt自动火神冲);
            ImGuiHelper.ToggleButton("自动爆发药".Loc(), ref SMNSettings.Instance.qt自动爆发药);
            ImGuiHelper.ToggleButton("阻止双插溃烂爆发".Loc(), ref SMNSettings.Instance.PreventDoubleFester);
            
            ImGui.Text("醒梦阈值: ".Loc());
            ImGui.SameLine();
            ImGui.SetNextItemWidth(100);
            if (ImGui.InputInt("##MPThreshold", ref SMNSettings.Instance.MPThreshold, 100, 1000))
            {
                SMNSettings.Instance.MPThreshold = Math.Clamp(SMNSettings.Instance.MPThreshold, 0, 10000);
            }

            ImGuiHelper.ToggleButton("智能AOE目标".Loc(), ref SMNSettings.Instance.SmartAoETarget);
            ImGuiHelper.SetHoverTooltip("将智能选择最适合释放AoE的目标，而不是根据当前目标决定是否使用AoE\n火神冲的支持待定".Loc());

            
            if (ImGui.Button("保存设置"))
            {
                SMNSettings.Instance.Save();
                Qt.NewDefault("自动火神冲".Loc(), SMNSettings.Instance.qt自动火神冲);
                Qt.NewDefault("爆发药".Loc(), SMNSettings.Instance.qt自动爆发药);
                Qt.Reset();
            }
            ImGui.Text("如何迁移逆光时间轴到小小梦魇:");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("1. 直接打开json文件");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("2. 搜索`NiGuangOwO`全部替换成`LittleNightmare`");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("2.1 搜索`SMNTriggerActionSetQt`全部替换成`SMNTriggerActionQt`");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("2.2 `SMNTriggersActionBahaPhoenixCheck`上一个技能是巴哈或凤凰 这个暂时不支持，麻烦先删掉，再导入");
            ImGui.Text("3. 保存");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("注：反向同理，本ACR会尽力兼容逆光的（在拿到源码时）");
            ImGui.Spacing();
            ImGui.Text("Qt选项介绍:");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("对于时间轴编者:请尽力通过专门的Qt面板来管理相关动作");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("爆发: 关闭时不会自动召唤亚灵神，使用灼热之光");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("爆发药: 关闭时不会开场吃爆发药");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("AOE: 关闭时不会启用AOE技能，但带AOE效果实际单体也用的技能正常使用");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("最终爆发: 启用时会尝试卸掉所有豆子和亚灵神的攻击类能力技," +
                       "但请注意，这不会打开爆发qt");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("灼热之光: 关闭时不会自动灼热之光，不开启爆发qt也不会用");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("三神召唤: 关闭时不会自动召唤三神");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("巴哈凤凰: 关闭时不会自动召唤亚灵神，不开启爆发qt也不会用");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("宝石耀: 关闭时不会自动使用三神的GCD，不开启爆发qt也不会用");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("自动火神冲: 火神冲会遵循火神模式设置，关闭时只有在目标圈上会用火神冲。开启后，会在不移动时使用火神冲");
        }
    }
}