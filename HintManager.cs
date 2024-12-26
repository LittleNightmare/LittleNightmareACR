using AEAssist;
using AEAssist.Command;
using AEAssist.Helper;
using AEAssist.MemoryApi;

namespace LittleNightmare
{
    public class HintManager
    {
        public Dictionary<string, Hint> Hints;

        public HintManager()
        {
            Hints = new();
        }

        public HintManager(Dictionary<string, Hint> hints)
        {
            Hints = hints;
        }

        public void AddHint(string key, Hint hint)
        {
            Hints.TryAdd(key, hint);
        }

        public void RemoveHint(string key)
        {
            if (Hints.ContainsKey(key))
            {
                Hints.Remove(key);
            }
        }

        public void TriggerHint(string key, string? customContent = null, string? customChat = null, string? customTTS = null, string? customToast2 = null)
        {
            if (Hints.TryGetValue(key, out var hint))
            {
                var content = customContent ?? hint.Content;

                if (hint.ShowInChat)
                {
                    ShowInChat(customChat ?? content);
                }
                if (hint.UseTTS)
                {
                    PlayTTS(customTTS ?? content);
                }
                // if (hint.PlaySound)
                // {
                //     PlaySound();
                // }
                if (hint.ShowToast2)
                {
                    ShowToast(customToast2 ?? content, hint.Toast2Style, hint.Toast2TimeInMs);
                }
            }
        }

        private void ShowInChat(string message)
        {
            LogHelper.Print(message);
        }

        private void PlayTTS(string message)
        {
            ChatHelper.SendMessage($"/pdr tts {message}");
        }

        private void PlaySound()
        {
            // 实现提示音的逻辑
            throw new NotImplementedException();
        }

        private void ShowToast(string message, int s, int time)
        {
            Core.Resolve<MemApiChatMessage>().Toast2(message, s, time);
            
        }
    }


}
