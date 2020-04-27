#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;

namespace LiveWallpaperCore {
    public sealed class MouseHook : HookBase {

        public OnMouseMessage onMouseMessage;
        public delegate void OnMouseMessage(MouseData data, MouseMessages message);

        public MouseHook(OnMouseMessage callback) {
            Intercept(WH_MOUSE_LL);
            onMouseMessage = callback;
        }

        protected override IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            if(nCode >= 0) {
                var data = (MouseData)Marshal.PtrToStructure(lParam, typeof(MouseData));
                var message = (MouseMessages)wParam;
                onMouseMessage(data, message);
            }
            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }
    }
}
#endif