using AEAssist.CombatRoutine.Trigger;
using ImGuiNET;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionSwiftCastMode : ITriggerAction
{
    public string DisplayName => "SMN/修改即刻咏唱模式";
    public string Remark { get; set; }

    public int 即刻咏唱模式 { get; set; } = new();
    private string? Preview;

    public void Check()
    {
    }

    public bool Draw()
    {
        Preview = 即刻咏唱模式 switch
        {
            0 => "即刻复活",
            1 => "风神读条",
            2 => "火神读条",
            3 => "全部",
            _ => Preview
        };

        if (ImGui.BeginCombo("", Preview))
        {
            if (ImGui.Selectable("即刻复活"))
            {
                即刻咏唱模式 = 0;
            }

            if (ImGui.Selectable("风神读条"))
            {
                即刻咏唱模式 = 1;
            }

            if (ImGui.Selectable("火神读条"))
            {
                即刻咏唱模式 = 2;
            }

            if (ImGui.Selectable("全部"))
            {
                即刻咏唱模式 = 3;
            }

            ImGui.EndCombo();
        }

        return true;
    }

    public bool Handle()
    {
        SMNSettings.Instance.SwiftCastMode = 即刻咏唱模式;
        return true;
    }
}