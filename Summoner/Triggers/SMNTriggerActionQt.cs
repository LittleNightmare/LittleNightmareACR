using System.Numerics;
using AEAssist.CombatRoutine.Trigger;
using ImGuiNET;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionQt : ITriggerAction
{
    public string DisplayName => "SMN/LittleNightmare/[LittleNightmare]QT设置";
    public string Remark { get; set; }

    private int 当前combo = 0;


    public string ValueName { get; set; } = new("");
    public bool Value { get; set; } = new();
    
    private int radioType;
    private int radioCheck;


    public bool Draw()
    {
        var qtArray = SummonerRotationEntry.QT.GetQtArray();
        当前combo = Array.IndexOf(qtArray,ValueName);
        if (当前combo == -1)
        {
            当前combo = 0;
        }
        radioCheck = Value?0:1;
        //return false;
        if (ImGui.BeginTabBar("###TriggerTab"))
        {
            if (ImGui.BeginTabItem("SMN"))
            {
                ImGui.BeginChild("###TriggerSMN", new Vector2(0,0));
                ImGui.RadioButton("Qt", ref radioType, 0);
                ImGui.NewLine();
                ImGui.SetCursorPos(new Vector2(0,40));
                if (radioType == 0)
                {
                    
                    ImGui.Combo("Qt开关",ref 当前combo,qtArray,qtArray.Length);
                    ValueName = qtArray[当前combo];
                    ImGui.RadioButton("开", ref radioCheck, 0);
                    ImGui.SameLine();
                    ImGui.RadioButton("关", ref radioCheck, 1);
                    Value = radioCheck == 0;
                }
                ImGui.EndChild();
                ImGui.EndTabItem();
            }
            ImGui.EndTabBar();
        }
        return true;
    }

    public bool Handle()
    {
        SummonerRotationEntry.QT.SetQt(ValueName, Value);
        return true;
    }
}