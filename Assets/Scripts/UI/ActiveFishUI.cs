using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActiveFishUI : MonoBehaviour
{

    public GameObject[] fishes;
    public Text nameText;
    public Text descr;

    public void SetFish(string name, string param)
    {
        //nameText.text = name;
        //descr.text = param;
    }

    public void Update()
    {
        fishes = GameObject.FindGameObjectsWithTag("Fish");
        foreach (GameObject respawn in fishes)
        {
            //  Instantiate(respawnPrefab, respawn.transform.position, respawn.transform.rotation);
            if (respawn.GetComponent<FishBehavior>().active == true)
            {
                //SetFish(respawn.GetComponent<FishBehavior>().name, respawn.GetComponent<FishBehavior>().food.ToString());
                //respawn.GetComponent<FishBehavior>().active = false;
            }

        }
    }
}
