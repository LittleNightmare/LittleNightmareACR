using AEAssist.GUI;
using ImGuiNET;

namespace LittleNightmare.Summoner
{
    public class SMNSettingView
    {
        public static SMNSettingView Instance = new();

        //public SMNSettings SMNSettings => SMNSettings.Instance;

        public void Draw()
        {
            ImGuiHelper.ToggleButton("自动火神冲", ref SMNSettings.Instance.qt自动火神冲);
            ImGuiHelper.ToggleButton("自动爆发药", ref SMNSettings.Instance.qt自动爆发药);
            ImGuiHelper.ToggleButton("阻止双插溃烂爆发", ref SMNSettings.Instance.PreventDoubleFester);

            ImGui.Text("醒梦阈值: ");
            ImGui.SameLine();
            ImGui.SetNextItemWidth(150);
            if (ImGui.InputInt("##MPThreshold", ref SMNSettings.Instance.MPThreshold, 100, 1000))
            {
                SMNSettings.Instance.MPThreshold = Math.Clamp(SMNSettings.Instance.MPThreshold, 0, 10000);
            }

            ImGuiHelper.ToggleButton("智能AOE目标", ref SMNSettings.Instance.SmartAoETarget);
            ImGuiHelper.SetHoverTooltip("将智能选择最适合释放AoE的目标，而不是根据当前目标决定是否使用AoE\n火神冲的支持待定");

            ImGuiHelper.ToggleButton("濒死检测", ref SMNSettings.Instance.TTKControl);
            ImGuiHelper.SetHoverTooltip("濒死检测会在你的目标濒死时，自动关闭爆发qt，以免浪费相关技能\n推荐日随使用，高难本请自行判断是否启用");

            ImGuiHelper.DrawEnum("起手选择: ", ref SMNSettings.Instance.SelectedOpener);
            ImGuiHelper.SetHoverTooltip("TheBalance: 是用TheBalance的通用起手\nTheBalance90: 是用TheBalance的90级起手");

            ImGuiHelper.DrawEnum("苏生之炎目标: ", ref SMNSettings.Instance.RekindleTarget);
            ImGuiHelper.SetHoverTooltip("苏生之炎的目标选择\n注意：最后三个 不 要 选");


            if (ImGui.Button("保存设置"))
            {
                SMNSettings.Instance.Save();
                SummonerRotationEntry.QT.NewDefault("自动火神冲", SMNSettings.Instance.qt自动火神冲);
                SummonerRotationEntry.QT.NewDefault("爆发药", SMNSettings.Instance.qt自动爆发药);
                SummonerRotationEntry.QT.Reset();
            }

            /*
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
            */
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

            if (ImGui.CollapsingHeader("更新日志##LittleNightmare"))
            {
                ImGui.Text("2024-10-22" +
                           "\n移动反馈链接到AE标准位置");
                if (ImGui.CollapsingHeader("历史更新日志##LittleNightmareHistory"))
                {
                    ImGui.Text("2024-10-19" +
                               "\n修复速卸三神下的火神冲会影响正常的问题x2" +
                               "\n添加濒死控制下，切换QT的提示");
                    ImGui.Text("2024-10-17" +
                               "\n修复速卸三神下的火神冲会影响正常的问题" +
                               "\n修复濒死检测的错误判断问题，现在应该更准确了");
                    ImGui.Text("2024-10-16" +
                               "\n修复即刻咏唱模式的问题导致无法正常释放能力技" +
                               "\n优化代码逻辑，避免类似的事情发生" +
                               "\n修复不拉人的问题");
                    ImGui.Text("2024-10-15" +
                               "\n更完善的濒死检测，现在应该更多的BOSS会被准确识别了" +
                               "\n在QT面板中，可以控制在最终爆发时，可以速卸三神" +
                               "\n在QT面板中，可以控制在速卸三神时，可以选择优先火神冲" +
                               "\n添加了一个时间轴行为，可以控制一些ACR设置");
                    ImGui.Text("2024-10-12" + "\nTTK的相关判断（是否为最后一个BOSS）采用AE内置的方法，更加稳定\n添加苏生之炎的目标选择\n添加火神冲1段在热键栏，如果还有想要加入的可以告诉我，适合那种写时间轴很麻烦，按钮控制又不太好的");
                    ImGui.Indent();
                    ImGui.Text("2024-10-10" + "\n修复副本内停止攻击的问题");
                    ImGui.Spacing();
                    ImGui.Text("2024-10-09" + "\n增加濒死检测（TTK）\n理论上修复召唤兽列表导致的崩溃\n增加一个更新日志");
                    ImGui.Unindent();
                }
            }
        }
    }
}