using CombatRoutine.TriggerModel;
using Common.Language;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionGemshine : ITriggerAction
{
    public string DisplayName => "SMN/LittleNightmare/宝石耀开关".Loc();
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
        return Qt.SetQt("宝石耀".Loc(), 宝石耀);
    }
}