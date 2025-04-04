﻿using AEAssist.CombatRoutine.Trigger;
using ImGuiNET;

namespace LittleNightmare.Summoner.Triggers
{
    public class SMNTriggerActionAdjustACRSimpleSettings : ITriggerAction
    {
        public string DisplayName => "SMN/修改ACR简单设置";

        public string Remark { get; set; }

        public int SelectedIndex; // 当前选中的设置项索引
        public bool CurrentValue; // 当前选中的设置项的值

        private readonly string[] _settingNames = 
       [
            "目标圈内移动时使用火神冲",
            "优先毁三填充最后的GCD窗口",
            "濒死检查",
            "阻止双插溃烂爆发",
            "智能AOE目标",
            "最终爆发时速卸三神",
            "调整火神施法模式",
            "自动减伤",
            "阻止亚灵神前召唤三神",
            "战斗结束后自动重置QT",
            "自动停手",
        ];


        public bool Draw()
        {
            ImGui.Text("将");
            ImGui.SameLine();
            // 下拉列表用于选择设置项
            ImGui.Combo("设置状态为：", ref SelectedIndex, _settingNames, _settingNames.Length);
            ImGui.SameLine();
            // 复选框用于设置选定设置项的值
            ImGui.Checkbox(CurrentValue ? "开启" : "关闭", ref CurrentValue);
            var currentStatus = GetSettingValue(_settingNames[SelectedIndex]) ? "开启" : "关闭";
            ImGui.Text($"当前状态：{currentStatus}");

            return true;
        }

        public bool Handle()
        {
            SetSettingValue(_settingNames[SelectedIndex], CurrentValue);
            return true;
        }

        private void SetSettingValue(string settingName, bool value)
        {
            // 根据settingName设置SMNSettings.Instance中相应的值
            switch (settingName)
            {
                case "目标圈内移动时使用火神冲":
                    SMNSettings.Instance.SlideUseCrimonCyclone = value;
                    break;
                case "优先毁三填充最后的GCD窗口":
                    SMNSettings.Instance.UseRuinIIIFirst = value;
                    break;
                case "濒死检查":
                    SMNSettings.Instance.TTKControl = value;
                    break;
                case "阻止双插溃烂爆发":
                    SMNSettings.Instance.PreventDoubleFester = value;
                    break;
                case "智能AOE目标":
                    SMNSettings.Instance.SmartAoETarget = value;
                    break;
                case "最终爆发时速卸三神":
                    SMNSettings.Instance.FastPassSummon = value;
                    break;
                case "调整火神施法模式":
                    SMNSettings.Instance.ModifyIfritMode = value;
                    break;
                case "自动减伤":
                    SMNSettings.Instance.AutoReduceDamage = value;
                    break;
                case "阻止亚灵神前召唤三神":
                    SMNSettings.Instance.PreventSummonBeforeBahamut = value;
                    break;
                case "战斗结束后自动重置QT":
                    SMNSettings.Instance.AutoResetQt = value;
                    break;
                case "自动停手":
                    SMNSettings.Instance.AutoStopForSpecialBuff = value;
                    break;
                default:
                    throw new ArgumentException("Invalid setting name");
            }
        }
        public bool GetSettingValue(string settingName)
        {
            // 根据settingName获取SMNSettings.Instance中相应的值
            switch (settingName)
            {
                case "目标圈内移动时使用火神冲":
                    return SMNSettings.Instance.SlideUseCrimonCyclone;
                case "优先毁三填充最后的GCD窗口":
                    return SMNSettings.Instance.UseRuinIIIFirst;
                case "濒死检查":
                    return SMNSettings.Instance.TTKControl;
                case "阻止双插溃烂爆发":
                    return SMNSettings.Instance.PreventDoubleFester;
                case "智能AOE目标":
                    return SMNSettings.Instance.SmartAoETarget;
                case "最终爆发时速卸三神":
                    return SMNSettings.Instance.FastPassSummon;
                case "调整火神施法模式":
                    return SMNSettings.Instance.ModifyIfritMode;
                case "自动减伤":
                    return SMNSettings.Instance.AutoReduceDamage;
                case "阻止亚灵神前召唤三神":
                    return SMNSettings.Instance.PreventSummonBeforeBahamut;
                case "战斗结束后自动重置QT":
                    return SMNSettings.Instance.AutoResetQt;
                case "自动停手":
                    return SMNSettings.Instance.AutoStopForSpecialBuff;
                default:
                    throw new ArgumentException("Invalid setting name");
            }
        }
    }
}