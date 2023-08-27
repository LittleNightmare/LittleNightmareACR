using CombatRoutine.TriggerModel;
using Common.Language;
using ImGuiNET;

namespace LittleNightmare.Summoner.Triggers;

public class SMNTriggerActionCustomGemshineTimes : ITriggerAction
{
    public string DisplayName => "SMN/LittleNightmare/下一轮三神技能使用次数".Loc();

    public int TitanGemshineCastTimes = 4;
    public int IfritGemshineCastTimes = 2;
    public int GarudaGemshineCastTimes = 4;

    public void Check()
    {
        if (TitanGemshineCastTimes < 0 || TitanGemshineCastTimes > 4)
        {
            throw new Exception("TitanGemshineCastTimes must be >= 0 && <= 4");
        }
        if (IfritGemshineCastTimes < 0 || IfritGemshineCastTimes > 2)
        {
            throw new Exception("IfritGemshineTimes must be >= 0 && <= 2");
        }
        if (GarudaGemshineCastTimes < 0 || GarudaGemshineCastTimes > 4)
        {
            throw new Exception("GarudaGemshineTimes must be >= 0 && <= 4");
        }
    }

    public bool Draw()
    {
        if (ImGui.InputInt("土神".Loc(), ref TitanGemshineCastTimes))
        {
            TitanGemshineCastTimes = Math.Clamp(TitanGemshineCastTimes, 0, 4);
        };
        if (ImGui.InputInt("火神".Loc(), ref IfritGemshineCastTimes))
        {
            IfritGemshineCastTimes = Math.Clamp(IfritGemshineCastTimes, 0, 2);
        };
        if (ImGui.InputInt("风神".Loc(), ref GarudaGemshineCastTimes))
        {
            GarudaGemshineCastTimes = Math.Clamp(GarudaGemshineCastTimes, 0, 4);
        };
        return true;
    }

    public bool Handle()
    {
        SMNBattleData.Instance.TitanGemshineTimesCustom = TitanGemshineCastTimes;
        SMNBattleData.Instance.IfritGemshineTimesCustom = IfritGemshineCastTimes;
        SMNBattleData.Instance.GarudaGemshineTimesCustom = GarudaGemshineCastTimes;
        return true;
    }
}