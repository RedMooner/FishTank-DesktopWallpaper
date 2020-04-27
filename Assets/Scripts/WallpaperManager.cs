using System;
using System.Diagnostics;
using System.IO;

using UnityEngine;
using UnityEngine.UI;


namespace LiveWallpaperCore
{
    /// <summary>
    /// Just a demo script to show off how to use this asset.
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class WallpaperManager : MonoBehaviour
    {
        [SerializeField]
        public GameObject canvas;
        [SerializeField] private Money MoneyScript;
        public GameObject Noty;
        public GameObject Graphy;
        public MainManager mainManager;
        public GameObject DebugConsole;
        public GameObject Canvas;
        public FishUI fishUI;
        private bool hook = false;
        float oldTime;
        public bool flag = false;
        private void Start()
        {
            //Get the current wallpaper texture
            var wallpaperTexture = Wallpaper.GetWallpaperTexture();
            //And asign it to the material of this game object renderer.
            GetComponent<MeshRenderer>().material.mainTexture = wallpaperTexture;

            //Calculate the texture ratio
            var textureRatio = (float)wallpaperTexture.width / wallpaperTexture.height;
            //And apply it to this object scale, so the texture won't look stretched.
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(textureRatio, 1f, textureRatio));

            //Reload the interface when the live wallpaper state changes.
            LiveWallpaper.Main.OnLiveWallpaperStateChanged += ReloadInterface;

            ReloadInterface();
        }
        public void ToogleClick()
        {
            ToggleLiveWallpaper(true);
            flag = true;
        }
        void Hook()
        {
            UnityEngine.Debug.Log("5");
            if (GlobalVar.OffWalling == "F1true")
            {
                    UnityEngine.Debug.Log("6");
                hook = true;


                Canvas.SetActive(false);
                ToggleLiveWallpaper(!flag);
                GlobalVar.livePaper = true;
                // flag = true;


                File.WriteAllText("log.txt", null);
                hook = false; // переведим переменную в false чтобы продолжить поиск нажатия хоткея
                              // MessageBox.Show(hook.ToString());

            }
             if (GlobalVar.OffWalling == "F1false")
            {
                hook = true;
                Canvas.SetActive(true);
                Noty.GetComponent<Animator>().Play(0);
                  hook = false;
                ToggleLiveWallpaper(false);
                GlobalVar.livePaper = false;
                File.WriteAllText("log.txt", null);

            }
             if (GlobalVar.OffWalling == "ESC")
            {
                hook = true;
                mainManager.Quit();
                hook = false;
            }
             if (GlobalVar.OffWalling == "K")
            {
                hook = true;
                fishUI.KillFishes();
                hook = false;
            }
             if (GlobalVar.OffWalling == "=")
            {
                hook = true;
                MoneyScript.money += 1000000;
                SaveSystem.Global.global_money += 1000000;
                hook = false;
            }

        }

        void HotKey()
        {
            if (GlobalVar.OffWalling == "ESC")
            {
                hook = true;
                mainManager.Quit();
                hook = false;
            }
            if (GlobalVar.OffWalling == "K")
            {
                hook = true;
                fishUI.KillFishes();
                hook = false;
            }
            if (GlobalVar.OffWalling == "=")
            {
                hook = true;
                MoneyScript.money += 1000000;
                SaveSystem.Global.global_money += 1000000;
                hook = false;
            }
        }
        void Update()
        {
            try
            {
                StreamReader sr = new StreamReader("log.txt");
                string value = sr.ReadToEnd(); // значение лога
                GlobalVar.OffWalling = value;
                sr.Close();
                UnityEngine.Debug.Log("Hook = " + hook);
                if (hook == false) // проверка переменной если true то хоткей уже нажат и идет проверка если false то идет проверка на нажатие
                {
                    Hook(); // проверка
                }
                HotKey();
            }
            catch (FileNotFoundException)
            {

            }
        }

        void TestVoid()
        {


        }

        public void ToggleLiveWallpaper(bool enable)
        {
            //This method is used by the toggle in the demo scene,
            //The parameter is passed by unity, we'll use it to
            //decide if we need to enable or disable the live wallpaper.
            if (enable)
            {
                LiveWallpaper.Main.Enable(); //This is where the magic begins.
                canvas.SetActive(false);
                flag = true;
                GlobalVar.livePaper = true;

            }

            if (enable == false)
            {
                canvas.SetActive(true);
                flag = false;
                GlobalVar.livePaper = false;
                LiveWallpaper.Main.Disable(); //This is where the magic ends.
            }


            ReloadInterface();
        }
        public void Btn_Toggle()
        {
            LiveWallpaper.Main.Enable(); //This is where the magic begins.
            canvas.SetActive(false);
            flag = true;
            GlobalVar.livePaper = true;
            File.WriteAllText("log.txt", "");
            File.WriteAllText("log.txt", "btn_true");
        }
        private void ReloadInterface()
        {
            //Setup the toggle to make it enable and disable the live wallpaper.
            // toggle.onValueChanged.RemoveAllListeners();
            // toggle.isOn = LiveWallpaper.Main.IsCurrentlyInWallpaperMode;
            //  toggle.onValueChanged.AddListener(ToggleLiveWallpaper);

            //We get whether the wallpaper is enabled and change the label text.
            //toggleLabel.text = LiveWallpaper.Main.IsCurrentlyInWallpaperMode ? "Disable wallpaper" : "Enable wallpaper";
        }
    }
}
