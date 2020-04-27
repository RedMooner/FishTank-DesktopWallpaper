using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

namespace LiveWallpaperCore {

    public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

    [Flags]
    public enum WindowStylesFlags : uint {
        WS_OVERLAPPED = 0x00000000,
        WS_POPUP = 0x80000000,
        WS_CHILD = 0x40000000,
        WS_MINIMIZE = 0x20000000,
        WS_VISIBLE = 0x10000000,
        WS_DISABLED = 0x08000000,
        WS_CLIPSIBLINGS = 0x04000000,
        WS_CLIPCHILDREN = 0x02000000,
        WS_MAXIMIZE = 0x01000000,
        WS_BORDER = 0x00800000,
        WS_DLGFRAME = 0x00400000,
        WS_VSCROLL = 0x00200000,
        WS_HSCROLL = 0x00100000,
        WS_SYSMENU = 0x00080000,
        WS_THICKFRAME = 0x00040000,
        WS_GROUP = 0x00020000,
        WS_TABSTOP = 0x00010000,

        WS_MINIMIZEBOX = 0x00020000,
        WS_MAXIMIZEBOX = 0x00010000,

        WS_CAPTION = WS_BORDER | WS_DLGFRAME,
        WS_TILED = WS_OVERLAPPED,
        WS_ICONIC = WS_MINIMIZE,
        WS_SIZEBOX = WS_THICKFRAME,
        WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
        WS_CHILDWINDOW = WS_CHILD,

        WS_EX_DLGMODALFRAME = 0x00000001,
        WS_EX_NOPARENTNOTIFY = 0x00000004,
        WS_EX_TOPMOST = 0x00000008,
        WS_EX_ACCEPTFILES = 0x00000010,
        WS_EX_TRANSPARENT = 0x00000020,

        WS_EX_MDICHILD = 0x00000040,
        WS_EX_TOOLWINDOW = 0x00000080,
        WS_EX_WINDOWEDGE = 0x00000100,
        WS_EX_CLIENTEDGE = 0x00000200,
        WS_EX_CONTEXTHELP = 0x00000400,

        WS_EX_RIGHT = 0x00001000,
        WS_EX_LEFT = 0x00000000,
        WS_EX_RTLREADING = 0x00002000,
        WS_EX_LTRREADING = 0x00000000,
        WS_EX_LEFTSCROLLBAR = 0x00004000,
        WS_EX_RIGHTSCROLLBAR = 0x00000000,

        WS_EX_CONTROLPARENT = 0x00010000,
        WS_EX_STATICEDGE = 0x00020000,
        WS_EX_APPWINDOW = 0x00040000,

        WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE),
        WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST),

        WS_EX_LAYERED = 0x00080000,

        WS_EX_NOINHERITLAYOUT = 0x00100000,
        WS_EX_LAYOUTRTL = 0x00400000,

        WS_EX_COMPOSITED = 0x02000000,
        WS_EX_NOACTIVATE = 0x08000000
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public static implicit operator Rectangle(Rect other) {
            return new Rectangle() {
                Left = (int)other.xMin,
                Right = (int)other.xMax,
                Bottom = (int)other.yMax,
                Top = (int)other.yMin
            };
        }

        public static implicit operator Rect(Rectangle other) {
            return new Rect() {
                xMin = other.Left,
                xMax = other.Right,
                yMax = other.Bottom,
                yMin = other.Top
            };
        }
    }

#if UNITY_STANDALONE_WIN
    /// <summary>
    /// Interop helper class.
    /// </summary>
    public static class User32 {

        public static void ThrowLastError() {
            var errorCode = Marshal.GetLastWin32Error();

            if(errorCode != 0)
                throw new Win32Exception(errorCode);
        }

        #region Get/Set window long
        [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
        private static extern WindowStylesFlags GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
        private static extern WindowStylesFlags GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, WindowStylesFlags dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, WindowStylesFlags dwNewLong);

        public static WindowStylesFlags GetWindowLongPtr(IntPtr hWnd, int nIndex) {
            if(IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return GetWindowLongPtr32(hWnd, nIndex);
        }

        public static void SetWindowLongPtr(IntPtr hWnd, int nIndex, WindowStylesFlags dwNewLong) {
            if(IntPtr.Size == 8)
                SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                SetWindowLong32(hWnd, nIndex, dwNewLong);
        }
        #endregion

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rectangle rectangle);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, IntPtr windowTitle);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint msg, IntPtr wParam, IntPtr lParam, uint flags, uint timeout, out IntPtr result);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetSystemMetrics(int smIndex);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(uint action, uint uParam, string vParam, uint winIni);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowThreadProcessId(IntPtr window, out int process);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static IntPtr GetActiveWindowSafe() {
            var windows = GetProcessWindows();

            for(var i = 0; i < windows.Length; i++) {
                SetForegroundWindow(windows[i]);

                if(GetActiveWindow() == windows[i])
                    return windows[i];
            }

            return IntPtr.Zero;
        }

        public static IntPtr[] GetProcessWindows() {
            using(var proc = Process.GetCurrentProcess())
                return GetProcessWindows(proc.Id);
        }

        public static IntPtr[] GetProcessWindows(int pid) {
            var results = new IntPtr[128];
            var count = 0;
            var processID = 0;

            EnumWindows((hWnd, lParam) => {
                GetWindowThreadProcessId(hWnd, out processID);

                if(processID == pid)
                    results[count++] = hWnd;

                return true;
            }, IntPtr.Zero);

            Array.Resize(ref results, count);
            return results;
        }

    }
#endif

}