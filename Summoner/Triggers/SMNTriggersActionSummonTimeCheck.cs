using CombatRoutine.TriggerModel;
using Common;
using Common.Language;
using ImGuiNET;

namespace LittleNightmare.Summoner.Triggers
{
    internal class SMNTriggersActionSummonTimeCheck : ITriggerCond
    {
        public string DisplayName => "SMN/LittleNightmare/召唤兽变身时间检测(宝宝盾必备)".Loc();

        public string Remark { get; set; }

        public bool Draw()
        {
            ImGui.Text("添加即可".Loc());
            ImGui.Text("用于检查SummonTimerRemaning数值");
            ImGui.Text("大于0返回false，等于0返回true");
            return true;
        }

        public bool Handle(ITriggerCondParams condParamas)
        {
            return Core.Get<IMemApiSummoner>().PetTimer == 0;
        }
    }
}