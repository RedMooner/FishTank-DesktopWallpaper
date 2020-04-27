#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;

namespace LiveWallpaperCore {
    public sealed class KeyboardHook : HookBase {

        public OnKeyboardMessage onKeyboardMessage;
        public delegate void OnKeyboardMessage(KeyboardData data, KeyboardMessages message);

        public KeyboardHook(OnKeyboardMessage callback) {
            Intercept(WH_KEYBOARD_LL);
            onKeyboardMessage = callback;
        }

        protected override IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            if(nCode >= 0) {
                var data = (KeyboardData)Marshal.PtrToStructure(lParam, typeof(KeyboardData));
                var message = (KeyboardMessages)wParam;
                onKeyboardMessage(data, message);
            }
            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }
    }
}
#endif