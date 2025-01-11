using AEAssist.GUI;
using ImGuiNET;

namespace LittleNightmare.Summoner
{
    public class SMNSettingView
    {
        public static SMNSettingView Instance = new();

        private readonly string[] _tableColum = ["名称", "内容", "聊天栏", "Toast2", "Toast2显示时间(ms)", "TTS", "提示音"];

        public void Draw()
        {
            ImGui.Spacing();
            if (ImGui.Button("保存设置"))
            {
                SMNSettings.Instance.Save();
                SummonerRotationEntry.QT.NewDefault("自动火神冲", SMNSettings.Instance.qt自动火神冲);
                SummonerRotationEntry.QT.NewDefault("爆发药", SMNSettings.Instance.qt自动爆发药);
                SummonerRotationEntry.QT.QTDefaultValue["爆发药"] = (SMNSettings.Instance.qt自动爆发药, "");
                SummonerRotationEntry.QT.QTDefaultValue["自动火神冲"] = (SMNSettings.Instance.qt自动火神冲, "");
                SummonerRotationEntry.QT.Reset();
            }
            ImGuiHelper.SetHoverTooltip("手动保存一下，切换时会自动保存");
            if (ImGui.CollapsingHeader("使用Tips"))
            {
                ImGui.Text("1. 配合Orbwalker进行滑步: 将AE的`提前使用下一个GCD时间`设置为50ms" +
                           "\n\t不排除会有其他问题，如延迟造成的影响，请及时反馈");
                ImGui.Text("2. GCD调整为2.48不然卡龙喷，建议配合Fuck进行动画锁调整，隔壁ACR推荐三插模式300ms，这与网络延迟有关" +
                           "\n\t不确定会不会导致logs红色，请及时反馈");
            }
            ImGui.Spacing();
            if (ImGui.CollapsingHeader("QT设置", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGuiHelper.ToggleButton("战斗结束后自动重置QT", ref SMNSettings.Instance.AutoResetQt);
                ImGuiHelper.ToggleButton("自动火神冲", ref SMNSettings.Instance.qt自动火神冲);
                ImGuiHelper.SetHoverTooltip("请参考Qt选项介绍中`自动火神冲`的介绍");
                ImGuiHelper.ToggleButton("自动爆发药", ref SMNSettings.Instance.qt自动爆发药);
                ImGuiHelper.SetHoverTooltip("请参考Qt选项介绍中`爆发药`的介绍");
            }
            ImGui.Spacing();

            if (ImGui.CollapsingHeader("循环设置", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGuiHelper.DrawEnum("起手选择: ", ref SMNSettings.Instance.SelectedOpener);
                ImGuiHelper.SetHoverTooltip("暂无自动切换，不满足条件不会启动，如果使用会有提示，具体参考`ACR自动提示控制`" +
                                            "\nTheBalance: 是用TheBalance的通用起手" +
                                            "\nTheBalance90: 是用TheBalance的90级起手" +
                                            "\nFastEnergyDrain: 是绝欧特化起手（慎用，尚未确定当前版本仍是这个）");

                ImGuiHelper.ToggleButton("阻止双插以太豆子技", ref SMNSettings.Instance.PreventDoubleFester);
                ImGuiHelper.SetHoverTooltip("阻止一个能力技窗口内，双插溃烂爆发或坏死爆发，还有他们的对应AOE技能");
                ImGuiHelper.ToggleButton("阻止亚灵神前召唤三神", ref SMNSettings.Instance.PreventSummonBeforeBahamut);
                ImGuiHelper.SetHoverTooltip("开启后召唤三神会影响亚灵神的释放时，不会召唤三神。主要用于防止循环因为召唤三神错位。" +
                                            "\n日随中开启应该没问题" +
                                            "\n高难时建议时间轴作者自行决定");

                ImGui.Text("醒梦阈值: ");
                ImGui.SameLine();
                ImGui.SetNextItemWidth(150);
                if (ImGui.InputInt("##MPThreshold", ref SMNSettings.Instance.MPThreshold, 100, 1000))
                {
                    SMNSettings.Instance.MPThreshold = Math.Clamp(SMNSettings.Instance.MPThreshold, 0, 10000);
                }
            }

            ImGui.Spacing();
            if (ImGui.CollapsingHeader("智能设置", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGuiHelper.SetHoverTooltip("智能设置会造成性能损耗，如果发现严重影响请关闭");

                ImGuiHelper.ToggleButton("智能AOE目标", ref SMNSettings.Instance.SmartAoETarget);
                ImGuiHelper.SetHoverTooltip("将智能选择最适合释放AoE的目标，而不是根据当前目标决定是否使用AoE\n火神冲的暂无支持计划");

                ImGuiHelper.ToggleButton("濒死检测", ref SMNSettings.Instance.TTKControl);
                ImGuiHelper.SetHoverTooltip("濒死检测会在你的目标濒死时，自动关闭爆发qt，以免浪费相关技能\n推荐日随使用，高难本请自行判断是否启用");

                ImGuiHelper.ToggleButton("自动减伤", ref SMNSettings.Instance.AutoReduceDamage);
                ImGuiHelper.SetHoverTooltip("在非当前高难本中，自动开启减伤，目前只有昏乱");
                ImGui.BeginDisabled(!SMNSettings.Instance.AutoReduceDamage);
                ImGuiHelper.LeftInputInt("自动减伤阈值:", ref SMNSettings.Instance.CastReduceTimeBeforeSeconds);
                ImGui.EndDisabled();
                ImGuiHelper.SetHoverTooltip("设置提前多少秒使用减伤");

                ImGuiHelper.ToggleButton("自动停手", ref SMNSettings.Instance.AutoStopForSpecialBuff);
                ImGuiHelper.SetHoverTooltip("在特殊buff下，自动停止攻击" +
                                            "\n如`加速度炸弹`在2s前停止、`无敌`时停手" +
                                            "\n理论上高难能用？请时间轴作者自行判断");
                
                ImGuiHelper.DrawEnum("苏生之炎目标: ", ref SMNSettings.Instance.RekindleTarget);
                ImGuiHelper.SetHoverTooltip("苏生之炎的目标选择\n注意：最后四个 不 要 选");
            }

            ImGui.Spacing();

            if (ImGui.CollapsingHeader("ACR自动提示控制", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGuiHelper.SetHoverTooltip("ACR自动行为的提示" +
                                            "\n勾选表示需要使用，请注意" +
                                            "\nTTS使用DailyRoutine" +
                                            "\n提示音暂时没有，可以忽视" +
                                            "\n如果误操作，请不要点保存，直接切换ACR");
                if (ImGui.BeginTable("hints", _tableColum.Length,
                        ImGuiTableFlags.RowBg | ImGuiTableFlags.ScrollY | ImGuiTableFlags.Resizable | ImGuiTableFlags.SizingFixedFit))
                {
                    // 画一个Table，用来展示SummonerRotationEntry.SMNHintManager.Hints内容
                    foreach (var t in _tableColum)
                    {
                        ImGui.TableSetupColumn(t, ImGuiTableColumnFlags.None, t.Length);
                    }
                    ImGui.TableHeadersRow();
                    var index = 0;
                    foreach (var key in SummonerRotationEntry.SMNHintManager.Hints.Keys.ToList())
                    {
                        ImGui.TableNextRow();
                        var hint = SummonerRotationEntry.SMNHintManager.Hints[key];
                        ImGui.TableNextColumn();
                        ImGui.Text(key);
                        ImGui.TableNextColumn();
                        if (key is "减伤" or "Welcome" or "即刻" or "复活" or "引用检测")
                        {
                            ImGui.Text("ACR自动控制，暂不可调整");
                        }
                        else
                        {
                            ImGui.InputText($"##内容{index}", ref hint.Content, 200);
                        }
                        ImGui.TableNextColumn();
                        ImGui.Checkbox($"##聊天栏{index}", ref hint.ShowInChat);
                        ImGui.TableNextColumn();
                        ImGui.Checkbox($"##Toast2{index}", ref hint.ShowToast2);
                        ImGui.TableNextColumn();
                        ImGui.InputInt($"##Toast2显示时间{index}", ref hint.Toast2TimeInMs);
                        ImGui.TableNextColumn();
                        ImGui.Checkbox($"##TTS{index}", ref hint.UseTTS);
                        ImGui.TableNextColumn();
                        ImGui.Checkbox($"##提示音{index}", ref hint.PlaySound);
                        SummonerRotationEntry.SMNHintManager.Hints[key] = hint; // 直接更新 Hints 中的值
                        index++;
                    }

                    ImGui.EndTable();
                }

                ImGui.Spacing();
                ImGui.BeginDisabled(!ImGui.GetIO().KeyShift);
                if (ImGui.Button("重置ACR自动提示控制##hints"))
                {
                    SummonerRotationEntry.BuildHints(true);
                }

                ImGui.EndDisabled();

                ImGuiHelper.SetHoverTooltip("重置ACR自动提示到默认状态" +
                                            "\n按住Shift，才能重置");
            }

            ImGui.Spacing();

            if (ImGui.CollapsingHeader("常见ACR行为：是Feature不是BUG"))
            {
                ImGui.Text("1. 豆子不打：目前设定是：即使卡能量吸收也会等到开启灼热之光再打，除非打开`最终爆发`。会考虑增加一个选项控制这个行为");
                ImGui.Text("2. 在目标圈里不放火神冲：在目标圈内移动中释放，需要开启面板的`目标圈内移动时使用火神冲`");
                ImGui.Text("3. 在目标圈外不放火神冲：需求启动`自动火神冲`，并且没有在移动");
                ImGui.Text("4. 不打三神：待修复，在确认开启了`爆发`和`三神召唤`后，这个原因是开启了`阻止亚灵神前召唤三神`，目前还没找到比较好的解决方案。建议先手动打一下，或者关闭这个功能");
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
            if (ImGui.CollapsingHeader("Qt选项介绍"))
            {
                ImGui.Text("对于时间轴编者:请尽力通过专门的Qt面板来管理相关动作");
                ImGui.SetNextItemWidth(200);
                ImGui.Text("爆发: 关闭时不会自动召唤亚灵神，使用灼热之光");
                ImGui.SetNextItemWidth(200);
                ImGui.Text("爆发药: 关闭时不会开场吃爆发药，目前没写战斗中吃爆发药，如果有需求可以反馈");
                ImGui.SetNextItemWidth(200);
                ImGui.Text("AOE: 关闭时不会启用AOE技能，但带AOE效果实际单体也用的技能正常使用");
                ImGui.SetNextItemWidth(200);
                ImGui.Text("最终爆发: 启用时会尝试卸掉所有豆子和亚灵神的攻击类能力技," +
                           "\n但请注意，这 不 会 打开爆发qt");
                ImGui.SetNextItemWidth(200);
                ImGui.Text("灼热之光: 关闭时不会自动灼热之光，不开启爆发qt也不会用");
                ImGui.SetNextItemWidth(200);
                ImGui.Text("三神召唤: 关闭时不会自动召唤三神");
                ImGui.SetNextItemWidth(200);
                ImGui.Text("巴哈凤凰: 关闭时不会自动召唤亚灵神，不开启爆发qt也不会用");
                ImGui.SetNextItemWidth(200);
                ImGui.Text("宝石耀: 关闭时不会自动使用三神的GCD，不开启爆发qt也不会用");
                ImGui.SetNextItemWidth(200);
                ImGui.Text("自动火神冲: 火神冲会遵循火神模式设置，关闭时只有在目标圈上会用火神冲。开启后，保留之前的不变，会在目标圈外不移动时使用火神冲");
            }
            ImGui.Spacing();
            if (ImGui.CollapsingHeader("更新日志##LittleNightmare"))
            {
                ImGui.Text("2025-01-12" +
                           "\n添加功能`自动停手`，现在依赖JiaXX的ACR，必须下载，不然我不保证这玩意不炸" +
                           "\n添加`减伤阈值`");
                ImGui.Indent();
                if (ImGui.CollapsingHeader("历史更新日志##LittleNightmareHistory"))
                {
                    ImGui.Text("2025-01-11" +
                               "\n修改部分描述：`起手选择`,`常见ACR行为`" +
                               "\n添加了一个`使用Tips`" +
                               "\n优化ACR设置UI，如果发现选项有丢失请及时联系我" +
                               "\n优化ACR代码，使用不应发生变化，除非有其他ACR用户引用我的SummonerRotationEntry");
                    ImGui.Text("2025-01-07" +
                               "\n修复API变化引起的ACR行为不正常的问题，如一直在使用宝石兽召唤");
                    ImGui.Text("2024-12-29" +
                               "\n新增了爆发药QT检测，用于时间轴：判断是否开启了爆发药" +
                               "\n修复最终爆发（倾泻资源）没有提示的问题" +
                               "\n添加复活、即刻提示，优化提示描述");
                    ImGui.Text("2024-12-28" +
                               "\n修复新用户的报错问题" +
                               "\n同步AE，现在需要战斗后重置QT移动到了设置界面");
                    ImGui.Text("2024-12-27" +
                               "\n1. 增加新功能`ACR自动提示控制`用户可以控制ACR部分自动操作的提示了（如果还有哪里需要，可以告诉我加或直接PR）" +
                               "\n提示音暂时没有" +
                               "\n2. 恢复绝欧特化起手，即提前能量吸收，不确定现在还是不是这么打" +
                               "\n3. 去掉冗余部分，正常无体感影响");
                    ImGui.Text("2024-12-05" +
                               "\n在默认状态下，恢复以前的逻辑" +
                               "\n新增选择`阻止亚灵神前召唤三神`，开启后召唤三神会影响亚灵神的释放时，不会召唤三神");
                    ImGui.Text("2024-12-02" +
                               "\n添加自动减伤，目前只有昏乱，只能用在非当前高难" +
                               "\n添加TTK控制到QT面板，方便控制");
                    ImGui.Text("2024-12-01" +
                               "\n修复`修改ACR简单设置`在时间轴里不保存的问题");
                    ImGui.Text("2024-11-15" +
                               "\n放宽起手判断到5s，可以更好的兼容一些抢开情况" +
                               "\n起手添加聊天栏提示");
                    ImGui.Text("2024-11-03" +
                               "\n三神现在不会在巴哈凤凰CD快结束时召唤" +
                               "\n最终爆发速卸三神添加可选 无视龙神CD");
                    ImGui.Text("2024-11-02" +
                               "\n修复火神冲hotkey需要激情连点的问题，现在火神冲会在亮起时加入队列" +
                               "\n如果需要想要提前选定火神施法行为，可以使用位于qt面板的`火神施法模式`" +
                               "\n将`自动火神冲`解释的更清晰点");
                    ImGui.Text("2024-11-01" +
                               "\n更准确的判断最后一个boss，现在24人本的濒死判断（TTK）应该会正常工作" +
                               "\n更新api，解决高优先级能力技的问题" +
                               "\n现在火神冲Hotkey不会在没好的时候进入优先级队列" +
                               "\n现在aoe类型的豆子技能也会进入阻止双插的检测了" +
                               "\n优化文本，更精准的描述");
                    ImGui.Text("2024-10-22" +
                               "\n移动反馈链接到AE标准位置");
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

                    ImGui.Text("2024-10-10" + "\n修复副本内停止攻击的问题");
                    ImGui.Text("2024-10-09" + "\n增加濒死检测（TTK）\n理论上修复召唤兽列表导致的崩溃\n增加一个更新日志");
                }
                ImGui.Unindent();
            }
        }
    }
}