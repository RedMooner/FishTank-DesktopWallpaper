using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UI.ThreeDimensional;

public class FisgManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Fish fish;
    public Text name;
    public Text price_text;
    public UIObject3D UIModelView;
    private GameObject SetSpawnFish; // точка спавна рыбы
    int tankSize = globalFlock.tankSize;
    private int player_money;
    private int price;

    public void Buy()
    {
        player_money = SaveSystem.Global.global_money;
        if (player_money >= price)
        {
            Debug.Log("Куплено!!!" + fish.name);
            Debug.LogError(price);
            player_money = player_money - price;
            SaveSystem.Global.global_money = player_money;
          //  Vector3 pos = new Vector3(Random.Range(-tankSize, tankSize),
          //                         Random.Range(-tankSize, tankSize),
          //                         Random.Range(-tankSize, tankSize));

            System.Array.Resize(ref GlobalVar.Fish, GlobalVar.Fish.Length + 1);
            fish.prefab.GetComponent<FishBehavior>().name = fish.name;
            GlobalVar.Fish[GlobalVar.sv.CountFish] = Instantiate(fish.prefab, SetSpawnFish.transform.position, Quaternion.identity);
            GlobalVar.Fish[GlobalVar.sv.CountFish].GetComponent<FishBehavior>().StartFish = true;
            Debug.Log("StartFish = " + GlobalVar.Fish[GlobalVar.sv.CountFish].GetComponent<FishBehavior>().StartFish);
            GlobalVar.Fish[GlobalVar.sv.CountFish].GetComponent<flock>().enabled = false;
            GlobalVar.Fish[GlobalVar.sv.CountFish].transform.Rotate(90,0,0);
            GlobalVar.sv.CountFish++;
            SaveFile.Saving();



        }
        else
        {

            Debug.Log("Недостаточно денег!!!");
        }
    }
    void Update()
    {
      UIModelView._ObjectPrefab = fish.prefabUI.transform;
        if (fish != null)
        {
            name.text = fish.name;
            price_text.text = fish.price.ToString();
            price = fish.price;
        }
        else
        {

        }
    }
    void Start()
    {
      SetSpawnFish = GameObject.Find("SetSpawnFish");
      if(fish.prefabUI){
        UIModelView._ObjectPrefab = fish.prefabUI.transform;
      }
    }
}
