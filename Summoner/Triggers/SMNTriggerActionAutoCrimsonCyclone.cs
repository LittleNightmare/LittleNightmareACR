using CombatRoutine.TriggerModel;
using Common;
using Common.Language;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionAutoCrimsonCyclone : ITriggerAction
{
    public string DisplayName => "SMN/LittleNightmare/自动火神冲开关".Loc();

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
        return Qt.SetQt("自动火神冲".Loc(), AutoCrimsonCyclone);
    }
}