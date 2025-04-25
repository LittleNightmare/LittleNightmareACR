using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using AEAssist.Extension;
using AEAssist.GUI;
using AEAssist.JobApi;
using ImGuiNET;
using System.Numerics;
using Dalamud.Utility;
using AEAssist.CombatRoutine.Module;
using LittleNightmare.Summoner.HotkeySlot;
using AEAssist.Helper;



#if DEBUG
using AEAssist.MemoryApi;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using FFXIVClientStructs.STD;
#endif

namespace LittleNightmare.Summoner
{
    public class SummonerOverlay : JobViewWindow
    {
        private static readonly string[] swiftcastModes = { "即刻复活", "风神读条", "火神读条", "全部", "停用" };

        private static readonly string[] IfritModes = { "先冲锋再读条", "先读条再冲锋", "读条-冲锋-读条" };

        // public string? SwiftcastMode;
        public Dictionary<string, string> OpenerDictionary =
            Enum.GetNames<SMNSettings.OpenerType>().ToDictionary(t => t, t => t);

        public Dictionary<string, (bool, string)> QTDefaultValue = new()
        {
            {"爆发", (true, "关闭时不会使用巴哈凤凰和灼热之光")},
            {"爆发药", (SMNSettings.Instance.qt自动爆发药, "")},
            {"AOE", (true, "")},
            {"最终爆发", (false, "倾泻资源")},
            {"灼热之光", (true, "")},
            {"三神召唤", (true, "")},
            {"巴哈凤凰", (true, "")},
            {"宝石耀", (true, "")},
            {"自动火神冲", (SMNSettings.Instance.qt自动火神冲, "")}
        };


        public SummonerOverlay(JobViewSave jobViewSave, Action save, string name) : base(jobViewSave, save, name)
        {
            AddTab("通常", DrawQtGeneral);
            // AddTab("时间轴信息", DrawTriggerlineInfo);
            // AddTab("Qt", DrawQtSettingView);
            // AddTab("风格", DrawChangeStyleView);
            AddTab("调试", DrawDebugView);
            // init QT value
            foreach (var (key, (value, tips)) in QTDefaultValue)
            {
                if (tips.IsNullOrEmpty())
                {
                    AddQt(key, value);
                }
                else
                {
                    AddQt(key, value, tips);
                }
            }

            AddHotkey("LB", new HotKeyResolver_LB());
            AddHotkey("沉稳咏唱", new HotKeyResolver_NormalSpell(SpellsDefine.Surecast, SpellTargetType.Self));
            AddHotkey("昏乱", new HotKeyResolver_NormalSpell(SpellsDefine.Addle, SpellTargetType.Target));
            AddHotkey("疾跑", new HotKeyResolver_疾跑());

            var crimson = new HotKeyResolver_NormalSpell(SMNData.Spells.CrimsonCyclone, SpellTargetType.Target, true);
            bool CustomCondition() => Core.Me.HasAura(SMNData.Buffs.IfritsFavor);
            AddHotkey("深红旋风", new HotKeyResolver_NormalSpellCustom(crimson, CustomCondition));

            AddHotkey("守护之光", new HotKeyResolver_NormalSpell(SMNData.Spells.RadiantAegis, SpellTargetType.Self));
            AddHotkey("日光普照", new HotKeyResolver_NormalSpell(SMNData.Spells.LuxSolaris, SpellTargetType.Self));
            AddHotkey("清理高优先级队列", new HotKey_HighPrioritySlotsClear());
            AddHotkey("即刻拉人，优先当前目标，没有即刻会读条", new SMNHotkey_Resurrection());
        }

        public new void Reset()
        {
            foreach (var (key, (value, tips)) in QTDefaultValue)
            {
                SetQt(key, value);
            }
        }

        public static void DrawQtGeneral(JobViewWindow jobViewWindow)
        {
            ImGui.Text("(自动)当前三神顺序:");
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

            ImGui.Text("土神次数" + SMNBattleData.Instance.TitanGemshineTimes.ToString());
            ImGui.SameLine();
            ImGui.Text("火神次数" + SMNBattleData.Instance.IfritGemshineTimes.ToString());
            ImGui.SameLine();
            ImGui.Text("风神次数" + SMNBattleData.Instance.GarudaGemshineTimes.ToString());

            if (ImGui.Button("土神优先", new Vector2(100, 30)))
            {
                SMNBattleData.Instance.TitanFirst();
            }

            ImGui.SameLine();
            if (ImGui.Button("火神优先", new Vector2(100, 30)))
            {
                SMNBattleData.Instance.IfritFirst();
            }

            ImGui.SameLine();
            if (ImGui.Button("风神优先", new Vector2(100, 30)))
            {
                SMNBattleData.Instance.GarudaFirst();
            }
            // ImGui.Text("开场爆发中: " + SMNBattleData.Instance.In90Opener);

            ImGui.SetNextItemWidth(200f);
            if (ImGui.BeginCombo("即刻咏唱模式", swiftcastModes[SMNSettings.Instance.SwiftCastMode]))
            {
                for (var i = 0; i < swiftcastModes.Length; i++)
                {
                    if (ImGui.Selectable(swiftcastModes[i]))
                    {
                        SMNSettings.Instance.SwiftCastMode = i;
                    }
                }

                ImGui.EndCombo();
            }

            //ImGuiHelper.SetHoverTooltip("起手会强制启用即刻风神");
            ImGui.SetNextItemWidth(200f);
            if (ImGui.BeginCombo("火神施法模式", IfritModes[SMNSettings.Instance.IfritMode]))
            {
                for (var j = 0; j < IfritModes.Length; j++)
                {
                    if (ImGui.Selectable(IfritModes[j]))
                    {
                        SMNSettings.Instance.IfritMode = j;
                    }
                }

                ImGui.EndCombo();
            }

            // ImGui.Text("即刻技能GCD数量" + SMNBattleData.Instance.GCDLeftUntilNextSwiftCasted());
            // ImGui.SameLine();
            // ImGui.Text("可以释放" + SMNBattleData.Instance.CastSwiftCastCouldCoverTargetSpell());


            // ImGuiHelper.DrawEnum("召唤起手", ref SMNSettings.Instance.SelectedOpener);

            ImGui.Checkbox("濒死检查", ref SMNSettings.Instance.TTKControl);
            ImGuiHelper.SetHoverTooltip("濒死检查会在你的目标濒死时，自动关闭爆发qt，以免浪费相关技能\n推荐日随使用，高难本请自行判断是否启用");

            ImGui.Checkbox("自动减伤", ref SMNSettings.Instance.AutoReduceDamage);
            ImGuiHelper.SetHoverTooltip("自动开启减伤，目前只有昏乱");

            ImGui.Checkbox("目标圈内移动时使用火神冲", ref SMNSettings.Instance.SlideUseCrimonCyclone);
            ImGuiHelper.SetHoverTooltip("移动时，如果在目标圈上，使用火神冲\n不然尝试其他的技能，比如毁4");
            ImGui.Checkbox("优先毁三填充最后GCD窗口", ref SMNSettings.Instance.UseRuinIIIFirst);
            ImGuiHelper.SetHoverTooltip("在GCD填充时，如果不移动，能量吸收还没马上好，优先毁3填充，再是毁4");

            ImGui.Checkbox("阻止亚灵神前召唤三神", ref SMNSettings.Instance.PreventSummonBeforeBahamut);
            ImGuiHelper.SetHoverTooltip("开启后召唤三神会影响亚灵神的释放时，不会召唤三神。主要用于防止循环因为召唤三神错位。" +
                                        "\n但关闭爆发QT或者关闭巴哈凤凰QT时，ACR会正常召唤三神" +
                                        "\n日随中开启应该没问题" +
                                        "\n高难时建议时间轴作者自行决定");

            ImGui.Checkbox("最终爆发时速卸三神", ref SMNSettings.Instance.FastPassSummon);
            ImGuiHelper.SetHoverTooltip("在最终爆发时，除了描述的行为外，同时速卸三神\n就是尽可能优先召唤三神，即使宝石技能没打完");
            if (SMNSettings.Instance.FastPassSummon)
            {
                // move next text object to a little right
                ImGui.Indent();
                ImGui.Checkbox("调整火神施法模式", ref SMNSettings.Instance.ModifyIfritMode);
                ImGuiHelper.SetHoverTooltip("强制将火神施法模式视为：先冲锋再读条");
                ImGui.BeginDisabled(!SMNSettings.Instance.PreventSummonBeforeBahamut);
                ImGui.Checkbox("无视龙神CD", ref SMNSettings.Instance.IngoreBahamutCDDuringFassPassSummon);
                ImGui.EndDisabled();
                ImGuiHelper.SetHoverTooltip("只是为了调整，开启`阻止亚灵神前召唤三神`时最终爆发的逻辑" +
                                            "\n在速卸三神召唤时，忽略巴哈的CD" +
                                            "\n即，可能会导致延后巴哈凤凰这些亚灵神");
                ImGui.Unindent();
            }

            // ImGuiHelper.ToggleButton("最终BOSS", ref SMNBattleData.Instance.FinalBoss);
            // ImGui.Checkbox("优先火神GCD", ref SMNSettings.Instance.RubyGCDFirst);
            // ImGuiHelper.SetHoverTooltip("在不移动时，优先使用火神GCD，而不是火神冲");
            ImGui.TextDisabled("Qt的描述可以看ACR的设置界面");
        }

        public void DrawDebugView(JobViewWindow window)
        {
            var target = Core.Me.GetCurrTarget();
            if (SMNSettings.Instance.AutoStopForSpecialBuff)
            {
                ImGui.Text($"需要停手：{SMNBattleData.Instance.NeedStop}");
                ImGui.Text($"自动停手已触发：{SMNBattleData.Instance.AutoStopTriggered}");
            }
            ImGui.Text($"Attunement层数：{Core.Resolve<JobApi_Summoner>().AttunementAdjust}");
            var type = "";
            switch (Core.Resolve<JobApi_Summoner>().ActivePetType)
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
                case ActivePetType.SolarBahamut:
                    type = "太阳龙神";
                    break;
            }

            ImGui.Text($"召唤兽类别：{type}");
            ImGui.Text($"召唤兽时间：{Core.Resolve<JobApi_Summoner>().SummonTimerRemaining}");
            ImGui.Text($"三神时间：{Core.Resolve<JobApi_Summoner>().AttunmentTimerRemaining}");
            // ImGui.Text($"山崩预备: {Core.Me.HasMyAura(AurasDefine.TitansFavor)}");
            //ImGui.Text($"能力技次数: {AI.Instance.BattleData.GetReceiveAbilityObjects().Count}");
            //ImGui.SameLine();
            //if (ImGui.Button("手动归零##AbilityCount"))
            //{
            //AI.Instance.BattleData.AbilityCount = 0;
            //}
            // ImGui.Text($"宝石耀属性: {Core.Resolve<MemApiSpell>().GetSpellType(SMNData.Spells.Gemshine.GetSpell().Id)}");
            if (target != null)
            {
                ImGui.Text($"距离Melee: {Core.Me.Distance(target, AEAssist.Define.DistanceMode.IgnoreTargetHitbox)}");
                ImGui.Text($"距离: {Core.Me.Distance(target)}");
            }
            ImGui.Text($"自定义等待队列宝宝数量: {SMNBattleData.Instance.CustomSummonWaitList.Count}");
            ImGui.SameLine();
            if (ImGui.Button("手动归零##CustomSummonWaitList"))
            {
                SMNBattleData.Instance.CustomSummonWaitList.Clear();
            }

            ImGui.Text($"自定义宝宝数量: {SMNBattleData.Instance.CustomSummon.Count}");
            ImGui.SameLine();
            if (ImGui.Button("手动归零##CustomSummon"))
            {
                SMNBattleData.Instance.CustomSummon.Clear();
            }
#if DEBUG
            var deadTarget = PartyHelper.DeadAllies.FirstOrDefault(r => !r.HasAura(SMNData.Buffs.Raise));
            var helperTarget = SMNHelper.GetDeadChara();
            if (deadTarget != null|| helperTarget != null) {
                ImGui.Text($"复活目标: {deadTarget.Name}");
                ImGui.Text($"复活目标IsTargetable: {deadTarget.IsTargetable}");
                ImGui.Text($"复活目标有复苏: {deadTarget.HasAura(SMNData.Buffs.Raise)}");
                ImGui.Text($"复活目标IsValid: {deadTarget.IsValid()}");
                ImGui.Text($"Helper给的目标: {helperTarget?.Name}");

            }
            ImGui.Text($"Duty相关");
            ImGui.Text($"IsLastDutyTaskAE：{LNMHelper.IsLastDutyTaskAE()}");
            ImGui.Text($"IsLastDutyTask：{LNMHelper.IsLastDutyTask()}");
            if (Core.Resolve<MemApiDuty>().GetSchedule() != null)
            {
                ImGui.Text($"AENowPoint: {Core.Resolve<MemApiDuty>().GetSchedule()?.NowPoint}");
                ImGui.Text($"AECountPoint: {Core.Resolve<MemApiDuty>().GetSchedule()?.CountPoint}");
            }

            var dutys = LNMHelper.GetTask();
            if (!dutys.Equals(new StdVector<EventHandlerObjective>()))
            {
                foreach (var duty in dutys)
                {
                    var name = duty.Label.ToString();
                    if (name.IsNullOrEmpty()) continue;
                    ImGui.Text($"Name:{name}");
                    ImGui.Text($"CountCurrent:{duty.CountCurrent}");
                    ImGui.Text($"CountNeeded:{duty.CountNeeded}");
                }
            }
            ImGui.Text($"InBossBattle: {Core.Resolve<MemApiDuty>().InBossBattle}");
#endif
            ImGui.Text("高优先级队列GCD:");
            var gcdHigh = AI.Instance.BattleData.HighPrioritySlots_GCD;
            var ogcdHigh = AI.Instance.BattleData.HighPrioritySlots_OffGCD;
            if (gcdHigh.Count != 0)
            {
                ImGui.Indent();
                foreach (var action in gcdHigh.SelectMany(gcds => gcds.Actions))
                {
                    ImGui.Text($"{action.Spell.Name}");
                }
                ImGui.Unindent();
            }
            ImGui.Text("高优先级队列oGCD:");
            if (ogcdHigh.Count != 0)
            {
                ImGui.Indent();
                foreach (var action in ogcdHigh.SelectMany(ogcds => ogcds.Actions))
                {
                    ImGui.Text($"{action.Spell.Name}");
                }
                ImGui.Unindent();
            }
                
            ImGui.Text($"TTK相关");
            ImGui.Text($"最终BOSS：{SMNBattleData.Instance.FinalBoss}");
            ImGui.Text($"TTKTriggered：{SMNBattleData.Instance.TTKTriggered}");
            ImGui.Text($"IsLastTask：{LNMHelper.IsLastTask()}");
            if (target != null)
                ImGui.Text($"IsBOSS: {target.IsBoss()}");
            // ImGui.Text($"优先火神GD: {SMNSettings.Instance.RubyGCDFirst && SMNBattleData.Instance.IfritGemshineTimes > 0}");
            // ImGui.Text($"灼热之光时间剩余时间(ms): {Core.Me.GetBuffTimespanLeft(AurasDefine.SearingLight).TotalMilliseconds}");
            // ImGui.Text($"灼热之光buff: {Core.Me.HasMyAura(AurasDefine.SearingLight)}");
            // ImGui.Text($"能量吸收2GCD内: {SMNData.Spells.EnergyDrain.CoolDownInGCDs(2)}");
            // ImGui.Text($"SummonTimerRemaining小于4GCD: {Core.Resolve<JobApi_Summoner>().SummonTimerRemaining <= Core.Resolve<MemApiSpell>().GetGCDDuration(false) * 4}");
            // ImGui.Text($"召唤了亚灵神: {Core.Resolve<JobApi_Summoner>().TranceTimer <= 0 && Core.Resolve<JobApi_Summoner>().SummonTimerRemaining > 0 && Core.Resolve<JobApi_Summoner>().ActivePetType == ActivePetType.None}");
            // ImGui.Text($"伊芙利特模式: {SMNSettings.Instance.IfritMode}");
            // ImGui.Text($"在巴哈姆特状态: {Core.Resolve<JobApi_Summoner>().InBahamut}");
            // ImGui.Text($"在巴哈姆特状态old: {Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Bahamut)}");
            // var IsTitan = Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Titan);
            // var IsIfrit = Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Ifrit);
            // var IsGaruda = Core.Resolve<JobApi_Summoner>().IsPetReady(ActivePetType.Garuda);
            // ImGui.Text($"土神PetReady: {IsTitan}");
            // ImGui.Text($"火神PetReady: {IsIfrit}");
            // ImGui.Text($"风神PetReady: {IsGaruda}");
        }
    }
}