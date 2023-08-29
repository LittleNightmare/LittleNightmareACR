using CombatRoutine.TriggerModel;
using Common.Language;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionUseSummon : ITriggerAction
{
    public string DisplayName => "SMN/LittleNightmare/三神召唤开关".Loc();
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
        return Qt.SetQt("三神召唤".Loc(), 三神召唤);
    }
}