using AEAssist.CombatRoutine.Trigger;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionPreCastSwiftcast : ITriggerAction
{
    public string DisplayName => "SMN/预读风神即刻咏唱";
    public string Remark { get; set; }

    public bool 预读风神即刻咏唱 { get; set; } = new();

    public void Check()
    {
    }

    public bool Draw()
    {
        return false;
    }

    public bool Handle()
    {
        return SummonerRotationEntry.QT.SetQt("预读风神即刻咏唱", 预读风神即刻咏唱);
    }
}