using CombatRoutine.TriggerModel;
using Common.Language;
using ImGuiNET;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionSwiftCastMode : ITriggerAction
{
    public string DisplayName => "SMN/LittleNightmare/修改即刻咏唱模式".Loc();

    public int 即刻咏唱模式 { get; set; } = new();
    private string? Preview;

    public void Check()
    {

    }

    public bool Draw()
    {
        if (即刻咏唱模式 == 0)
        {
            Preview = "即刻复活".Loc();
        }
        else if (即刻咏唱模式 == 1)
        {
            Preview = "风神读条".Loc();
        }
        else if (即刻咏唱模式 == 2)
        {
            Preview = "火神读条".Loc();
        }

        if (ImGui.BeginCombo("", Preview))
        {
            if (ImGui.Selectable("即刻复活".Loc()))
            {
                即刻咏唱模式 = 0;
            }
            if (ImGui.Selectable("风神读条".Loc()))
            {
                即刻咏唱模式 = 1;
            }
            if (ImGui.Selectable("火神读条".Loc()))
            {
                即刻咏唱模式 = 2;
            }
            ImGui.EndCombo();
        }
        return true;
    }

    public bool Handle()
    {
        SMNSettings.Instance.即刻咏唱模式 = 即刻咏唱模式;
        return true;
    }
}