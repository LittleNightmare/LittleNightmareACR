using AEAssist.CombatRoutine.Trigger;
using ImGuiNET;

namespace LittleNightmare.Summoner.Triggers
{
    public class SMNTriggerActionIfritMode : ITriggerAction
    {
        private readonly string[] IfritModes = { "先冲锋再读条", "先读条再冲锋", "读条-冲锋-读条" };

        public string DisplayName => "SMN/修改火神施法模式";

        public string Remark { get; set; }

        public int IfritMode { get; set; }

        public void Check()
        {
        }

        public bool Draw()
        {
            if (ImGui.BeginCombo("火神施法模式", IfritModes[IfritMode]))
            {
                for (int i = 0; i < IfritModes.Length; i++)
                {
                    if (ImGui.Selectable(IfritModes[i]))
                    {
                        IfritMode = i;
                    }
                }

                ImGui.EndCombo();
            }

            return true;
        }

        public bool Handle()
        {
            SMNSettings.Instance.IfritMode = IfritMode;
            return true;
        }
    }
}