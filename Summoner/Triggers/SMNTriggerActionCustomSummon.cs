using CombatRoutine.TriggerModel;
using Common;
using Common.GUI;
using Common.Language;
using ImGuiNET;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionCustomSummon : ITriggerAction
{
    public string DisplayName => "SMN/LittleNightmare/蛮神队列添加".Loc();
    public string Remark { get; set; }

    public int NextSummon { get; set; }
    private string? PrevSummon;
    public bool ClearPrevoiusSummon;

    public void Check()
    {

    }

    public bool Draw()
    {
        switch (NextSummon)
        {
            case 0:
                PrevSummon = "土-火-风";
                break;
            case 1:
                PrevSummon = "土-风-火";
                break;
            case 2:
                PrevSummon = "火-土-风";
                break;
            case 3:
                PrevSummon = "火-风-土";
                break;
            case 4:
                PrevSummon = "风-土-火";
                break;
            case 5:
                PrevSummon = "风-火-土";
                break;
        }
        if (ImGui.BeginCombo("", PrevSummon))
        {
            if (ImGui.Selectable("土-火-风".Loc()))
            {
                NextSummon = 0;
            }
            if (ImGui.Selectable("土-风-火".Loc()))
            {
                NextSummon = 1;
            }
            if (ImGui.Selectable("火-土-风".Loc()))
            {
                NextSummon = 2;
            }
            if (ImGui.Selectable("火-风-土".Loc()))
            {
                NextSummon = 3;
            }
            if (ImGui.Selectable("风-土-火".Loc()))
            {
                NextSummon = 4;
            }
            if (ImGui.Selectable("风-火-土".Loc()))
            {
                NextSummon = 5;
            }
            ImGui.EndCombo();
        }
        
        ImGui.Checkbox("##ClearPrevoiusSummon", ref ClearPrevoiusSummon);
        ImGui.SameLine();
        ImGui.Text("添加前清除当前自定义的蛮神队列".Loc());
        ImGuiHelper.SetHoverTooltip("因为蛮神队列是添加到队尾\n有时需要清空队列，以保证符合预期".Loc());
        
        return true;
    }

    public bool Handle()
    {
        if (ClearPrevoiusSummon)
        {
            SMNBattleData.Instance.CustomSummonWaitList.Clear();
            SMNBattleData.Instance.CustomSummon.Clear();
        }
        
        switch (NextSummon)
        {
            case 0:
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Titan());
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Ifrit());
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Garuda());
                break;
            case 1:
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Titan());
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Garuda());
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Ifrit());
                break;
            case 2:
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Ifrit());
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Titan());
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Garuda());
                break;
            case 3:
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Ifrit());
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Garuda());
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Titan());
                break;
            case 4:
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Garuda());
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Titan());
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Ifrit());
                break;
            case 5:
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Garuda());
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Ifrit());
                SMNBattleData.Instance.CustomSummonWaitList.Add(SMNSpellHelper.Titan());
                break;
        }
        return true;
    }
}