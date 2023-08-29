using CombatRoutine.Setting;
using CombatRoutine;
using CombatRoutine.View;
using ImGuiNET;
using System.Numerics;
using Common.Language;
using Common;
using Common.Define;
using Common.GUI;
using Common.Helper;

namespace LittleNightmare.Summoner
{
    public class SummonerOverlay
    {
        public string? SwiftcastMode;

        internal void Draw()
        {
            Style.SetMainStyle();
            Style.MainControlView(ref Core.CombatRun, ref PlayerOptions.Instance.Stop);
            ImGui.Dummy(new Vector2(0, 5));
            if (ImGui.BeginTabBar("###tab"))
            {
                if (ImGui.BeginTabItem("通常"))
                {
                    ImGui.BeginChild("##tab1", new Vector2(0, 0));

                    ImGui.Text("(自动)当前三神顺序:".Loc());
                    foreach (var spell in SMNBattleData.Instance.Summon)
                    {
                        ImGui.SameLine();
                        ImGui.Text(spell.Name);
                    }

                    ImGui.Text("(队列)当前三神顺序:");
                    foreach (var spell in SMNBattleData.Instance.CustomSummon)
                    {
                        ImGui.SameLine();
                        ImGui.Text(spell.Name);
                    }

                    ImGui.Text("土神次数".Loc()+ SMNBattleData.Instance.TitanGemshineTimes.ToString());
                    ImGui.SameLine();
                    ImGui.Text("火神次数".Loc()+ SMNBattleData.Instance.IfritGemshineTimes.ToString());
                    ImGui.SameLine();
                    ImGui.Text("风神次数".Loc()+ SMNBattleData.Instance.GarudaGemshineTimes.ToString());

                    if (ImGui.Button("土神优先".Loc(), new Vector2(100, 30)))
                    {
                        SMNBattleData.Instance.TitanFirst();
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("火神优先".Loc(), new Vector2(100, 30)))
                    {
                        SMNBattleData.Instance.IfritFirst();
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("风神优先".Loc(), new Vector2(100, 30)))
                    {
                        SMNBattleData.Instance.GarudaFirst();
                    }
                    ImGui.Text("开场爆发中: ".Loc() + SMNBattleData.Instance.In90Opener);
                    SwiftcastMode = SMNSettings.Instance.即刻咏唱模式 switch
                    {
                        0 => "即刻复活".Loc(),
                        1 => "风神读条".Loc(),
                        2 => "火神读条".Loc(),
                        3 => "全部".Loc(),
                        _ => SwiftcastMode
                    };

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.BeginCombo("即刻咏唱模式".Loc(), SwiftcastMode))
                    {
                        if (ImGui.Selectable("即刻复活".Loc()))
                        {
                            SMNSettings.Instance.即刻咏唱模式 = 0;
                            SMNSettings.Instance.Save();
                        }
                        if (ImGui.Selectable("风神读条".Loc()))
                        {
                            SMNSettings.Instance.即刻咏唱模式 = 1;
                            SMNSettings.Instance.Save();
                        }
                        if (ImGui.Selectable("火神读条".Loc()))
                        {
                            SMNSettings.Instance.即刻咏唱模式 = 2;
                            SMNSettings.Instance.Save();
                        }
                        if (ImGui.Selectable("全部".Loc()))
                        {
                            SMNSettings.Instance.即刻咏唱模式 = 3;
                            SMNSettings.Instance.Save();
                        }
                        ImGui.EndCombo();
                    }

                    ImGui.Text("即刻技能GCD数量".Loc() + SMNBattleData.Instance.GCDLeftUntilNextSwiftCasted());
                    ImGui.SameLine();
                    ImGui.Text("可以释放".Loc() + SMNBattleData.Instance.CastSwiftCastCouldCoverTargetSpell());
                    if (ImGui.CollapsingHeader("SMN起手设置"))
                    {
                        ImGui.SetNextItemWidth(200);
                        if (ImGui.Checkbox("开场灼热之光优先".Loc(), ref SMNSettings.Instance.SearingLightFirst))
                        {
                            SMNSettings.Instance.Save();
                        }
                        ImGui.SetNextItemWidth(200);
                        if (ImGui.Checkbox("开场龙神第二个GCD用能量吸收".Loc(), ref SMNSettings.Instance.FastEnergyDrain))
                        {
                            SMNSettings.Instance.Save();
                        }
                    }

                    if (ImGui.Checkbox("目标圈内移动时使用火神冲".Loc(), ref SMNSettings.Instance.SlideUseCrimonCyclone))
                    {
                        SMNSettings.Instance.Save();
                    }
                    ImGuiHelper.SetHoverTooltip("移动时，如果在目标圈上，使用火神冲\n不然尝试其他的技能，比如毁4".Loc());
                    ImGui.TextDisabled("Qt的描述可以看ACR的设置界面".Loc());

                    ImGui.EndChild();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("时间轴信息"))
                {
                    ImGui.BeginChild("##tab2", new Vector2(0, 0));
                    OverlayHelper.DrawTriggerlineInfo();
                    ImGui.EndChild();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Qt"))
                {
                    ImGui.BeginChild("##tab3", new Vector2(0, 0));

                    ImGui.Checkbox("战斗结束qt自动重置回战斗前状态".Loc(), ref SMNSettings.Instance.AutoReset);
                    Qt.QtWindow.QtSettingView();

                    ImGui.EndChild();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Qt默认开启设置"))
                {
                    ImGui.BeginChild("##tab4", new Vector2(0, 0));
                    if (ImGui.Checkbox("自动火神冲".Loc(), ref SMNSettings.Instance.qt自动火神冲))
                    {
                        Qt.NewDefault("自动火神冲".Loc(), SMNSettings.Instance.qt自动火神冲);
                        SMNSettings.Instance.Save();
                    }

                    ImGui.EndChild();
                    ImGui.EndTabItem();
                }
                

                if (ImGui.BeginTabItem("风格"))
                {
                    ImGui.BeginChild("##tab5", new Vector2(0, 0));
                    //风格设置
                    ImGui.Text("编辑风格");
                    Style.ChangeStyleView();

                    ImGui.EndChild();
                    ImGui.EndTabItem();
                }
#if DEBUG
                if (ImGui.BeginTabItem("Debug"))
                {
                    ImGui.BeginChild("##tab6", new Vector2(0, 0));
                    ImGui.Text($"Attunement层数：{Core.Get<IMemApiSummoner>().ElementalAttunement}");
                    var type = "";
                    switch (Core.Get<IMemApiSummoner>().ActivePetType)
                    {
                        case ActivePetType.None:
                            type = "无";
                            break;
                        case ActivePetType.Titan:
                            type = "土神";
                            break;
                        case ActivePetType.Garuda:
                            type = "风神";
                            break;
                        case ActivePetType.Ifrit:
                            type = "火神";
                            break;
                        case ActivePetType.Bahamut:
                            type = "龙神";
                            break;
                        case ActivePetType.Phoneix:
                            type = "凤凰";
                            break;
                    }
                    ImGui.Text($"召唤兽类别：{type}");
                    ImGui.Text($"召唤兽时间：{Core.Get<IMemApiSummoner>().PetTimer}");
                    ImGui.Text($"山崩预备: {Core.Me.HasMyAura(AurasDefine.TitansFavor)}");
                    ImGui.Text($"能力技次数: {AI.Instance.BattleData.AbilityCount}");
                    ImGui.Text($"宝石耀属性: {Core.Get<IMemApiSpell>().GetSpellType(SpellsDefine.Gemshine.GetSpell().Id)}");
                    ImGui.Text($"距离Melee: {Core.Me.DistanceMelee(Core.Me.GetCurrTarget())}");
                    ImGui.Text($"距离: {Core.Me.Distance(Core.Me.GetCurrTarget())}");
                    ImGui.EndChild();
                    ImGui.EndTabItem();
                }
#endif
                ImGui.EndTabBar();
            }
            //QT窗口
            Qt.Draw();
            Style.EndMainStyle();
        }

    }

    
    public static class Qt
    {
        public static Style.QtWindowClass QtWindow;

        static Qt()
        {
            OverlayHelper.DrawCommon();
            QtWindow = new Style.QtWindowClass();
            QtWindow.AddQt("爆发".Loc(), true);
            QtWindow.AddQt("爆发药".Loc(), true );
            QtWindow.AddQt("AOE".Loc(), true);
            QtWindow.AddQt("最终爆发".Loc(), false);
            QtWindow.AddQt("灼热之光".Loc(), true);
            QtWindow.AddQt("三神召唤".Loc(), true);
            QtWindow.AddQt("巴哈凤凰".Loc(), true);
            QtWindow.AddQt("宝石耀".Loc(), true);
            QtWindow.AddQt("自动火神冲".Loc(), SMNSettings.Instance.qt自动火神冲);
            // 毛病太多算了
            // QtWindow.AddQt("预读风神即刻咏唱".Loc(), false);
        }

        /// 获取指定名称qt的bool值
        public static bool GetQt(string qtName)
        {
            return QtWindow.GetQt(qtName);
        }

        /// 反转指定qt的值
        /// <returns>成功返回true，否则返回false</returns>
        public static bool ReverseQt(string qtName)
        {
            return QtWindow.ReverseQt(qtName);
        }

        /// 设置指定qt的值
        /// <returns>成功返回true，否则返回false</returns>
        public static bool SetQt(string qtName, bool qtValue)
        {
            return QtWindow.SetQt(qtName, qtValue);
        }

        /// 重置所有qt为默认值
        public static void Reset()
        {
            QtWindow.Reset();
        }

        /// 给指定qt设置新的默认值
        public static void NewDefault(string qtName, bool newDefault)
        {
            QtWindow.NewDefault(qtName, newDefault);
        }

        /// 将当前所有Qt状态记录为新的默认值，
        /// 通常用于战斗重置后qt还原到倒计时时间点的状态
        public static void SetDefaultFromNow()
        {
            QtWindow.SetDefaultFromNow();
        }

        /// 返回包含当前所有qt名字的数组
        public static string[] GetQtArray()
        {
            return QtWindow.GetQtArray();
        }

        public static void Draw()
        {
            QtWindow.DrawQtWindow();
        }
    }
}