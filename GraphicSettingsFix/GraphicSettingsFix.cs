using Assets.Scripts.Graphics;
using HarmonyLib;
using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace GraphicSettingsFix
{
    public delegate bool CallBackPtr(IntPtr hwnd, int lParam);
    public class GraphicSettingsFix : IMod
    {
        public string Name => "GraphicSettingsFix";

        public string Description => "无边框窗口化修复";

        public string Author => "Mo10";

        public string HomePage => "https://github.com/mo10";

        public string RequireAssembly => "Assembly-CSharp";


        private const uint SWP_SHOWWINDOW = 64u;
        private const int GWL_STYLE = -16;
        private const int WS_BORDER = 1;
        private const int WS_POPUP = 8388608;
        private const int WS_CAPTION = 12582912;
        public void DoPatching()
        {
            var harmony = new Harmony("com.github.mo10.graphicsettingsfix");

            var SetNoBorder = AccessTools.Method(typeof(Assets.Scripts.Graphics.GraphicSettings), "SetNoBorder");
            var SetNoBorderPreFix = AccessTools.Method(typeof(GraphicSettingsFix), "SetNoBorder");

            harmony.Patch(SetNoBorder, new HarmonyMethod(SetNoBorderPreFix));

            var SetHasBorder = AccessTools.Method(typeof(Assets.Scripts.Graphics.GraphicSettings), "SetHasBorder");
            var SetHasBorderPreFix = AccessTools.Method(typeof(GraphicSettingsFix), "SetHasBorder");

            harmony.Patch(SetHasBorder, new HarmonyMethod(SetHasBorderPreFix));
        }

        public static bool SetNoBorder(int width, int height)
        {
            int num = Screen.currentResolution.width - width;
            int num2 = Screen.currentResolution.height - height;
            if (num < 0)
            {
                num = 0;
            }
            if (num2 < 0)
            {
                num2 = 0;
            }
            SetWindowLong(FindWindow("Muse Dash", "UnityWndClass"), GWL_STYLE, WS_POPUP + WS_BORDER);
            SetWindowPos(FindWindow("Muse Dash", "UnityWndClass"), 0, num / 2, num2 / 2, width, height, SWP_SHOWWINDOW);
            return false;
        }
        public static bool SetHasBorder(int width, int height)
        {
            int num = Screen.currentResolution.width - width;
            int num2 = Screen.currentResolution.height - height;
            if (num < 0)
            {
                num = 0;
            }
            if (num2 < 0)
            {
                num2 = 0;
            }
            SetWindowLong(FindWindow("Muse Dash", "UnityWndClass"), GWL_STYLE, WS_CAPTION);
            SetWindowPos(FindWindow("Muse Dash", "UnityWndClass"), 0, num / 2, num2 / 2, width, height, SWP_SHOWWINDOW);
            return false;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        private static extern int EnumWindows(CallBackPtr callPtr, int lPar);
        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32")]
        private static extern int GetWindowText(IntPtr hwnd, StringBuilder lptrString, int nMaxCount);
        public static IntPtr FindWindow(string Title, string ClassName)
        {
            IntPtr TargetHwnd = IntPtr.Zero;
            EnumWindows(OnWindowEnum, 0);
            return TargetHwnd;

            bool OnWindowEnum(IntPtr hwnd, int lParam)
            {
                var lpClassname = new StringBuilder(512);
                var lpTitle = new StringBuilder(512);

                GetClassName(hwnd, lpClassname, lpClassname.Capacity);
                GetWindowText(hwnd, lpTitle, lpTitle.Capacity);

                if (lpClassname.ToString().Equals(ClassName, StringComparison.InvariantCultureIgnoreCase)
                    && lpTitle.ToString().Equals(Title, StringComparison.InvariantCultureIgnoreCase))
                {
                    TargetHwnd = hwnd;
                }
                return true;
            }
        }
    }
}
