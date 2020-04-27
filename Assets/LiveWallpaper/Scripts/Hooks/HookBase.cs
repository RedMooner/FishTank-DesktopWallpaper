#if UNITY_STANDALONE_WIN
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LiveWallpaperCore {
    public abstract class HookBase : IDisposable {

        protected const int WH_KEYBOARD_LL = 13;
        protected const int WH_MOUSE_LL = 14;

        protected IntPtr hookID = IntPtr.Zero;
        protected LowLevelMouseProc proc;
        protected delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private bool disposed;

        protected void Intercept(int idHook) { }

        protected abstract IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam);

        protected static IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam) {
            return IntPtr.Zero;
        }

        public void Dispose() {
            disposed = true;
        }

    }
}
#endif
