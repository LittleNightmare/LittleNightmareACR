using CombatRoutine.TriggerModel;
using Common.Language;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionPreCastSwiftcast : ITriggerAction
{
    public string DisplayName => "SMN/LittleNightmare/预读风神即刻咏唱".Loc();

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
        return Qt.SetQt("预读风神即刻咏唱".Loc(), 预读风神即刻咏唱);
    }
}