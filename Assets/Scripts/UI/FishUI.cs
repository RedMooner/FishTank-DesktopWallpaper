using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FishUI : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] fishes;
    [SerializeField]
    private Text priceText;
    [SerializeField]
    private int factor = 2;
    public int price = 0;
    private int player_money;
    public Notyfication notyfication;
    void Start()
    {
        StartCoroutine(PriceCondition());
    }
    #region Система управления рыбами

    public void KillFishes()
    {
        GameObject[] allfishes = GameObject.FindGameObjectsWithTag("Fish");
        foreach (GameObject respawn in allfishes)
        {
            //  Instantiate(respawnPrefab, respawn.transform.position, respawn.transform.rotation);
            Destroy(respawn);

        }
    }

    #endregion
    void Update()
    {

        //  fishes = GameObject.FindGameObjectsWithTag("Fish");
        //    price = (fishes.Length * factor);

        priceText.text = price.ToString();

    }
    public void Feed()
    {
        player_money = SaveSystem.Global.global_money;
        if (player_money >= price)
        {
            fishes = GameObject.FindGameObjectsWithTag("Fish");
            foreach (GameObject respawn in fishes)
            {
                //  Instantiate(respawnPrefab, respawn.transform.position, respawn.transform.rotation);
                respawn.GetComponent<FishBehavior>().food = 100;
                Debug.Log("Рыбы покормлены");


            }
            player_money = player_money - price;
            Debug.Log(player_money + "тек" + "-" + price);
            SaveSystem.Global.global_money = player_money;
            UpdatePrice(0);
        }
        else if (player_money < price)
        {
            notyfication.ShowNotyfication("Alert!!!", "You do not have any money to buy it", "Close");
        }
        else if (price == 0)
        {
            notyfication.ShowNotyfication("Alert!!!", "All fish already food!", "Close");
        }



    }
    private void UpdatePrice(int value)
    {
        price = value;
        priceText.text = price.ToString();

    }
    IEnumerator PriceCondition()
    {
        while (true)
        {
            fishes = GameObject.FindGameObjectsWithTag("Fish");
            foreach (GameObject respawn in fishes)
            {
                //  Instantiate(respawnPrefab, respawn.transform.position, respawn.transform.rotation);
                if (respawn.GetComponent<FishBehavior>().food < 100)
                {
                    price = price + (fishes.Length * factor);
                }
                else if (respawn.GetComponent<FishBehavior>().food < 80)
                {
                    price = price + (fishes.Length * factor);
                    priceText.text = price.ToString();
                }
                else if (respawn.GetComponent<FishBehavior>().food < 60)
                {
                    price = price + (fishes.Length * factor);
                    priceText.text = price.ToString();
                }
                else if (respawn.GetComponent<FishBehavior>().food < 40)
                {
                    price = price + (fishes.Length * factor);
                    priceText.text = price.ToString();
                }
                else if (respawn.GetComponent<FishBehavior>().food < 20)
                {
                    price = price + (fishes.Length * factor);
                    priceText.text = price.ToString();
                }
                else
                {
                    price = 0;
                }

            }
            yield return new WaitForSeconds(10);
        }
    }
}
