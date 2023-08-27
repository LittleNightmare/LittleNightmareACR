using CombatRoutine.TriggerModel;
using Common.Language;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionBahamutPhoenix : ITriggerAction
{
    public string DisplayName => "SMN/LittleNightmare/巴哈凤凰开关".Loc();

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
        return Qt.SetQt("巴哈凤凰".Loc(), useBahamutPhoenix);
    }
}