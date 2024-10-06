using AEAssist.CombatRoutine.Trigger;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionBahamutPhoenix : ITriggerAction
{
    public string DisplayName => "SMN/LittleNightmare/巴哈凤凰开关";
    public string Remark { get; set; }

    public bool useBahamutPhoenix { get; set; } = new();

    public void Check()
    {

    }

    public bool Draw()
    {
        return false;
    }

    public bool Handle()
    {
        return SummonerRotationEntry.QT.SetQt("巴哈凤凰", useBahamutPhoenix);
    }
}