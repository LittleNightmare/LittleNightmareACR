using CombatRoutine.TriggerModel;
using Common;
using Common.GUI;
using Common.Language;
using ImGuiNET;

namespace LittleNightmare.Summoner.Triggers
{
    public class SMNTriggersActionAttunementCheck : ITriggerCond
    {
        public int Attunement;

        public string DisplayName => "SMN/LittleNightmare/检测三神中已使用次数".Loc();

        public string Remark { get; set; }

        public bool Draw()
        {
            ImGui.TextDisabled("我建议用 量谱条件，这个你需要组合 检测在场召唤兽类型 才稳\n另外，这个也不支持检测为4\nBy 动不了源码，阴暗的嘀嘀咕咕的小鬼");
            if (ImGui.InputInt("次数".Loc(), ref Attunement))
            {
                Attunement = Math.Clamp(Attunement, 0, 4);
            }

            return true;
        }

        public bool Handle(ITriggerCondParams condParamas)
        {
            if (Core.Get<IMemApiSummoner>().ActivePetType == ActivePetType.Titan || Core.Get<IMemApiSummoner>().ActivePetType == ActivePetType.Garuda)
            {
                return Attunement == 4 - Core.Get<IMemApiSummoner>().ElementalAttunement;
            }

            if (Core.Get<IMemApiSummoner>().ActivePetType == ActivePetType.Ifrit)
            {
                return Attunement == 2 - Core.Get<IMemApiSummoner>().ElementalAttunement;
            }

            return false;
        }
    }
}