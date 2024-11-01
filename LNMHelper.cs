using AEAssist;
using AEAssist.Extension;
using AEAssist.MemoryApi;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.STD;
using Lumina.Excel.GeneratedSheets;
using ContentType = Lumina.Excel.GeneratedSheets.ContentType;

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
        if (Core.Resolve<MemApiDuty>().DutyMembersNumber() == 24
            // 这个是迷宫挑战，这两个应该都可以用这个代替IsBoss
            || Core.Resolve<MemApiDuty>().DutyInfo.ContentType.Value == Svc.Data.Excel.GetSheet<ContentType>().GetRow(2))
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
            if (!task.Label.IsEmpty && task.Label.ToString().Equals(Svc.Data.GetExcelSheet<InstanceContentTextData>().GetRow(7).Text.RawString)) break;
        }
        
        if (totalNeeded == 0) return true;
        return totalNeeded - 1 == currentCount;
    }
#if DEBUG
    public static unsafe StdVector<EventHandlerObjective> GetTask()
    {
        var taskList = new StdVector<EventHandlerObjective>();
        try
        {
            taskList = EventFramework.Instance()->GetContentDirector()->Objectives;
        }
        catch (Exception e)
        {
            return taskList;
        }

        return taskList;
    }
#endif
}