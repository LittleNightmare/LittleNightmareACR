using AEAssist.CombatRoutine.Trigger;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionSearingLight : ITriggerAction
{
    public string DisplayName => "SMN/灼热之光开关";
    public string Remark { get; set; }

    public bool 灼热之光 { get; set; } = new();

    public void Check()
    {

    }

    public bool Draw()
    {
        return false;
    }

    public bool Handle()
    {
        return SummonerRotationEntry.QT.SetQt("灼热之光", 灼热之光);
    }
}