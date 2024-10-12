using AEAssist;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using FFXIVClientStructs.STD;

namespace LittleNightmare;

public class LNMHelper
{
    public static bool IsLastTaskAE()
    {
        var duty = Core.Resolve<MemApiDuty>()?.GetSchedule();
        if (duty == null) return true;
        return duty.CountPoint == duty.NowPoint + 1;
    }
    public static unsafe bool IsLastTask()
    {
        var taskList = EventFramework.Instance()->GetContentDirector();
        if (taskList == null) return true;
        // var lastValidTaskIndex = -1;
        var finalTask = new EventHandlerObjective();
        for (var i = 0; i < taskList->Objectives.Count; i++)
        {
            var task = taskList->Objectives[i];
            if (!task.Enabled) break;
            finalTask = task;
        }
        if (finalTask.Equals(new EventHandlerObjective())) return true;
        // finalTask = taskList->Objectives[lastValidTaskIndex];
        return finalTask.CountNeeded - 1 == finalTask.CountCurrent;
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
