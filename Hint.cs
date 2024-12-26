namespace LittleNightmare;

public class Hint
{
    public string Content;
    public bool ShowInChat;
    public bool UseTTS;
    public bool PlaySound;
    public bool ShowToast2;

    public int Toast2Style;
    public int Toast2TimeInMs;


    public Hint(string content,
        bool showInChat = true, bool useTTS = false,
        bool playSound = false, bool showToast2 = true,
        int toast2Style = 1, int toast2TimeInMs = 3000)
    {
        Content = content;
        ShowInChat = showInChat;
        UseTTS = useTTS;
        PlaySound = playSound;
        ShowToast2 = showToast2;

        Toast2Style = toast2Style;
        Toast2TimeInMs = toast2TimeInMs;
    }
}