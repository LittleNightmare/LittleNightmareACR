using Common.Helper;

namespace LittleNightmare.Summoner
{
    public class SMNSettings
    {
        public static SMNSettings Instance;
        private static string path;
        public static void Build(string settingPath)
        {
            path = Path.Combine(settingPath, "SMNSettings.json");
            if (!File.Exists(path))
            {
                Instance = new SMNSettings();
                Instance.Save();
                return;
            }

            try
            {
                Instance = JsonHelper.FromJson<SMNSettings>(File.ReadAllText(path));
            }
            catch (Exception e)
            {
                Instance = new();
                LogHelper.Error(e.ToString());
            }
        }

        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, JsonHelper.ToJson(this));
        }

        public Dictionary<string, object> StyleSetting = new();
        public bool AutoReset = true;

        public int 即刻咏唱模式 = 1;
        public bool qt自动火神冲 = false;

        /// <summary>
        /// 灼热之光优先于亚灵神释放
        /// </summary>
        public bool SearingLightFirst = false;

        /// <summary>
        /// 开场第二个GCD使用能量吸收
        /// </summary>
        public bool FastEnergyDrain = false;

        /// <summary>
        /// 在移动时，使用火神冲填充GCD，比毁4优先级高
        /// </summary>
        public bool SlideUseCrimonCyclone = true;
    }
}