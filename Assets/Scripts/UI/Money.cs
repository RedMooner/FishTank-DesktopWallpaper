using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Money : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text money_text;

    public int money = SaveSystem.Global.global_money;
    public SaveSystem saveSys;

    [SerializeField]
    private int bouns = 500;
    // Update is called once per frame
    void Awake()
    {
        CheckOffline();
    }

    void Update()
    {
        if (money < 0)
        {
            SaveSystem.Global.global_money = 0;
            money = 0;
        }
        money = SaveSystem.Global.global_money;
        money_text.text = money.ToString();
    }

    // Проверка отсутсвия игкрока 
    void CheckOffline()
    {
        TimeSpan ts;
        if (PlayerPrefs.HasKey("LastSession"))
        {
            string LastSession = PlayerPrefs.GetString("LastSession");
            //  Debug.LogWarning(LastSession);
            ts = DateTime.Now - DateTime.Parse(LastSession);
            Debug.Log("Время : " + ts.Days + ":дней ,   " + ts.Hours + ": часов " + " " + ts.Seconds + ":Сeкунд");
            if ((ts.Hours >= 23) && (ts.Minutes >= 59))
            {
                Debug.Log("Вас не было день");
                SaveSystem.Global.global_money += 1000;
                PlayerPrefs.SetString("LastSession", DateTime.Now.ToString());
            }
            else if ((ts.Hours >= 1) || (ts.Minutes >= 10))
            {
                SaveSystem.Global.global_money += 100;
                PlayerPrefs.SetInt("money_key", SaveSystem.Global.global_money);
                Debug.Log("mon " + SaveSystem.Global.global_money);
                PlayerPrefs.SetString("LastSession", DateTime.Now.ToString());
            }


        }

    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LastSession", DateTime.Now.ToString());
    }
}
