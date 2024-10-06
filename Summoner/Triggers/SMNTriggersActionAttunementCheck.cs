using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.JobApi;
using ImGuiNET;

namespace LittleNightmare.Summoner.Triggers
{
    public class SMNTriggersActionAttunementCheck : ITriggerCond
    {
        public int Attunement;

        public string DisplayName => "SMN/检测三神中已使用次数";

        public string Remark { get; set; }

        public bool Draw()
        {
            ImGui.TextDisabled("我建议用 量谱条件，这个你需要组合 检测在场召唤兽类型 才稳\n另外，这个也不支持检测为4\nBy 动不了源码，阴暗的嘀嘀咕咕的小鬼");
            if (ImGui.InputInt("次数", ref Attunement))
            {
                Attunement = Math.Clamp(Attunement, 0, 4);
            }

            return true;
        }

        public bool Handle(ITriggerCondParams condParamas)
        {
            if (Core.Resolve<JobApi_Summoner>().ActivePetType == ActivePetType.Titan || Core.Resolve<JobApi_Summoner>().ActivePetType == ActivePetType.Garuda)
            {
                return Attunement == 4 - Core.Resolve<JobApi_Summoner>().AttunementAdjust;
            }

            if (Core.Resolve<JobApi_Summoner>().ActivePetType == ActivePetType.Ifrit)
            {
                return Attunement == 2 - Core.Resolve<JobApi_Summoner>().AttunementAdjust;
            }

            return false;
        }
    }
}