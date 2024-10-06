using AEAssist.CombatRoutine.Trigger;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionGemshine : ITriggerAction
{
    public string DisplayName => "SMN/宝石耀开关";
    public string Remark { get; set; }

    public bool 宝石耀 { get; set; } = new();

    public void Check()
    {

    }

    public bool Draw()
    {
        return false;
    }

    public bool Handle()
    {
        return SummonerRotationEntry.QT.SetQt("宝石耀", 宝石耀);
    }
}