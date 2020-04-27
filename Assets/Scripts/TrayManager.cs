using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace LiveWallpaperCore
{
    public class TrayManager : MonoBehaviour
    {
        public MainManager mainManager;
        public WallpaperManager manager;
        private bool hook = false;

        void Hook()
        {
            Debug.Log("23r534o th34oijg 34fl34jl ;gj34l gj4;gj p;3jg3");
            if (GlobalVar.TrayLog == "modetrue")
            {
                hook = true;
                GlobalVar.livePaper = true;
                manager.ToggleLiveWallpaper(true);
                // flag = true;
                File.WriteAllText("tray_log.txt", null);
                hook = false; // переведим переменную в false чтобы продолжить поиск нажатия хоткея
                              // MessageBox.Show(hook.ToString());
            }
            else if (GlobalVar.TrayLog == "modefalse")
            {
                hook = true;
                GlobalVar.livePaper = false;
                manager.Noty.GetComponent<Animator>().Play(0);
                manager.ToggleLiveWallpaper(false);
                File.WriteAllText("tray_log.txt", null);
                hook = false;
                //MessageBox.Show("f1");
                //flag = false;
            }
            else if (GlobalVar.TrayLog == "exit")
            {
                hook = true;
                mainManager.Quit();
                hook = false;
            }

        }

        void HotKey()
        {

        }
    }
}
