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
            return true;
        }

        public bool Handle(ITriggerCondParamas condParamas)
        {
            return Core.Get<IMemApiSummoner>().PetTimer == 0;
        }
    }
}