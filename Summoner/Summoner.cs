using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using AEAssist.Extension;
using AEAssist.GUI;
using AEAssist.JobApi;
using ImGuiNET;
using System.Numerics;

namespace LittleNightmare.Summoner
{
    public class SummonerOverlay : JobViewWindow
    {
        private static readonly string[] swiftcastModes = { "即刻复活", "风神读条", "火神读条", "全部" };
        private static readonly string[] IfritModes = { "先冲锋再读条", "先读条再冲锋", "读条-冲锋-读条" };
        // public string? SwiftcastMode;
        public Dictionary<string,string> OpenerDictionary = Enum.GetNames<SMNSettings.OpenerType>().ToDictionary(t => t, t => t);


        public SummonerOverlay(JobViewSave jobViewSave, Action save, string name) : base(jobViewSave, save, name)
        {

            AddTab("通常", DrawQtGeneral);
            // AddTab("时间轴信息", DrawTriggerlineInfo);
            // AddTab("Qt", DrawQtSettingView);
            // AddTab("风格", DrawChangeStyleView);
            AddTab("调试", DrawDebugView);


            AddQt("爆发", true);
            AddQt("爆发药", SMNSettings.Instance.qt自动爆发药);
            AddQt("AOE", true);
            AddQt("最终爆发", false);
            AddQt("灼热之光", true);
            AddQt("三神召唤", true);
            AddQt("巴哈凤凰", true);
            AddQt("宝石耀", true);
            AddQt("自动火神冲", SMNSettings.Instance.qt自动火神冲);

            AddHotkey("LB", new HotKeyResolver_LB());
            AddHotkey("沉稳咏唱", new HotKeyResolver_NormalSpell(SpellsDefine.Surecast, SpellTargetType.Self));
            AddHotkey("昏乱", new HotKeyResolver_NormalSpell(SpellsDefine.Addle, SpellTargetType.Target));
            AddHotkey("疾跑", new HotKeyResolver_疾跑());
            AddHotkey("守护之光", new HotKeyResolver_NormalSpell(SMNData.Spells.RadiantAegis, SpellTargetType.Self));
            AddHotkey("日光普照", new HotKeyResolver_NormalSpell(SMNData.Spells.LuxSolaris, SpellTargetType.Self));
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


            //ImGuiHelper.DrawEnum("召唤起手", ref SMNSettings.Instance.SelectedOpener, nameMap: OpenerDictionary);

            ImGui.Checkbox("目标圈内移动时使用火神冲", ref SMNSettings.Instance.SlideUseCrimonCyclone);
            ImGuiHelper.SetHoverTooltip("移动时，如果在目标圈上，使用火神冲\n不然尝试其他的技能，比如毁4");
            ImGui.Checkbox("优先毁三填充最后GCD窗口", ref SMNSettings.Instance.UseRuinIIIFirst);
            ImGuiHelper.SetHoverTooltip("在GCD填充时，如果不移动，能量吸收还没马上好，优先毁3填充，再是毁4");
            // ImGui.Checkbox("优先火神GCD", ref SMNSettings.Instance.RubyGCDFirst);
            // ImGuiHelper.SetHoverTooltip("在不移动时，优先使用火神GCD，而不是火神冲");
            ImGui.TextDisabled("Qt的描述可以看ACR的设置界面");

        }

        public void DrawDebugView(JobViewWindow window)
        {
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

            ImGui.Text($"距离Melee: {Core.Me.Distance(Core.Me.GetCurrTarget(), AEAssist.Define.DistanceMode.IgnoreTargetHitbox)}");
            ImGui.Text($"距离: {Core.Me.Distance(Core.Me.GetCurrTarget())}");

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