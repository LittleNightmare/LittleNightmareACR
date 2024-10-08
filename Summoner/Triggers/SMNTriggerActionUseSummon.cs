using AEAssist.CombatRoutine.Trigger;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionUseSummon : ITriggerAction
{
    public string DisplayName => "SMN/三神召唤开关";
    public string Remark { get; set; }

    public bool 三神召唤 { get; set; } = new();

    public void Check()
    {
    }

    public bool Draw()
    {
        return false;
    }

    public bool Handle()
    {
        return SummonerRotationEntry.QT.SetQt("三神召唤", 三神召唤);
    }
}