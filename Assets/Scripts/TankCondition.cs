using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TankCondition : MonoBehaviour
{
    public Text priceText;
    public int price = 0;
    private int player_money;
    void Start()
    {
        StartCoroutine(PriceCondition());
    }
    public void Clean()
    {

        player_money = SaveSystem.Global.global_money;
        if (player_money >= price)
        {


            player_money = player_money - price;
            Debug.Log(player_money + "тек" + "-" + price);
            SaveSystem.Global.global_money = player_money;
            GlobalVar.Pollution = 0;
            UpdatePrice(0);
        }
        else if (player_money < price)
        {

        }
        else if (price == 0)
        {

        }
    }
    private void UpdatePrice(int value)
    {
        price = value;
        priceText.text = price.ToString();

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        priceText.text = price.ToString();
    }
    IEnumerator PriceCondition()
    {
        while (true)
        {


            //  Instantiate(respawnPrefab, respawn.transform.position, respawn.transform.rotation);
            if (GlobalVar.Pollution < 100)
            {
                price = price + 10;
            }
            else if (GlobalVar.Pollution < 80)
            {
                price = price + 20;
                priceText.text = price.ToString();
            }
            else if (GlobalVar.Pollution < 60)
            {
                price = price + 30;
                priceText.text = price.ToString();
            }
            else if (GlobalVar.Pollution < 40)
            {
                price = price + 40;
                priceText.text = price.ToString();
            }
            else if (GlobalVar.Pollution < 20)
            {
                price = price + 50;
                priceText.text = price.ToString();
            }
            else
            {
                price = 0;
            }


            yield return new WaitForSeconds(10);
        }
    }


}