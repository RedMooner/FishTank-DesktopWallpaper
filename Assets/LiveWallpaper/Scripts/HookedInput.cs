using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace LiveWallpaperCore {
    /// <summary>
    /// Option for getting keyboard and mouse inputs while <see cref="LiveWallpaper"/> is enabled, 
    /// because <see cref="Input"/> only works if the application has focus, which is not the case here.
    /// </summary>
    public static class HookedInput {

#if UNITY_STANDALONE_WIN
#pragma warning disable 0414 //Asigned but never used
        private static readonly MouseHook mouseHook = new MouseHook(OnMouseEvent);
        private static readonly KeyboardHook keyboardHook = new KeyboardHook(OnKeyboardEvent);
#pragma warning restore 0414

        private static readonly Dictionary<int, MouseMessages> mouseInputs = new Dictionary<int, MouseMessages>();
        private static readonly Dictionary<HookKeyCode, KeyboardMessages> keyboardInputs = new Dictionary<HookKeyCode, KeyboardMessages>();

        private static void OnMouseEvent(MouseData data, MouseMessages message) {
            var mouseID = -1;

            switch(message) {
                case MouseMessages.LeftButtonDown:
                case MouseMessages.LeftButtonUp:
                    mouseID = 0;
                    break;

                case MouseMessages.MiddleButtonDown:
                case MouseMessages.MiddleButtonUp:
                    mouseID = 2;
                    break;

                case MouseMessages.RightButtonDown:
                case MouseMessages.RightButtonUp:
                    mouseID = 1;
                    break;

                default:
                    return;
            }

            lock(mouseInputs)
                mouseInputs[mouseID] = message;
        }

        private static void OnKeyboardEvent(KeyboardData data, KeyboardMessages message) {
            lock(keyboardInputs) {
                var current = KeyboardMessages.NoMessage;

                switch(message) {
                    case KeyboardMessages.KeyDown:
                    case KeyboardMessages.SystemKeyDown:
                        if(keyboardInputs.TryGetValue(data.key, out current))
                            if(current == KeyboardMessages.Pressing && message == KeyboardMessages.KeyDown)
                                return;

                        keyboardInputs[data.key] = message;
                        break;

                    default:
                        keyboardInputs[data.key] = message;
                        break;
                }
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void ClearInputs() {
            var lastFrame = Time.frameCount;

            Camera.onPostRender += (cam) => {

                if(lastFrame == Time.frameCount)
                    return;
                lastFrame = Time.frameCount;

                Profiler.BeginSample("Hooked Input clear");

                var referenceMouseInputs = new Dictionary<int, MouseMessages>(mouseInputs);
                var referenceKeyboardInputs = new Dictionary<HookKeyCode, KeyboardMessages>(keyboardInputs);

                Profiler.BeginSample("Mouse inputs");
                foreach(var kvp in referenceMouseInputs)
                    switch(kvp.Value) {
                        case MouseMessages.LeftButtonDown:
                        case MouseMessages.MiddleButtonDown:
                        case MouseMessages.RightButtonDown:
                            mouseInputs[kvp.Key] = MouseMessages.Pressing;
                            break;

                        case MouseMessages.LeftButtonUp:
                        case MouseMessages.MiddleButtonUp:
                        case MouseMessages.RightButtonUp:
                            mouseInputs[kvp.Key] = MouseMessages.NoMessage;
                            break;
                    }
                Profiler.EndSample();

                Profiler.BeginSample("Keyboard inputs");
                foreach(var kvp in referenceKeyboardInputs)
                    switch(kvp.Value) {
                        case KeyboardMessages.KeyDown:
                        case KeyboardMessages.SystemKeyDown:
                            keyboardInputs[kvp.Key] = KeyboardMessages.Pressing;
                            break;

                        case KeyboardMessages.KeyUp:
                        case KeyboardMessages.SystemKeyUp:
                            keyboardInputs[kvp.Key] = KeyboardMessages.NoMessage;
                            break;
                    }
                Profiler.EndSample();

                Profiler.EndSample();
            };
        }
#endif

        /// <summary>
        /// <para>The current mouse position in pixel coordinates. (Read Only)</para>
        /// </summary>
        public static Vector3 MousePosition { get { return Input.mousePosition; } }

        /// <summary>
        /// <para>Returns true while the user holds down the key identified by the key <see cref="HookKeyCode"/> enum parameter.</para>
        /// </summary>
        /// <param name="key">The key to get</param>
        public static bool GetKey(HookKeyCode key) {
#if UNITY_STANDALONE_WIN
            var result = KeyboardMessages.NoMessage;

            if(!keyboardInputs.TryGetValue(key, out result))
                return false;

            return result == KeyboardMessages.Pressing;
#else
            return false;
#endif
        }

        /// <summary>
        /// <para>Returns true during the frame the user starts pressing down the key identified by the key <see cref="HookKeyCode"/> enum parameter.</para>
        /// </summary>
        /// <param name="key">The key to get</param>
        public static bool GetKeyDown(HookKeyCode key) {
#if UNITY_STANDALONE_WIN 
            var result = KeyboardMessages.NoMessage;

            if(!keyboardInputs.TryGetValue(key, out result))
                return false;

            return result == KeyboardMessages.KeyDown || result == KeyboardMessages.SystemKeyDown;
#else
            return false;
#endif
        }

        /// <summary>
        /// <para>Returns true during the frame the user releases the key identified by the key <see cref="HookKeyCode"/> enum parameter.</para>
        /// </summary>
        /// <param name="key">The key to get</param>
        public static bool GetKeyUp(HookKeyCode key) {
#if UNITY_STANDALONE_WIN
            var result = KeyboardMessages.NoMessage;

            if(!keyboardInputs.TryGetValue(key, out result))
                return false;

            return result == KeyboardMessages.KeyUp || result == KeyboardMessages.SystemKeyUp;
#else
            return false;
#endif
        }

        /// <summary>
        /// <para>Returns whether the given mouse button is held down.</para>
        /// </summary>
        /// <param name="button">Button number, 0 is LMB, 1 is RMB and 2 is MMB.</param>
        public static bool GetMouseButton(int button) {
#if UNITY_STANDALONE_WIN
            var result = MouseMessages.NoMessage;

            if(!mouseInputs.TryGetValue(button, out result))
                return false;

            return result == MouseMessages.Pressing;
#else
            return false;
#endif
        }

        /// <summary>
        /// <para>Returns true during the frame the user pressed the given mouse button.</para>
        /// </summary>
        /// <param name="button">Button number, 0 is LMB, 1 is RMB and 2 is MMB.</param>
        public static bool GetMouseButtonDown(int button) {
#if UNITY_STANDALONE_WIN
            var result = MouseMessages.NoMessage;

            if(!mouseInputs.TryGetValue(button, out result))
                return false;

            return result == MouseMessages.LeftButtonDown ||
                   result == MouseMessages.MiddleButtonDown ||
                   result == MouseMessages.RightButtonDown;
#else
            return false;
#endif
        }

        /// <summary>
        /// <para>Returns true during the frame the user releases the given mouse button.</para>
        /// </summary>
        /// <param name="button">Button number, 0 is LMB, 1 is RMB and 2 is MMB.</param>
        public static bool GetMouseButtonUp(int button) {
#if UNITY_STANDALONE_WIN
            var result = MouseMessages.NoMessage;

            if(!mouseInputs.TryGetValue(button, out result))
                return false;

            return result == MouseMessages.LeftButtonUp ||
                   result == MouseMessages.MiddleButtonUp ||
                   result == MouseMessages.RightButtonUp;

#else
            return false;
#endif
        }
    }
}