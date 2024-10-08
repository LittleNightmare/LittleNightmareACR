using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using AEAssist.JobApi;
using ImGuiNET;

namespace LittleNightmare.Summoner.Triggers
{
    public class SMNTriggersActionPetCheck : ITriggerCond
    {
        public ActivePetType ActivePetType;

        public string preview = "";

        public string DisplayName => "SMN/检测在场召唤兽类型";

        public string Remark { get; set; }

        public bool Draw()
        {
            if (ImGui.BeginCombo("召唤兽", preview))
            {
                if (ImGui.Selectable("无", ActivePetType == ActivePetType.None))
                {
                    ActivePetType = ActivePetType.None;
                    preview = "无";
                }

                if (ImGui.Selectable("土神", ActivePetType == ActivePetType.Titan))
                {
                    ActivePetType = ActivePetType.Titan;
                    preview = "土神";
                }

                if (ImGui.Selectable("火神", ActivePetType == ActivePetType.Ifrit))
                {
                    ActivePetType = ActivePetType.Ifrit;
                    preview = "火神";
                }

                if (ImGui.Selectable("风神", ActivePetType == ActivePetType.Garuda))
                {
                    ActivePetType = ActivePetType.Garuda;
                    preview = "风神";
                }

                ImGui.EndCombo();
            }

            ImGuiHelper.SetHoverTooltip("当前召唤兽为无时，可能包涵巴哈和凤凰");

            return true;
        }

        public bool Handle(ITriggerCondParams condParamas)
        {
            if (Core.Resolve<JobApi_Summoner>().ActivePetType == ActivePetType)
            {
                return true;
            }

            return false;
        }
    }
}