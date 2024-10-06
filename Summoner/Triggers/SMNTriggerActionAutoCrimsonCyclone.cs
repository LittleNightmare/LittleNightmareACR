
using AEAssist.CombatRoutine.Trigger;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionAutoCrimsonCyclone : ITriggerAction
{
    public string DisplayName => "SMN/LittleNightmare/自动火神冲开关";
    public string Remark { get; set; }

    public bool AutoCrimsonCyclone { get; set; } = new();

    public void Check()
    {

    }

    public bool Draw()
    {
        return false;
    }

    public bool Handle()
    {
        return SummonerRotationEntry.QT.SetQt("自动火神冲", AutoCrimsonCyclone);
    }
}