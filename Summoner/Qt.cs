namespace LittleNightmare.Summoner;

public static class Qt
{
    public static bool GetQt(string qtName)
    {
        return SummonerRotationEntry.JobViewWindow.GetQt(qtName);
    }

    public static bool ReverseQt(string qtName)
    {
        return SummonerRotationEntry.JobViewWindow.ReverseQt(qtName);
    }

    public static bool SetQt(string qtName, bool qtValue)
    {
        return SummonerRotationEntry.JobViewWindow.SetQt(qtName, qtValue);
    }

    public static void Reset()
    {
        SummonerRotationEntry.JobViewWindow.Reset();
    }

    public static void NewDefault(string qtName, bool newDefault)
    {
        SummonerRotationEntry.JobViewWindow.NewDefault(qtName, newDefault);
    }

    public static void SetDefaultFromNow()
    {
        SummonerRotationEntry.JobViewWindow.SetDefaultFromNow();
    }

    public static string[] GetQtArray()
    {
        return SummonerRotationEntry.JobViewWindow.GetQtArray();
    }

}