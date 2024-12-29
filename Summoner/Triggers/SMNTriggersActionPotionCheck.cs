using AEAssist.CombatRoutine.Trigger;
using ImGuiNET;

namespace LittleNightmare.Summoner.Triggers
{
    internal class SMNTriggersPotionCheck : ITriggerCond
    {
        public string DisplayName => "SMN/爆发药是否开启检测";

        public string Remark { get; set; }

        public bool Draw()
        {
            ImGui.Text("添加即可");
            ImGui.Text("用于告知QT中的爆发药是否开启");
            return true;
        }

        public bool Handle(ITriggerCondParams condParamas)
        {
            return SummonerRotationEntry.QT.GetQt("爆发药");
        }
    }
}