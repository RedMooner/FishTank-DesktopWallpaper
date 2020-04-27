#if UNITY_EDITOR_WIN && UNITY_STANDALONE_WIN
using System;
using LiveWallpaperCore;
using UnityEditor;
using UnityEngine;

public class EditorLiveWallpaper<T> : LiveWallpaper where T : EditorWindow {

    private T currentWindow;
    private Type windowType;

    public string Name { get { return windowType.Name + "_LiveWallpaper"; } }

    public T OpenWindow {
        get {
            if(currentWindow)
                return currentWindow;

            var all = Resources.FindObjectsOfTypeAll<T>();

            for(var i = 0; i < all.Length; i++)
                if(all[i] && all[i].name == Name)
                    return currentWindow = all[i];

            return null;
        }
        private set {
            currentWindow = value;
        }
    }

    public EditorLiveWallpaper() : base(IntPtr.Zero) { windowType = typeof(T); }

    public EditorLiveWallpaper(Type windowType) : base(IntPtr.Zero) { this.windowType = windowType; }

    private static IntPtr GetEditorWindowHandle(EditorWindow window) {
        if(!window)
            throw new NullReferenceException("Window can't be null");

        var i = 0;

        while(EditorWindow.focusedWindow != window) {
            window.Focus();

            if(i++ >= 1000)
                throw new Exception("Failed to focus window");
        }

        var handle = User32.GetActiveWindow();
        User32.ThrowLastError();
        return handle;
    }

    private T CreateWindow() {
        var window = ScriptableObject.CreateInstance(windowType) as T;

        window.name = Name;
        window.ShowPopup();

        OpenWindow = window;

        return window;
    }

    public override bool IsCurrentlyInWallpaperMode {
        get { return OpenWindow; }
        set { base.IsCurrentlyInWallpaperMode = value; }
    }

    public override void Enable() {
        WindowHandle = GetEditorWindowHandle(OpenWindow ?? CreateWindow());

        User32.SetParent(WindowHandle, WorkerWHandle);
        User32.ThrowLastError();
        Fullscreen.SetWindowFullscreen(WindowHandle);
    }

    public override void Disable() {
        if(OpenWindow)
            OpenWindow.Close();
        Wallpaper.RefreshWallpaper();
    }

    protected override void CallLiveWallpaperStateChanged() {
        if(EditorApplication.isPlayingOrWillChangePlaymode)
            base.CallLiveWallpaperStateChanged();
    }

}
#endif