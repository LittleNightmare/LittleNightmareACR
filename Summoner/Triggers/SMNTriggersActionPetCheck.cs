using CombatRoutine.TriggerModel;
using Common;
using Common.Language;
using ImGuiNET;

namespace LittleNightmare.Summoner.Triggers
{
    public class SMNTriggersActionPetCheck : ITriggerCond
    {
        public ActivePetType ActivePetType;

        public string preview = "";

        public string DisplayName => "SMN/LittleNightmare/检测在场召唤兽类型".Loc();

        public string Remark { get; set; }

        public bool Draw()
        {
            if (ImGui.BeginCombo("召唤兽".Loc(), preview))
            {
                if (ImGui.Selectable("无".Loc(), ActivePetType == ActivePetType.None))
                {
                    ActivePetType = ActivePetType.None;
                    preview = "无".Loc();
                }

                if (ImGui.Selectable("土神".Loc(), ActivePetType == ActivePetType.Titan))
                {
                    ActivePetType = ActivePetType.Titan;
                    preview = "土神".Loc();
                }

                if (ImGui.Selectable("火神".Loc(), ActivePetType == ActivePetType.Ifrit))
                {
                    ActivePetType = ActivePetType.Ifrit;
                    preview = "火神".Loc();
                }

                if (ImGui.Selectable("风神".Loc(), ActivePetType == ActivePetType.Garuda))
                {
                    ActivePetType = ActivePetType.Garuda;
                    preview = "风神".Loc();
                }

                ImGui.EndCombo();
            }

            return true;
        }

        public bool Handle(ITriggerCondParamas condParamas)
        {
            if (Core.Get<IMemApiSummoner>().ActivePetType == ActivePetType)
            {
                return true;
            }

            return false;
        }
    }
}