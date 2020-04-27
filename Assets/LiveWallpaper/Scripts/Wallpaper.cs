using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace LiveWallpaperCore {
    /// <summary>
    /// Contains methods for getting and setting the static wallpaper.
    /// </summary>
    public static class Wallpaper {

        private const int MAX_WALLPAPER_PATH_LENGTH = 260;
        private const uint SPI_SETDESKWALLPAPER = 20; //System parameters info
        private const uint SPI_GETDESKWALLPAPER = 115; //System parameters info
        private const uint SPIF_UPDATEINIFILE = 0x01; //System parameters info flags
        private const uint SPIF_SENDWININICHANGE = 0x02; //System parameters info flags

        /// <summary>
        /// Get the path to the current wallpaper image.
        /// <para/>The file is not guaranteed to exist and the string might be null in some cases.
        /// </summary>
        public static string GetWallpaperPath() {
#if UNITY_STANDALONE_WIN
            var wallpaper = new string('\0', MAX_WALLPAPER_PATH_LENGTH);

            User32.SystemParametersInfo(SPI_GETDESKWALLPAPER, (uint)wallpaper.Length, wallpaper, 0);
            User32.ThrowLastError();

            return wallpaper.Substring(0, wallpaper.IndexOf('\0'));
#else
            return string.Empty;
#endif
        }

        /// <summary>
        /// Get a texture of the current wallpaper image.
        /// <para/>Returns a black texture if the wallpaper path is empty or invalid.
        /// <para/>This methods uses <see cref="File.ReadAllBytes"/>, which is pretty slow and may cause low framerates if called many times.
        /// Consider calling this only when you really need.
        /// </summary>
        /// <returns></returns>
        public static Texture2D GetWallpaperTexture() {
#if UNITY_STANDALONE_WIN
            var wallpaperPath = GetWallpaperPath();

            if(!File.Exists(wallpaperPath))
                return Texture2D.blackTexture;

            var bytes = File.ReadAllBytes(wallpaperPath);
            var texture = new Texture2D(0, 0, TextureFormat.ARGB32, false);

            texture.LoadImage(bytes, true);
            texture.name = wallpaperPath;

            return texture;
#else
            return Texture2D.blackTexture;
#endif
        }

        /// <summary>
        /// Set the wallpaper based on the passed filepath. Relative paths are allowed.
        /// </summary>
        /// <param name="filePath">The path to the image, the image format must be valid for a wallpaper.</param>
        [Conditional("UNITY_STANDALONE_WIN")]
        public static void SetWallpaper(string filePath) {
#if UNITY_STANDALONE_WIN
            if(!File.Exists(filePath))
                throw new FileNotFoundException("Image file not found", filePath);

            filePath = Path.GetFullPath(filePath);

            User32.SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filePath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            User32.ThrowLastError();
#endif
        }

        /// <summary>
        /// Set the wallpaper based on the passed texture.
        /// </summary>
        /// <param name="image">The texture to set as wallpaper, it will be saved to <see cref="Application.persistentDataPath"/> named Wallpaper.png.</param>
        [Conditional("UNITY_STANDALONE_WIN")]
        public static void SetWallpaper(Texture2D image) {
#if UNITY_STANDALONE_WIN
            if(!image)
                throw new ArgumentNullException("image", "Image is null");

            var path = Path.Combine(Application.persistentDataPath, "Wallpaper.png");
            var encoded = image.EncodeToPNG();

            path = Path.GetFullPath(path);
            File.WriteAllBytes(path, encoded);
            SetWallpaper(path);
#endif
        }

        /// <summary>
        /// Refreshes the current wallpaper image.
        /// <para/>The wallpaper automatically refreshed when you call <see cref="LiveWallpaper.Disable"/>, 
        /// but you may want to refresh it manually if you see that there is any bug causing the <see cref="LiveWallpaper"/> to freeze.
        /// </summary>
        [Conditional("UNITY_STANDALONE_WIN")]
        public static void RefreshWallpaper() {
            SetWallpaper(GetWallpaperPath());
        }

    }
}