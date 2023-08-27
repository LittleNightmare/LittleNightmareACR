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
            if (ImGui.Button("保存设置"))
            {
                SMNSettings.Instance.Save();
            }
            ImGui.Text("如何迁移逆光时间轴到小小梦魇:");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("1. 直接打开json文件");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("2. 搜索`NiGuangOwO`全部替换成`LittleNightmare`");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("3. 保存");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("注：反向同理，本ACR会尽力兼容逆光的（在拿到源码时）");
            ImGui.Spacing();
            ImGui.Text("Qt选项介绍:");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("对于时间轴编者:请通过单独的Qt触发器控制Qt的行为,\n通用的许多与Qt的同名选项目前无法起到作用");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("爆发: 关闭时不会自动召唤亚灵神，使用灼热之光");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("爆发药: 关闭时不会开场吃爆发药");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("AOE: 关闭时不会启用AOE技能，\n但带AOE效果实际单体也用的技能正常使用");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("最终爆发: 启用时会尝试卸掉所有豆子和亚灵神的攻击类能力技,\n" +
                       "但不会打开爆发qt");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("灼热之光: 关闭时不会自动灼热之光，不开启爆发qt也不会用");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("三神召唤: 关闭时不会自动召唤三神");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("巴哈凤凰: 关闭时不会自动召唤亚灵神，不开启爆发qt也不会用");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("宝石耀: 关闭时不会自动使用三神的GCD，不开启爆发qt也不会用");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("自动火神冲: 关闭时只有在目标距离目标小于3m才会用火神冲，如果开启将会直接使用");
            ImGui.SetNextItemWidth(200);
            ImGui.Text("预读风神即刻咏唱: **实验性**尝试提前预读即刻使用螺旋气流，螺旋气流锁定时，停止工作");
        }
    }
}