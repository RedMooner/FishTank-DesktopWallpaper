#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace LiveWallpaperCore {

    [StructLayout(LayoutKind.Sequential)]
    public struct MousePosition {
        public int x;
        public int y;

        public override string ToString() {
            return string.Format("{0}x{1}", x, y);
        }

        public static implicit operator Vector2(MousePosition other) { return new Vector2(other.x, other.y); }
        public static implicit operator MousePosition(Vector2 other) { return new MousePosition() { x = (int)other.x, y = (int)other.y }; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MouseData {
        public MousePosition position;
        public uint data;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardData {
        public HookKeyCode key;
        public ushort data;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;

        public override string ToString() {
            return key.ToString();
        }
    }

}
#endif