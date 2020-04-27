#if !UNITY_EDITOR
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace LiveWallpaperCore.Launcher {
    public class LauncherSource {

        private const string YOUR_GAME = @"GAME_NAME_HERE"; //This is automatically replaced with your game .exe when building
        private const string PROGMAN = "Progman";
        private const string WORKERW = "WorkerW";
        private const string SHELLDLL_DEF_VIEW = "SHELLDLL_DefView";

        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

        public static void Main(string[] args) {
            try {
                var workerW = WorkerWHandle;
                var arguments = string.Join(" ", args);

                arguments += string.Format("-wallpaper -parentHWND {0} delayed", workerW);

                var processStart = new ProcessStartInfo() {
                    FileName = YOUR_GAME,
                    Arguments = arguments,
                };

                Process.Start(processStart);
            }
            catch(Exception e) {
                using(var stream = new StreamWriter("wallpaper_crash.txt", true))
                    stream.Write(e);
            }
        }

        public static IntPtr WorkerWHandle {
            get {
                var progman = FindWindow(PROGMAN, null);
                var workerw = IntPtr.Zero;
                var result = IntPtr.Zero;
                var shellPtr = IntPtr.Zero;
                var windowEnumerator = new EnumWindowsProc((tophandle, topparamhandle) => {
                    shellPtr = FindWindowEx(tophandle, IntPtr.Zero, SHELLDLL_DEF_VIEW, IntPtr.Zero);

                    if(shellPtr != IntPtr.Zero)
                        workerw = FindWindowEx(IntPtr.Zero, tophandle, WORKERW, IntPtr.Zero);

                    return true;
                });

                SendMessageTimeout(progman, 0x052C, IntPtr.Zero, IntPtr.Zero, 0, 1000, out result);
                EnumWindows(windowEnumerator, IntPtr.Zero);

                if(workerw == IntPtr.Zero)
                    throw new Exception("Could not find WorkerW IntPtr");

                return workerw;
            }
        }

#region PInvoke
        public static void ThrowLastError() {
            var errorCode = Marshal.GetLastWin32Error();

            if(errorCode != 0)
                throw new Win32Exception(errorCode);
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, IntPtr windowTitle);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, uint flags, uint timeout, out IntPtr result);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
#endregion

    }
}
#endif