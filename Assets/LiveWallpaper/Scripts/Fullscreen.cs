using System;
using System.Diagnostics;
using UnityEngine;

namespace LiveWallpaperCore {
    /// <summary>
    /// Helper class for making windows fullscreen and/or borderless.
    /// </summary>
    public static class Fullscreen {

        #region Constants
        private const int GWL_STYLE = -16; //Get window long
        private const int SM_CXVIRTUALSCREEN = 78; //System metrics
        private const int SM_CYVIRTUALSCREEN = 79; //System metrics
        private const uint SWP_SHOWWINDOW = 0x0040; //Set window pos

        private const WindowStylesFlags WSF_BORDER = WindowStylesFlags.WS_CAPTION | WindowStylesFlags.WS_THICKFRAME | WindowStylesFlags.WS_MINIMIZE | WindowStylesFlags.WS_MAXIMIZE | WindowStylesFlags.WS_SYSMENU;

#if UNITY_EDITOR
        private const int TOOLBARHEIGHT = 17;
#else
        private const int TOOLBARHEIGHT = 0;
#endif
        #endregion

        /// <summary>
        /// The full width of the screen, including all monitors.
        /// </summary>
        public static int VirtualScreenWidth {
            get {
#if UNITY_STANDALONE_WIN
                return User32.GetSystemMetrics(SM_CXVIRTUALSCREEN);
#else
                return Screen.width;
#endif
            }
        }

        /// <summary>
        /// The full height of the screen, including all monitors.
        /// </summary>
        public static int VirtualScreenHeight {
            get {
#if UNITY_STANDALONE_WIN
                return User32.GetSystemMetrics(SM_CYVIRTUALSCREEN);
#else
                return Screen.height;
#endif
            }
        }

        /// <summary>
        /// Get the style of a specified window.
        /// </summary>
        /// <param name="windowHandle">The handle to the window we'll get the style</param>
        /// <returns></returns>
        public static WindowStylesFlags GetWindowStyle(IntPtr windowHandle) {
#if UNITY_STANDALONE_WIN
            var currentstyle = User32.GetWindowLongPtr(windowHandle, GWL_STYLE);
            User32.ThrowLastError();
            return currentstyle;
#else
            return (WindowStylesFlags)0;
#endif
        }

        /// <summary>
        /// Set the border flags to a window. This is not guaranteed to be the same as before using <see cref="RemoveWindowBorders"/>.
        /// <para/>Use <see cref="GetWindowStyle"/> and <see cref="SetWindowStyle"/> if you need the exactly same style as before.
        /// </summary>
        /// <param name="windowHandle">The handle of the window that will be affected</param>
        [Conditional("UNITY_STANDALONE_WIN")]
        public static void RestoreWindowBorders(IntPtr windowHandle) {
#if UNITY_STANDALONE_WIN
            var newStyle = GetWindowStyle(windowHandle) | WSF_BORDER;
            SetWindowStyle(windowHandle, newStyle);
#endif
        }

        /// <summary>
        /// Remove all the <see cref="WindowStylesFlags"/> that makes a border on the window.
        /// </summary>
        /// <param name="windowHandle">The handle of the window that will be affected</param>
        [Conditional("UNITY_STANDALONE_WIN")]
        public static void RemoveWindowBorders(IntPtr windowHandle) {
#if UNITY_STANDALONE_WIN
            var newStyle = GetWindowStyle(windowHandle) & ~WSF_BORDER;
            SetWindowStyle(windowHandle, newStyle);
#endif
        }

        /// <summary>
        /// Set a window style to a window handle. Pretty auto explicative, isn't it?
        /// </summary>
        /// <param name="windowHandle">The handle of the window that will be affected</param>
        /// <param name="newStyle">The new window style</param>
        [Conditional("UNITY_STANDALONE_WIN")]
        public static void SetWindowStyle(IntPtr windowHandle, WindowStylesFlags newStyle) {
#if UNITY_STANDALONE_WIN
            User32.SetWindowLongPtr(windowHandle, GWL_STYLE, newStyle);
            User32.ThrowLastError();
#endif
        }

        /// <summary>
        /// Set the window to the full size of the screen.
        /// <para/>On multi-screens setups, the window will be resized to fit all the
        /// monitors, and it will have "invisible parts" if the screens are not aligned, for example, if
        /// one screen is placed on the top-left and other in the bottom-right, then the top-right
        /// and bottom-left would be invisible but they would still be rendered as part of the
        /// screen.
        /// </summary>
        /// <param name="windowHandle">The handle of the window that will be affected</param>
        [Conditional("UNITY_STANDALONE_WIN")]
        public static void SetWindowFullscreen(IntPtr windowHandle) {
#if UNITY_STANDALONE_WIN
            User32.SetWindowPos(windowHandle, IntPtr.Zero, 0, -TOOLBARHEIGHT, VirtualScreenWidth, VirtualScreenHeight + TOOLBARHEIGHT, SWP_SHOWWINDOW);
            User32.ThrowLastError();
#endif
        }

        /// <summary>
        /// Set the windows position based on a <see cref="Rect"/>.
        /// </summary>
        /// <param name="windowHandle">The handle of the window that will be affected</param>
        /// <param name="rect">The rect that will be set to the window</param>
        [Conditional("UNITY_STANDALONE_WIN")]
        public static void SetWindowPosition(IntPtr windowHandle, Rect rect) {
#if UNITY_STANDALONE_WIN
            User32.SetWindowPos(windowHandle, IntPtr.Zero, (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, SWP_SHOWWINDOW);
            User32.ThrowLastError();
#endif
        }

    }
}