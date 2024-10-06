using AEAssist.CombatRoutine.Trigger;
using ImGuiNET;
using AEAssist.CombatRoutine;
using AEAssist.GUI;
using AEAssist;
using AEAssist.JobApi;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerGaugeCheck : ITriggerCond
{
    

    public string DisplayName => "SMN/量谱条件";
    public string Remark { get; set; }

    public ActivePetType CheckSummon;
    public int SummonTimes;
    public bool UseGaugeTimesDirectly;
    private string PrevSummon;
    private int _min = 0;
    private int _max = 4; 
    public bool Draw()
    {
        switch (CheckSummon)
        {
            case ActivePetType.Titan:
                PrevSummon = "土神";
                break;
            case ActivePetType.Garuda:
                PrevSummon = "风神";
                break;
            case ActivePetType.Ifrit:
                PrevSummon = "火神";
                break;
        }

        if (ImGui.BeginCombo("", PrevSummon))
        {
            if (ImGui.Selectable("土神"))
            {
                CheckSummon = ActivePetType.Titan;
                _min = 0;
                _max = 4;
            }
            if (ImGui.Selectable("风神"))
            {
                CheckSummon = ActivePetType.Garuda;
                _min = 0;
                _max = 4;
            }
            if (ImGui.Selectable("火神"))
            {
                CheckSummon = ActivePetType.Ifrit;
                _min = 0;
                _max = 4;
            }
            ImGui.EndCombo();
        }
        ImGui.Text("剩余次数等于: ");
        ImGui.SameLine();
        ImGuiHelper.LeftInputInt("次", ref SummonTimes, UseGaugeTimesDirectly? 1 + _min : _min, _max);
        ImGuiHelper.SetHoverTooltip("举例: 三神刚出现的窗口，用最大值，比如土神设置为4\n用完第一次宝石耀的窗口，用3");
        ImGui.Checkbox("直接使用量谱次数", ref UseGaugeTimesDirectly);
        ImGuiHelper.SetHoverTooltip("如果不勾选，这个检测会受到自定义次数的影响\n勾选后，直接读取量谱，但不能检测次数为0的情况");
        return true;
    }
    public bool Handle(ITriggerCondParams condParamas = null)
    {
        if (UseGaugeTimesDirectly)
        {
            // TODO: 量谱归零时，这个检测会失效
            if (Core.Resolve<JobApi_Summoner>().ActivePetType == CheckSummon)
            {
                return Core.Resolve<JobApi_Summoner>().AttunementAdjust == SummonTimes;
            }
        }
        else
        {
            switch (CheckSummon)
            {
                case ActivePetType.Titan:
                    return SMNBattleData.Instance.TitanGemshineTimes == SummonTimes;
                case ActivePetType.Garuda:
                    return SMNBattleData.Instance.GarudaGemshineTimes == SummonTimes;
                case ActivePetType.Ifrit:
                    return SMNBattleData.Instance.IfritGemshineTimes == SummonTimes;
            }
        }
        return false;
    }

}