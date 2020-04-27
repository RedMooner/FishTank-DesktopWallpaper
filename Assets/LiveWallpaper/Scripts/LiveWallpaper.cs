using System;
using System.Diagnostics;
using LiveWallpaperCore;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

/// <summary>
/// Class used for setting windows as live wallpapers.
/// </summary>
public class LiveWallpaper {

    #region Constants
    private const string PROGMAN = "Progman";
    private const string WORKERW = "WorkerW";
    private const string SHELLDLL_DEF_VIEW = "SHELLDLL_DefView";
    private const string WALLPAPER_COMMANDLINE = "-wallpaper";
    private const uint SMTO_NORMAL = 0x0; //Send message timeout
    #endregion

#pragma warning disable 0414 //Asigned but never used
#pragma warning disable 0649 //Not asigned and never used
    private Rectangle positionBeforeLiveWallpaper;
    private WindowStylesFlags flagsBeforeLiveWallpaper;

    private bool m_isCurrentlyInWallpaperMode = Environment.CommandLine.Contains(WALLPAPER_COMMANDLINE);
    private static LiveWallpaper m_main;
    private static readonly bool m_startedByCommandLine = Environment.CommandLine.Contains(WALLPAPER_COMMANDLINE);
#pragma warning restore 0414
#pragma warning restore 0649

    /// <summary>
    /// The window handle associated with this <see cref="LiveWallpaper"/> instance.
    /// </summary>
    public IntPtr WindowHandle { get; protected set; }

    /// <summary>
    /// UnityAction that is triggered when the <see cref="LiveWallpaper"/> changes its state to enabled or disabled.
    /// </summary>
    public UnityAction OnLiveWallpaperStateChanged { get; set; }

    /// <summary>
    /// Returns true if this application was started by the launcher tool.
    /// <para/>You cannot disable the <see cref="Main"/> if this property returns true
    /// </summary>
    public static bool StartedByCommandLine { get { return m_startedByCommandLine; } }

    /// <summary>
    /// Returns true if any error occurred while enabling or disabling this <see cref="LiveWallpaper"/> instance.
    /// <para/><see cref="IsCurrentPlatformSupported"/> may not return the right value if it's out of sync.
    /// </summary>
    public static bool IsCurrentlyInWallpaperModeOutOfSync { get; private set; }

    /// <summary>
    /// Is <see cref="LiveWallpaper"/> supported on the current platform?
    /// <para/>Retuns true if we're running in the standalone build or if our target platform is Windows.
    /// </summary>
    public static bool IsCurrentPlatformSupported {
        get {
#if UNITY_STANDALONE_WIN
            return true;
#else
            return false;
#endif
        }
    }

    /// <summary>
    /// Get whether this instance is enabled or not.
    /// <para/>Prefer using <see cref="Enable"/> and <see cref="Disable"/> instead of setting a value to this property directly.
    /// </summary>
    public virtual bool IsCurrentlyInWallpaperMode {
        get {
            if(StartedByCommandLine)
                return true;

            else if(IsCurrentlyInWallpaperModeOutOfSync)
                Debug.LogWarning("Wallpaper.IsCurrentlyInWallpaperMode is out of sync, call Enable() or Disable() to fix it");

            return m_isCurrentlyInWallpaperMode;
        }
        set {
            ThrowExceptionIfStartedByCommandLine();

            if(IsCurrentlyInWallpaperMode == value)
                return;

            if(value)
                Enable();
            else
                Disable();
        }
    }

    /// <summary>
    /// Returns the WorkerW handle, which will be the parent of a <see cref="WindowHandle"/>.
    /// </summary>
    public static IntPtr WorkerWHandle {
        get {
#if !UNITY_STANDALONE_WIN
            return IntPtr.Zero;
#else
            var progman = User32.FindWindow(PROGMAN, null);
            var workerw = IntPtr.Zero;
            var shellPtr = IntPtr.Zero;
            var result = IntPtr.Zero;
            var windowEnumerator = new EnumWindowsProc((tophandle, topparamhandle) => {
                shellPtr = User32.FindWindowEx(tophandle, IntPtr.Zero, SHELLDLL_DEF_VIEW, IntPtr.Zero);

                if(shellPtr != IntPtr.Zero)
                    workerw = User32.FindWindowEx(IntPtr.Zero, tophandle, WORKERW, IntPtr.Zero);

                return true;
            });

            User32.SendMessageTimeout(progman, 0x052C, IntPtr.Zero, IntPtr.Zero, SMTO_NORMAL, 10000, out result);
            //User32.SendMessage(progman, 0x052C, IntPtr.Zero, IntPtr.Zero);
            User32.ThrowLastError();
            User32.EnumWindows(windowEnumerator, IntPtr.Zero);
            User32.ThrowLastError();

            if(workerw == IntPtr.Zero)
                throw new Exception("Could not find WorkerW handle");

            return workerw;
#endif
        }
    }

    /// <summary>
    /// The main instance of <see cref="LiveWallpaper"/>, you may always want to use this instance instead of creating one manually.
    /// <para/>If we're running in the editor this property will return an <see cref="EditorLiveWallpaper{T}"/> with a game view associated.
    /// Otherwise it will return an instance with the main window handle set as <see cref="WindowHandle"/>.
    /// </summary>
    public static LiveWallpaper Main {
        get {
            if(m_main != null)
                return m_main;

#if UNITY_EDITOR && UNITY_STANDALONE_WIN
            return m_main = new EditorLiveWallpaper<UnityEditor.EditorWindow>(typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameView"));
#elif UNITY_STANDALONE_WIN
            return m_main = new LiveWallpaper(User32.GetActiveWindowSafe());
#else
            return new LiveWallpaper(IntPtr.Zero);
#endif
        }
    }

    /// <summary>
    /// Creates a new instance of <see cref="LiveWallpaper"/>.
    /// <para/>Use <see cref="Main"/> if you're not sure of what handle to use.
    /// </summary>
    /// <param name="windowHandle">The handle of the window that will be set as wallpaper.</param>
    public LiveWallpaper(IntPtr windowHandle) {
        if(!IsCurrentPlatformSupported)
            Debug.LogWarning("Live Wallpaper not supported on current platform");

        WindowHandle = windowHandle;
    }

    /// <summary>
    /// Set <see cref="WindowHandle"/> as wallpaper.
    /// </summary>
    [Conditional("UNITY_STANDALONE_WIN")]
    public virtual void Enable() {
#if UNITY_STANDALONE_WIN
        try {
            ThrowExceptionIfStartedByCommandLine();
            User32.SetParent(WindowHandle, WorkerWHandle);
            User32.ThrowLastError();

            User32.GetWindowRect(WindowHandle, ref positionBeforeLiveWallpaper);
            Debug.Log((UnityEngine.Rect)positionBeforeLiveWallpaper);
            User32.ThrowLastError();

            positionBeforeLiveWallpaper.Right -= positionBeforeLiveWallpaper.Left;
            positionBeforeLiveWallpaper.Bottom -= positionBeforeLiveWallpaper.Top;

            positionBeforeLiveWallpaper.Left = 0;
            positionBeforeLiveWallpaper.Top = 0;

            flagsBeforeLiveWallpaper = Fullscreen.GetWindowStyle(WindowHandle);
            Fullscreen.RemoveWindowBorders(WindowHandle);
            Fullscreen.SetWindowFullscreen(WindowHandle);

            m_isCurrentlyInWallpaperMode = true;
            IsCurrentlyInWallpaperModeOutOfSync = false;
        }
        catch(Exception e) {
            IsCurrentlyInWallpaperModeOutOfSync = true;
            throw e;
        }
        finally { CallLiveWallpaperStateChanged(); }
#endif
    }

    /// <summary>
    /// Remove the <see cref="WindowHandle"/> from wallpaper and refreshes the static wallpaper.
    /// <para/>You cannot Disable the <see cref="Main"/> if it was started by the launcher tool.
    /// </summary>
    [Conditional("UNITY_STANDALONE_WIN")]
    public virtual void Disable() {
#if UNITY_STANDALONE_WIN
        try {
            ThrowExceptionIfStartedByCommandLine();
            User32.SetParent(WindowHandle, IntPtr.Zero);
            User32.ThrowLastError();

            Fullscreen.SetWindowStyle(WindowHandle, flagsBeforeLiveWallpaper);
            Fullscreen.SetWindowPosition(WindowHandle, positionBeforeLiveWallpaper);

            Wallpaper.RefreshWallpaper();

            m_isCurrentlyInWallpaperMode = false;
            IsCurrentlyInWallpaperModeOutOfSync = false;
        }
        catch(Exception e) {
            IsCurrentlyInWallpaperModeOutOfSync = true;
            throw e;
        }
        finally { CallLiveWallpaperStateChanged(); }
#endif
    }

    protected virtual void CallLiveWallpaperStateChanged() {
        if(OnLiveWallpaperStateChanged != null)
            OnLiveWallpaperStateChanged();
    }

    protected void ThrowExceptionIfStartedByCommandLine() {
        if(StartedByCommandLine && Main == this)
            throw new InvalidOperationException("You cannot enable or disable a live wallpaper started by the launcher");
    }

}