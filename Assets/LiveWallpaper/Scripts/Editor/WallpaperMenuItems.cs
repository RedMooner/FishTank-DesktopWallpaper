#if UNITY_EDITOR_WIN && UNITY_STANDALONE_WIN
using UnityEditor;

namespace LiveWallpaperCore {
    public static class WallpaperMenuItems {

        [MenuItem("Live Wallpaper/Refresh Wallpaper")]
        private static void RefreshWallpaper() {
            Wallpaper.RefreshWallpaper();
        }

        [MenuItem("Live Wallpaper/Toggle live wallpaper")]
        private static void ToggleWallpaper() {
            if(LiveWallpaper.Main.IsCurrentlyInWallpaperMode)
                LiveWallpaper.Main.Disable();
            else
                LiveWallpaper.Main.Enable();
        }

        [MenuItem("Live Wallpaper/Toggle live wallpaper", true)]
        private static bool SetMenuWallpaperEnabled() {
            Menu.SetChecked("Live Wallpaper/Toggle live wallpaper", LiveWallpaper.Main.IsCurrentlyInWallpaperMode);
            return true;
        }

    }
}
#endif