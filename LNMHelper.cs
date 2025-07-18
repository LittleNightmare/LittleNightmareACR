﻿using AEAssist;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.STD;
using Lumina.Excel.Sheets;

namespace LittleNightmare;

public class LNMHelper
{
    public static bool IsLastTask()
    {
        if (!Core.Resolve<MemApiDuty>().IsBoundByDuty()) return true;
        var target = Core.Me.GetCurrTarget();
        if (target == null) return false;
        var additionalCheck = target.IsBoss();
        // TODO: 多变迷宫也是四人本，所以用不了这个判断，要不判断CountPoint>=1？
        var dutyInfo = Core.Resolve<MemApiDuty>().DutyInfo;
        if (Core.Resolve<MemApiDuty>().DutyMembersNumber() == 24
            // 这个是迷宫挑战，这两个应该都可以用这个代替IsBoss
            // 2是迷宫挑战
            || dutyInfo is { ContentType.Value.RowId: 2}
            )
            additionalCheck = Core.Resolve<MemApiDuty>().InBossBattle;
        return IsLastDutyTask() && additionalCheck;
    }

    public static bool IsLastDutyTaskAE()
    {
        var duty = Core.Resolve<MemApiDuty>().GetSchedule();
        if (duty == null) return true;
        if (duty.CountPoint == 0) return true;
        return duty.CountPoint == duty.NowPoint;
    }

    public static unsafe bool IsLastDutyTask()
    {
        var taskList = EventFramework.Instance()->GetContentDirector();
        if (taskList == null) return true;

        var totalNeeded = 0;
        var currentCount = 0;

        foreach (var task in taskList->Objectives.Where(task => task.Enabled))
        {
            totalNeeded += task.CountNeeded;
            currentCount += task.CountCurrent;
            // 处理那种存在第一行任务进行度，第二行显示当前任务的情况
            var instanceContentTextData = ECHelper.Data.GetExcelSheet<InstanceContentTextData>()?.GetRow(7);
            if (instanceContentTextData != null && !task.Label.IsEmpty && task.Label.ToString().Equals(instanceContentTextData.Value.Text.ExtractText())) break;
        }

        if (totalNeeded == 0) return true;
        return totalNeeded - 1 == currentCount;
    }
}
