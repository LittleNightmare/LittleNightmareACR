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

        public enum OpenerType
        {
            TheBalance,
            FastEnergyDrain,
        }

        /// <summary>
        /// 起手种类
        /// TheBalance: 是用TheBalance的最佳通用起手
        /// FastEnergyDrain: 是召唤巴哈后，直接灼热之光，然后插入能量吸收的起手
        /// </summary>
        public OpenerType SelectedOpener = OpenerType.TheBalance;

        /// <summary>
        /// 目标圈内，在移动时，使用火神冲填充GCD，比毁4优先级高
        /// </summary>
        public bool SlideUseCrimonCyclone = true;

        /// <summary>
        /// 在GCD填充时，如果不移动，能量吸收还没马上好，优先毁3填充，再是毁4
        /// </summary>
        public bool UseRuinIIIFirst = false;

        /// <summary>
        /// 在不移动时，优先使用火神GCD，而不是火神冲
        /// </summary>
        public bool RubyGCDFirst = false;
    }
}