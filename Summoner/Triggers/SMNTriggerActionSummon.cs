using AEAssist.CombatRoutine.Trigger;
using ImGuiNET;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionSummon : ITriggerAction
{
    public string DisplayName => "SMN/选择下一个召唤的蛮神";
    public string Remark { get; set; }

    public int NextSummon { get; set; } = new();
    private string? PrevSummon;

    public void Check()
    {
    }

    public bool Draw()
    {
        if (NextSummon == 0)
        {
            PrevSummon = "土神";
        }
        else if (NextSummon == 1)
        {
            PrevSummon = "风神";
        }
        else if (NextSummon == 2)
        {
            PrevSummon = "火神";
        }

        if (ImGui.BeginCombo("", PrevSummon))
        {
            if (ImGui.Selectable("土神"))
            {
                NextSummon = 0;
            }

            if (ImGui.Selectable("风神"))
            {
                NextSummon = 1;
            }

            if (ImGui.Selectable("火神"))
            {
                NextSummon = 2;
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
                SMNBattleData.Instance.TitanFirst();
                break;
            case 1:
                SMNBattleData.Instance.GarudaFirst();
                break;
            case 2:
                SMNBattleData.Instance.IfritFirst();
                break;
        }

        return true;
    }
}