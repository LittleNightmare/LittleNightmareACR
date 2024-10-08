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
        var finalTask = taskList[taskList.FindLastIndex(x => x.Enabled)];
        return finalTask.CountNeeded - 1 == finalTask.CountCurrent;
    }
}
