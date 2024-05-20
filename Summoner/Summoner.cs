using CombatRoutine.Setting;
using CombatRoutine;
using CombatRoutine.View;
using CombatRoutine.View.JobView;
using ImGuiNET;
using System.Numerics;
using Common.Language;
using Common;
using Common.Define;
using Common.GUI;
using Common.Helper;

namespace LittleNightmare.Summoner
{
    public class SummonerOverlay : JobViewWindow
    {
        private readonly string[] swiftcastModes = { "即刻复活".Loc(), "风神读条".Loc(), "火神读条".Loc(), "全部".Loc() };
        private readonly string[] IfritModes = { "先冲锋再读条".Loc(), "先读条再冲锋".Loc(), "读条-冲锋-读条".Loc() };
        // public string? SwiftcastMode;
        public Dictionary<string,string> OpenerDictionary = Enum.GetNames<SMNSettings.OpenerType>().ToDictionary(t => t, t => t.Loc());


        public SummonerOverlay(JobViewSave jobViewSave, Action save, string name) : base(jobViewSave, save, name)
        {
            SetUpdateAction(OnUIUpdate);

            AddTab("通常".Loc(), DrawQtGeneral);
            // AddTab("时间轴信息".Loc(), DrawTriggerlineInfo);
            // AddTab("Qt".Loc(), DrawQtSettingView);
            // AddTab("风格".Loc(), DrawChangeStyleView);
            AddTab("调试".Loc(), DrawDebugView);


            AddQt("爆发".Loc(), true);
            AddQt("爆发药".Loc(), SMNSettings.Instance.qt自动爆发药);
            AddQt("AOE".Loc(), true);
            AddQt("最终爆发".Loc(), false);
            AddQt("灼热之光".Loc(), true);
            AddQt("三神召唤".Loc(), true);
            AddQt("巴哈凤凰".Loc(), true);
            AddQt("宝石耀".Loc(), true);
            AddQt("自动火神冲".Loc(), SMNSettings.Instance.qt自动火神冲);

        }

        

        public void OnUIUpdate()
        {
            
        }

        public void DrawQtGeneral(JobViewWindow jobViewWindow)
        {
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

            ImGui.Text("土神次数".Loc() + SMNBattleData.Instance.TitanGemshineTimes.ToString());
            ImGui.SameLine();
            ImGui.Text("火神次数".Loc() + SMNBattleData.Instance.IfritGemshineTimes.ToString());
            ImGui.SameLine();
            ImGui.Text("风神次数".Loc() + SMNBattleData.Instance.GarudaGemshineTimes.ToString());

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
            // ImGui.Text("开场爆发中: ".Loc() + SMNBattleData.Instance.In90Opener);

            ImGui.SetNextItemWidth(200f);
            if (ImGui.BeginCombo("即刻咏唱模式".Loc(), swiftcastModes[SMNSettings.Instance.SwiftCastMode]))
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
            ImGuiHelper.SetHoverTooltip("起手会强制启用即刻风神".Loc());
            ImGui.SetNextItemWidth(200f);
            if (ImGui.BeginCombo("火神施法模式".Loc(), IfritModes[SMNSettings.Instance.IfritMode]))
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

            // ImGui.Text("即刻技能GCD数量".Loc() + SMNBattleData.Instance.GCDLeftUntilNextSwiftCasted());
            // ImGui.SameLine();
            // ImGui.Text("可以释放".Loc() + SMNBattleData.Instance.CastSwiftCastCouldCoverTargetSpell());


            ImGuiHelper.DrawEnum("召唤起手".Loc(), ref SMNSettings.Instance.SelectedOpener, nameMap: OpenerDictionary);

            ImGui.Checkbox("目标圈内移动时使用火神冲".Loc(), ref SMNSettings.Instance.SlideUseCrimonCyclone);
            ImGuiHelper.SetHoverTooltip("移动时，如果在目标圈上，使用火神冲\n不然尝试其他的技能，比如毁4".Loc());
            ImGui.Checkbox("优先毁三填充最后GCD窗口".Loc(), ref SMNSettings.Instance.UseRuinIIIFirst);
            ImGuiHelper.SetHoverTooltip("在GCD填充时，如果不移动，能量吸收还没马上好，优先毁3填充，再是毁4".Loc());
            // ImGui.Checkbox("优先火神GCD".Loc(), ref SMNSettings.Instance.RubyGCDFirst);
            // ImGuiHelper.SetHoverTooltip("在不移动时，优先使用火神GCD，而不是火神冲".Loc());
            ImGui.TextDisabled("Qt的描述可以看ACR的设置界面".Loc());

        }

        public void DrawTriggerlineInfo(JobViewWindow jobViewWindow)
        {
            OverlayHelper.DrawTriggerlineInfo();
        }

        public void DrawQtSettingView(JobViewWindow window)
        {
            QtSettingView();
        }

        public void DrawChangeStyleView(JobViewWindow window)
        {
            ChangeStyleView();
        }

        public void DrawDebugView(JobViewWindow window)
        {
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
            // ImGui.Text($"山崩预备: {Core.Me.HasMyAura(AurasDefine.TitansFavor)}");
            ImGui.Text($"能力技次数: {AI.Instance.BattleData.AbilityCount}");
            ImGui.SameLine();
            if (ImGui.Button("手动归零##AbilityCount"))
            {
                AI.Instance.BattleData.AbilityCount = 0;
            }
            // ImGui.Text($"宝石耀属性: {Core.Get<IMemApiSpell>().GetSpellType(SpellsDefine.Gemshine.GetSpell().Id)}");
            ImGui.Text($"距离Melee: {Core.Me.DistanceMelee(Core.Me.GetCurrTarget())}");
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
            // ImGui.Text($"能量吸收2GCD内: {SpellsDefine.EnergyDrain.CoolDownInGCDs(2)}");
            // ImGui.Text($"PetTimer小于4GCD: {Core.Get<IMemApiSummoner>().PetTimer <= Core.Get<IMemApiSpell>().GetGCDDuration(false) * 4}");
            // ImGui.Text($"召唤了亚灵神: {Core.Get<IMemApiSummoner>().TranceTimer <= 0 && Core.Get<IMemApiSummoner>().PetTimer > 0 && Core.Get<IMemApiSummoner>().ActivePetType == ActivePetType.None}");
            // ImGui.Text($"伊芙利特模式: {SMNSettings.Instance.IfritMode}");
            // ImGui.Text($"在巴哈姆特状态: {Core.Get<IMemApiSummoner>().InBahamut}");
            // ImGui.Text($"在巴哈姆特状态old: {Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Bahamut)}");
            // var IsTitan = Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Titan);
            // var IsIfrit = Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Ifrit);
            // var IsGaruda = Core.Get<IMemApiSummoner>().IsPetReady(ActivePetType.Garuda);
            // ImGui.Text($"土神PetReady: {IsTitan}");
            // ImGui.Text($"火神PetReady: {IsIfrit}");
            // ImGui.Text($"风神PetReady: {IsGaruda}");
        }
    }
}