using System.Numerics;
using System.Xml.Linq;
using CombatRoutine;
using CombatRoutine.Setting;
using Common.Define;
using Common.GUI;
using Common.Helper;
using Common.Language;
using ImGuiNET;


namespace LittleNightmare.Summoner;

public class Style
{
    private static float scale => SettingMgr.GetSetting<GeneralSettings>().OverlayScale;

    public static Vector2 SwitchButtonSize =>
        new Vector2(95, 40) * scale;

    public const ImGuiWindowFlags WindowFlag =
        ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoTitleBar |
        ImGuiWindowFlags.AlwaysAutoResize;

    //默认风格
    public static readonly Vector4 DefaultMainColor = new(161 / 255f, 47 / 255f, 114 / 255f, 0.8f);
    public static readonly float DefaultQtWindowBgAlpha = 0.2f;
    public static readonly int DefaultQtLineCount = 2;

    //保存style设置的地方
    public static SMNSettings Settings => SMNSettings.Instance; //改成自己的设置文件

    public static Dictionary<string, object> StyleSetting => Settings.StyleSetting; //在设置的cs文件中加上字典字段
    // public Dictionary<string, object> StyleSetting = new();
    // 在setting文件里加上这个


    /// 主颜色
    public static Vector4 MainColor
    {
        get
        {
            StyleSetting.TryAdd("MainColor", DefaultMainColor);
            return (Vector4)StyleSetting["MainColor"];
        }
        set
        {
            if (StyleSetting.ContainsKey("MainColor"))
                StyleSetting["MainColor"] = value;
            else
                StyleSetting.TryAdd("MainColor", value);
        }
    }

    /// Qt窗口背景透明度
    public static float QtWindowBgAlpha
    {
        get
        {
            StyleSetting.TryAdd("QtWindowBgAlpha", DefaultQtWindowBgAlpha);
            return Convert.ToSingle(StyleSetting["QtWindowBgAlpha"]);
        }
        set
        {
            if (StyleSetting.ContainsKey("QtWindowBgAlpha"))
                StyleSetting["QtWindowBgAlpha"] = value;
            else
                StyleSetting.TryAdd("QtWindowBgAlpha", value);
        }
    }

    ///QT按钮一行有几个
    public static int QtLineCount
    {
        get
        {
            StyleSetting.TryAdd("QtLineCount", DefaultQtLineCount);
            return Convert.ToInt32(StyleSetting["QtLineCount"]);
        }
        set
        {
            if (StyleSetting.ContainsKey("QtLineCount"))
                StyleSetting["QtLineCount"] = value;
            else
                StyleSetting.TryAdd("QtLineCount", value);
        }
    }


    //
    public static Vector4 ColorFalse => new(25 / 255f, 25 / 255f, 25 / 255f, MainColor.W);
    public static Vector4 ColorDark => new(47 / 255f, 47 / 255f, 47 / 255f, MainColor.W);
    public static Vector4 ColorDark2 => new(81 / 255f, 81 / 255f, 81 / 255f, MainColor.W);

    public static Vector4 ColorHovered => new((MainColor.X * 255 + 20) / 255f, (MainColor.Y * 255 + 20) / 255f,
        (MainColor.Z * 255 + 20) / 255f, MainColor.W);

    public static Vector4 ColorActive => new((MainColor.X * 255 + 50) / 255f, (MainColor.Y * 255 + 50) / 255f,
        (MainColor.Z * 255 + 50) / 255f, MainColor.W);

    private static bool smallWindow;

    /// <summary>
    /// 初始化主窗口风格
    /// </summary>
    public static void SetMainStyle()
    {
        ImGui.PushStyleColor(ImGuiCol.Border, new Vector4(0, 0, 0, 0));
        ImGui.PushStyleColor(ImGuiCol.FrameBg, ColorDark);
        ImGui.PushStyleColor(ImGuiCol.ResizeGrip, ColorFalse);
        ImGui.PushStyleColor(ImGuiCol.ResizeGripActive, MainColor);
        ImGui.PushStyleColor(ImGuiCol.ResizeGripHovered, MainColor);
        ImGui.PushStyleColor(ImGuiCol.CheckMark, ColorActive);
        ImGui.PushStyleColor(ImGuiCol.SliderGrab, MainColor);
        ImGui.PushStyleColor(ImGuiCol.SliderGrabActive, ColorActive);
        ImGui.PushStyleColor(ImGuiCol.Button, ColorDark2);
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, ColorActive);
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, ColorHovered);
        ImGui.PushStyleColor(ImGuiCol.Header, MainColor);
        ImGui.PushStyleColor(ImGuiCol.HeaderActive, ColorActive);
        ImGui.PushStyleColor(ImGuiCol.HeaderHovered, ColorHovered);
        ImGui.PushStyleColor(ImGuiCol.Tab, ColorDark);
        ImGui.PushStyleColor(ImGuiCol.TabActive, MainColor);
        ImGui.PushStyleColor(ImGuiCol.TabHovered, ColorHovered);
    }

    /// <summary>
    /// 注销主窗口风格，绝对不能少
    /// </summary>
    public static void EndMainStyle()
    {
        ImGui.PopStyleColor(17);
    }


    /// <summary>
    /// 初始化QT窗口风格
    /// </summary>
    public static void SetQtStyle()
    {
        ImGui.SetNextWindowSize(new Vector2(300, 450), ImGuiCond.FirstUseEver);
        ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 4);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(8, 8));
        ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(6, 6));
        ImGui.PushStyleColor(ImGuiCol.WindowBg, new Vector4(0, 0, 0, QtWindowBgAlpha));
    }

    /// <summary>
    /// 注销QT窗口风格，绝对不能少
    /// </summary>
    public static void EndQtStyle()
    {
        ImGui.PopStyleVar(3);
        ImGui.PopStyleColor(1);
    }


    /// <summary>
    /// Qt窗口类
    /// </summary>
    public class QtWindowClass
    {
        /// 用于储存所有qt控件的字典
        private Dictionary<string, QtControl> QtDict = new();

        /// 动态按顺序储存qt名称的list，用于排序显示qt
        private static List<string> QtNameList
        {
            get
            {
                StyleSetting.TryAdd("QtNameList", new List<string>());
                return (List<string>)StyleSetting["QtNameList"];
            }
            set
            {
                if (!StyleSetting.ContainsKey("QtNameList"))
                    StyleSetting.TryAdd("QtNameList", new List<string>());
                StyleSetting["QtNameList"] = value;
            }
        }
        
        /// 隐藏的qt列表
        private static List<string> QtUnVisibleList
        {
            get
            {
                StyleSetting.TryAdd("QtUnVisibleList", new List<string>());
                return (List<string>)StyleSetting["QtUnVisibleList"];
            }
            set
            {
                if (!StyleSetting.ContainsKey("QtUnVisibleList"))
                    StyleSetting.TryAdd("QtUnVisibleList", new List<string>());
                StyleSetting["QtUnVisibleList"] = value;
            }
        }

        public class QtControl
        {
            public readonly string Name;
            private bool _qtValue;

            public bool QtValue
            {
                get => GetPlayerOptions();
                set
                {
                    _qtValue = value;
                    SetPlayerOptions();
                }
            }
            public bool QtValueDefault;
            public string ToolTip = "";

            public QtControl(string name, bool qtValueDefault)
            {
                Name = name;
                QtValueDefault = qtValueDefault;
                Reset();
               
            }


            public void SetPlayerOptions()
            {
                var name = Name;
                if (Language.Instance.Type2Config[LanguageType.EN].KV.TryGetValue(Name, out var enName))
                {
                    name = enName;
                }
                
                // 反射拿到PlayerOptions的字段，然后赋值
                var field = typeof(PlayerOptions).GetField(name);
                field?.SetValue(PlayerOptions.Instance, _qtValue);

            }

            public bool GetPlayerOptions()
            {
                var name = Name;
                if (Language.Instance.Type2Config[LanguageType.EN].KV.TryGetValue(Name, out var enName))
                {
                    name = enName;
                }
                // 反射拿到PlayerOptions的字段，然后获取值
                var field = typeof(PlayerOptions).GetField(name);
                if (field != null)
                {
                    _qtValue = (bool)field.GetValue(PlayerOptions.Instance);
                }
                return _qtValue;
            }

            ///重置qt状态
            public void Reset()
            {
                QtValue = QtValueDefault;
            }
        }

        /// <summary>
        /// 添加新的qt控件
        /// </summary>
        /// <param name="name">qt的名称</param>
        /// <param name="qtValueDefault">qt的bool默认值</param>
        public void AddQt(string name, bool qtValueDefault)
        {
            name = name.Loc();
            if (QtDict.ContainsKey(name)) return;
            var qt = new QtControl(name, qtValueDefault);
            QtDict.Add(name, qt);
            if (!QtNameList.Contains(name))
            {
                QtNameList.Add(name);
            }
        }


        /// <summary>
        /// 添加新的qt控件
        /// </summary>
        /// <param name="name">qt的名称</param>
        /// <param name="qtValueDefault">qt的bool默认值</param>
        /// <param name="toolTip">鼠标悬停提示</param>
        public void AddQt(string name, bool qtValueDefault, string toolTip)
        {
            name = name.Loc();
            if (QtDict.ContainsKey(name)) return;
            var qt = new QtControl(name, qtValueDefault);
            QtDict.Add(name, qt);
            QtDict[name].ToolTip = toolTip;
            if (!QtNameList.Contains(name))
            {
                QtNameList.Add(name);
            }
        }


        // /// <summary>
        // /// 添加新的技能不位移qt
        // /// </summary>
        // /// <param name="name">qt的名称</param>
        // public void AddsQt_NoActionMove(string name)
        // {
        //     name = name.Loc();
        //     if (!AI.Instance.Hack) return;
        //     if (QtDict.ContainsKey(name)) return;
        //     var qt = new QtControl(name, AI.Instance.NoActionMove);
        //     QtDict.Add(name, qt);
        //     if (!QtNameList.Contains(name))
        //     {
        //         QtNameList.Add(name);
        //     }
        // }
        //
        // /// <summary>
        // /// 添加新的技能不位移qt
        // /// </summary>
        // /// <param name="name">qt的名称</param>
        // /// <param name="toolTip">鼠标悬停提示</param>
        // public void AddsQt_NoActionMove(string name,string toolTip)
        // {
        //     name = name.Loc();
        //     if (!AI.Instance.Hack) return;
        //     if (QtDict.ContainsKey(name)) return;
        //     var qt = new QtControl(name, AI.Instance.NoActionMove);
        //     QtDict.Add(name, qt);
        //     QtDict[name].ToolTip = toolTip;
        //     if (!QtNameList.Contains(name))
        //     {
        //         QtNameList.Add(name);
        //     }
        // }

        /// 获取指定名称qt的bool值
        public bool GetQt(string qtName)
        {
            qtName = qtName.Loc();
            if (!QtDict.ContainsKey(qtName))
                return false;
            return QtDict[qtName].QtValue;
        }

        /// 设置指定qt的值
        /// <returns>成功返回true，否则返回false</returns>
        public bool SetQt(string qtName, bool qtValue)
        {
            qtName = qtName.Loc();
            if (!QtDict.TryGetValue(qtName, out QtControl qt))
                return false;
            qt.QtValue =  qtValue;
            return true;
        }

        /// 反转指定qt的值
        /// <returns>成功返回true，否则返回false</returns>
        public bool ReverseQt(string qtName)
        {
            qtName = qtName.Loc();
            if (!QtDict.TryGetValue(qtName, out QtControl qt))
                return false;
            qt.QtValue = !qt.QtValue;
            return true;
        }

        /// 重置所有qt为默认值
        public void Reset()
        {
            foreach (var qt in QtDict.Select(_qt => _qt.Value))
            {
                qt.Reset();
                LogHelper.Info($"重置所有qt为默认值");
            }
        }

        /// 给指定qt设置新的默认值
        public void NewDefault(string qtName, bool newDefault)
        {
            qtName = qtName.Loc();
            if (!QtDict.TryGetValue(qtName, out QtControl qt))
                return;
            qt.QtValueDefault = newDefault;
            LogHelper.Info($"改变qt \"{qt.Name}\" 默认值为 {qt.QtValueDefault}");
        }

        /// 将当前所有Qt状态记录为新的默认值
        public void SetDefaultFromNow()
        {
            foreach (var qt in QtDict.Select(_qt => _qt.Value))
                if (qt.QtValueDefault != qt.QtValue)
                {
                    qt.QtValueDefault = qt.QtValue;
                    LogHelper.Info($"改变qt \"{qt.Name}\" 默认值为 {qt.QtValueDefault}");
                }
        }

        /// 返回包含当前所有qt名字的数组
        public string[] GetQtArray()
        {
            return QtDict.Keys.ToArray();
        }

        /// 画一个新的Qt窗口
        public void DrawQtWindow()
        {
            SetQtStyle();
            ImGui.Begin("##Qt_Window", WindowFlag);

            int i = 0;

            foreach (var qtName in QtNameList)
            {
                if (!QtDict.ContainsKey(qtName)) continue;
                if (QtUnVisibleList.Contains(qtName)) continue;
                var qt = QtDict[qtName];
                if (SwitchButton(qtName, ref qt))
                    LogHelper.Info($"改变qt \"{qt.Name}\" 状态为 {qt.QtValue}");

                if (qt.ToolTip != "")
                    ImGuiHelper.SetHoverTooltip(qt.ToolTip);
                if (i % QtLineCount != QtLineCount - 1) ImGui.SameLine();
                i++;
            }

            var row = Math.Ceiling((float)i / QtLineCount);
            var width = (QtLineCount - 1) * 6 + 16 + SwitchButtonSize.X * QtLineCount;
            var height = (row - 1) * 6 + 16 + SwitchButtonSize.Y * row;
            ImGui.SetWindowSize(new Vector2(width, (int)height));

            ImGui.End();
            EndQtStyle();
        }

        /// 用于draw一个更改qt排序显示等设置的视图
        public void QtSettingView()
        {
            ImGui.TextDisabled("   *左键拖动改变qt顺序，右键点击qt隐藏");
            for (var i = 0; i < QtNameList.Count; i++)
            {
                var item = QtNameList[i];
                var visible = !QtUnVisibleList.Contains(item) ? "显示" : "隐藏";

                if (visible == "隐藏")
                    ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0.6f, 0, 0, 1));

                ImGui.Selectable($"   {visible}        {item}");

                if (visible == "隐藏")
                    ImGui.PopStyleColor(1);

                //排序        
                if (ImGui.IsItemActive() && !ImGui.IsItemHovered())
                {
                    var n_next = i + (ImGui.GetMouseDragDelta(0).Y < 0f ? -1 : 1);
                    if (n_next < 0 || n_next >= QtNameList.Count) continue;
                    QtNameList[i] = QtNameList[n_next];
                    QtNameList[n_next] = item;
                    ImGui.ResetMouseDragDelta();
                }

                //右键
                if (ImGuiHelper.IsRightMouseClicked())
                {
                    if (!QtUnVisibleList.Contains(item))
                        QtUnVisibleList.Add(item);
                    else
                        QtUnVisibleList.Remove(item); 
                }
            }
        }
        public void CardView(List<Jobs>joblist)
        {
            for (var i = 0; i < joblist.Count; i++)
            {
                var item = joblist[i];
                ImGui.Selectable($"{JobHelper.GetTranslation(item)}");
                
                //排序        
                if (ImGui.IsItemActive() && !ImGui.IsItemHovered())
                {
                    var n_next = i + (ImGui.GetMouseDragDelta(0).Y < 0f ? -1 : 1);
                    if (n_next < 0 || n_next >= joblist.Count) continue;
                    joblist[i] = joblist[n_next];
                    joblist[n_next] = item;
                    ImGui.ResetMouseDragDelta();
                }

            }
        }
    }


    ///风格设置控件
    public static void ChangeStyleView()
    {
        //主颜色和透明度
        var colorV3 = new Vector3(MainColor.X, MainColor.Y, MainColor.Z);
        var alpha = MainColor.W;
        if (ImGui.ColorEdit3("主颜色".Loc(), ref colorV3))
        {
            MainColor = new Vector4(colorV3.X, colorV3.Y, colorV3.Z, alpha);
        }

        if (ImGui.SliderFloat("主透明度".Loc(), ref alpha, 0.2f, 1f, "%.1f"))
        {
            MainColor = MainColor with { W = alpha };
        }

        //QT透明度
        var qtBackGroundAlpha = QtWindowBgAlpha;
        if (ImGui.SliderFloat("Qt背景透明度".Loc(), ref qtBackGroundAlpha, 0f, 1f, "%.1f"))
        {
            QtWindowBgAlpha = qtBackGroundAlpha;
        }

        //QT按钮一行个数
        var input = QtLineCount;
        if (ImGui.InputInt("Qt按钮每行个数".Loc(), ref input))
        {
            if (input < 1)
                QtLineCount = 1;
            else
                QtLineCount = input;
        }

        //重置按钮
        if (ImGui.Button("重置"))
        {
            MainColor = DefaultMainColor;
            QtWindowBgAlpha = DefaultQtWindowBgAlpha;
            QtLineCount = DefaultQtLineCount;
        }
    }


    /// 重新包装的开关按钮控件，一般用于QT按钮
    public static bool SwitchButton(string label, ref QtWindowClass.QtControl qt)
    {
        var size = SwitchButtonSize;
        var ret = false;

        if (qt.QtValue)
        {
            ImGui.PushStyleColor(ImGuiCol.Button, MainColor);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, MainColor);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, MainColor);
            if (ImGui.Button(label, size))
            {
                ret = true;
                qt.QtValue = !qt.QtValue;
            }

            ImGui.PopStyleColor(3);
        }
        else
        {
            ImGui.PushStyleColor(ImGuiCol.Button, ColorFalse);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, ColorFalse);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, ColorFalse);
            if (ImGui.Button(label, size))
            {
                ret = true;
                qt.QtValue = !qt.QtValue;
            }

            ImGui.PopStyleColor(3);
        }

        return ret;
    }

    /// <summary>
    /// 用于开关自动输出的控件组合
    /// </summary>
    /// <param name="buttonValue">主开关</param>
    /// <param name="stopButton">传入控制停手的变量</param>
    /// <returns>1：摁下主按钮 2：摁下缩放按钮</returns>
    public static int MainControlView(ref bool buttonValue, ref bool stopButton)
    {
        var label = stopButton ? "停手" : "启动";
        var size = new Vector2(100, 45);
        var ret = 0;

        if (buttonValue)
        {
            if (!stopButton)
            {
                ImGui.PushStyleColor(ImGuiCol.Border, new Vector4(1, 1, 1, 0.5f));
                ImGui.PushStyleColor(ImGuiCol.Button, MainColor);
                ImGui.PushStyleColor(ImGuiCol.ButtonActive, ColorActive);
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, ColorHovered);
            }
            else
            {
                ImGui.PushStyleColor(ImGuiCol.Border, ColorHovered with { W = 1f });
                ImGui.PushStyleColor(ImGuiCol.Text, ColorActive with { W = 1f });
                ImGui.PushStyleColor(ImGuiCol.Button, ColorFalse);
                ImGui.PushStyleColor(ImGuiCol.ButtonActive, ColorFalse);
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, ColorFalse);
            }

            //启动按钮
            if (ImGui.Button(label, size))
            {
                ret = 1;
                if (!stopButton)
                {
                    buttonValue = !buttonValue;
                }
                else
                {
                    stopButton = !stopButton;
                    ImGui.PopStyleColor(1);
                }
            }

            if (stopButton) ImGui.PopStyleColor(1);
        }
        else
        {
            ImGui.PushStyleColor(ImGuiCol.Border, new Vector4(1, 1, 1, 0.5f));
            ImGui.PushStyleColor(ImGuiCol.Button, ColorFalse);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, ColorFalse);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, ColorFalse);
            //启动按钮
            if (ImGui.Button(label, size))
            {
                ret = 1;
                buttonValue = !buttonValue;
            }

            //启动按钮开启状态下可切换stop
            if (ImGuiHelper.IsRightMouseClicked() && buttonValue)
            {
                stopButton = !stopButton;
            }
        }

        ImGui.PopStyleColor(4);
        //如果启动按钮没开，stop永远为false
        if (!buttonValue) stopButton = false;
        //启动按钮开启状态下可切换stop
        if (ImGuiHelper.IsRightMouseClicked() && buttonValue)
        {
            stopButton = !stopButton;
        }

        //缩放按钮
        ImGui.SameLine();

        var label_text = smallWindow ? "▽" : "△";
        var _color = new Vector4(0, 0, 0, 0);
        ImGui.PushStyleColor(ImGuiCol.Button, _color);
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, _color);
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(1, 1, 1, 0.3f));
        ImGui.PushStyleColor(ImGuiCol.Border, new Vector4(1, 1, 1, 0.1f));

        if (ImGui.Button(label_text, new Vector2(30, 20)))
        {
            ret = 2;
            smallWindow = !smallWindow;
        }

        if (smallWindow)
        {
            ImGui.SetWindowSize(new Vector2(170, 80) * scale);
        }
        else
        {
            ImGui.SetWindowSize(new Vector2(320, 400) * scale);
        }

        //保存按钮
        ImGui.SameLine(0, 25);
        ImGui.BeginChild("##保存设置".Loc(), new Vector2(-1, 50));


        if (ImGui.Button("保存设置"))
            Settings.Save();

        ImGui.TextDisabled("*右键\"启动\"可停手");

        ImGui.PopStyleColor(4);
        ImGui.EndChild();

        return ret;
    }
}