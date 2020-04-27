using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUI : MonoBehaviour
{
    // public GameObject FishViewPanel;
    public GameObject[] fishes;
    public bool FoodStatus = false;
    public bool TankStatus = false;
    public GameObject[] notyfications;
    void Update()
    {
        fishes = GameObject.FindGameObjectsWithTag("Fish");
        foreach (GameObject respawn in fishes)
        {
            //  Instantiate(respawnPrefab, respawn.transform.position, respawn.transform.rotation);
            if (respawn.GetComponent<FishBehavior>().food < 50)
            {
                notyfications[1].SetActive(true);
            }
            else
            {
                notyfications[1].SetActive(false);
            }


        }



    }
    public void SetVisibleCustom(GameObject gameObject, bool value)
    {
        gameObject.SetActive(value);
    }
    public void SetVisible(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
