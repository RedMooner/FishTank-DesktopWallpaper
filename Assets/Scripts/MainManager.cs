using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public MainManager mainManager;
    [SerializeField] private globalFlock globalFlock;
    [SerializeField] private Money MoneyScript;
    void Awake()
    {

        Process.Start("dist\\win-unpacked\\s.exe");
        File.WriteAllText("log.txt", "");

    }
    public void Quit()
    {


        UnityEngine.Application.Quit();
    }
    void OnApplicationQuit()
    {
        foreach (var process in Process.GetProcessesByName("s"))
        {
            process.Kill();
        }
        PlayerPrefs.SetInt("money_key", MoneyScript.money);
        PlayerPrefs.SetInt("all_fish", globalFlock.numFish);
        PlayerPrefs.SetInt("money_key", SaveSystem.Global.global_money);
    }
    bool isPaused = false;

    void OnGUI()
    {
        if (isPaused)
        {
            GUI.Label(new Rect(100, 100, 50, 30), "Game paused");

        }

    }

    void OnApplicationFocus(bool hasFocus)
    {
        isPaused = !hasFocus;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;

    }
}