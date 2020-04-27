using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Application = UnityEngine.Application;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private Money MoneyScript;
    [SerializeField] private globalFlock globalFlock;
    [SerializeField] private bool Debug_point = false;
    [System.Serializable]
    public class Global
    {
        public static int global_money = 10000;

    }
    public void Set(int value, string key)
    {
        PlayerPrefs.SetInt(key, value);
    }
    public void Set(string value, string key)
    {
        PlayerPrefs.SetString(key, value);
    }
    public void Set(float value, string key)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    public void CheckProcces()
    {
        var procs = System.Diagnostics.Process.GetProcessesByName("Boids Project"); // Ищем процесс игры (все процессы)
        if (procs.Length > 1) // если процессов больше одного , то убиваем новый
        {
            Application.Quit();
        }
    }
    void Awake()
    {
        CheckProcces();
        print("loading...");

        Global.global_money = PlayerPrefs.GetInt("money_key");
        print("Загрузилось всего " + Global.global_money);
        globalFlock.numFish = PlayerPrefs.GetInt("all_fish");
        MoneyScript.money = PlayerPrefs.GetInt("money_key");
        globalFlock.allFish = new GameObject[PlayerPrefs.GetInt("all_fish")];
        print("Загружено!" + "Деньги::  " + Global.global_money + " Рыбы::  " + globalFlock.numFish + "Деньги:: " + MoneyScript.money);





    }

}
