using FFXIVClientStructs.FFXIV.Client.Game.Event;
using FFXIVClientStructs.STD;

namespace LittleNightmare;

public class LNMHelper
{
    public static unsafe bool IsLastTask()
    {
        var taskList = new StdVector<EventHandlerObjective>();
        try
        {
            taskList = EventFramework.Instance()->GetContentDirector()->Objectives;
        }
        catch (Exception e)
        {
            return true;
        }

        if (taskList.Count == 0) return true;
        if (taskList.All(x => !x.Enabled)) return true;
        var lastIndex = -1;
        for (var i = 0; i < taskList.Count; i++)
        {
            if (taskList[i].Enabled)
            {
                lastIndex = i;
            }
        }

        if (lastIndex == -1) return true;
        var finalTask = taskList[lastIndex];
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