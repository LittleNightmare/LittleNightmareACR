using CombatRoutine.TriggerModel;
using Common.Language;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionSearingLight : ITriggerAction
{
    public string DisplayName => "SMN/LittleNightmare/灼热之光开关".Loc();
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
        return Qt.SetQt("灼热之光".Loc(), 灼热之光);
    }
}