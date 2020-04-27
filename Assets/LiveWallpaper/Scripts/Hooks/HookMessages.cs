#if UNITY_STANDALONE_WIN
namespace LiveWallpaperCore {
    public enum MouseMessages {
        NoMessage = 0,
        Pressing = 1,

        MouseMove = 0x0200,
        MouseWheel = 0x020A,
        MouseHWheel = 0x020E,
        LeftButtonDown = 0x0201,
        LeftButtonUp = 0x0202,
        RightButtonDown = 0x0204,
        RightButtonUp = 0x0205,
        MiddleButtonDown = 0x0207,
        MiddleButtonUp = 0x0208,
    }

    public enum KeyboardMessages {
        NoMessage = 0,
        Pressing = 1,

        KeyDown = 0x0100,
        KeyUp = 0x0101,
        SystemKeyDown = 0x0104,
        SystemKeyUp = 0x0105,

    }
}
#endif