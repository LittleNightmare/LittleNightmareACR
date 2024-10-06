using AEAssist.CombatRoutine.Trigger;
using ImGuiNET;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionCustomGemshineTimes : ITriggerAction
{
    public string DisplayName => "SMN/LittleNightmare/下一轮三神技能使用次数";
    public string Remark { get; set; }

    public int TitanGemshineTimes = 4;
    public int IfritGemshineTimes = 2;
    public int GarudaGemshineTimes = 4;

    public void Check()
    {
        if (TitanGemshineTimes < 0 || TitanGemshineTimes > 4)
        {
            throw new Exception("TitanGemshineCastTimes must be >= 0 && <= 4");
        }
        if (IfritGemshineTimes < 0 || IfritGemshineTimes > 2)
        {
            throw new Exception("IfritGemshineTimes must be >= 0 && <= 2");
        }
        if (GarudaGemshineTimes < 0 || GarudaGemshineTimes > 4)
        {
            throw new Exception("GarudaGemshineTimes must be >= 0 && <= 4");
        }
    }

    public bool Draw()
    {
        if (ImGui.InputInt("土神", ref TitanGemshineTimes))
        {
            TitanGemshineTimes = Math.Clamp(TitanGemshineTimes, 0, 4);
        };
        if (ImGui.InputInt("火神", ref IfritGemshineTimes))
        {
            IfritGemshineTimes = Math.Clamp(IfritGemshineTimes, 0, 2);
        };
        if (ImGui.InputInt("风神", ref GarudaGemshineTimes))
        {
            GarudaGemshineTimes = Math.Clamp(GarudaGemshineTimes, 0, 4);
        };
        return true;
    }

    public bool Handle()
    {
        SMNBattleData.Instance.TitanGemshineTimesCustom = TitanGemshineTimes;
        SMNBattleData.Instance.IfritGemshineTimesCustom = IfritGemshineTimes;
        SMNBattleData.Instance.GarudaGemshineTimesCustom = GarudaGemshineTimes;
        return true;
    }
}