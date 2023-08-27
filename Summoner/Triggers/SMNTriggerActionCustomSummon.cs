using CombatRoutine.TriggerModel;
using Common.Language;
using ImGuiNET;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionCustomSummon : ITriggerAction
{
    public string DisplayName => "SMN/LittleNightmare/蛮神队列添加".Loc();

    public int NextSummon { get; set; } = new();
    private string? PrevSummon;

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
        return true;
    }

    public bool Handle()
    {
        switch (NextSummon)
        {
            case 0:
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Titan());
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Ifrit());
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Garuda());
                break;
            case 1:
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Titan());
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Garuda());
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Ifrit());
                break;
            case 2:
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Ifrit());
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Titan());
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Garuda());
                break;
            case 3:
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Ifrit());
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Garuda());
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Titan());
                break;
            case 4:
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Garuda());
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Titan());
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Ifrit());
                break;
            case 5:
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Garuda());
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Ifrit());
                SMNBattleData.Instance.CustomSummon.Add(SMNSpellHelper.Titan());
                break;
        }
        return true;
    }
}