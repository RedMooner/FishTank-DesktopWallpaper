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
    private GameObject SetSpawnFish1; // 1 точка спавна рыбы
    private GameObject SetSpawnFish2; // 2 точка спавна рыбы
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
            Vector3 randSpawn = new Vector3(Random.Range(SetSpawnFish1.transform.position.x,SetSpawnFish2.transform.position.x), SetSpawnFish1.transform.position.y, Random.Range(SetSpawnFish1.transform.position.z,SetSpawnFish2.transform.position.z));
            GlobalVar.Fish[GlobalVar.sv.CountFish] = Instantiate(fish.prefab, randSpawn, Quaternion.identity);
            GlobalVar.Fish[GlobalVar.sv.CountFish].GetComponent<FishBehavior>().StartFish = true;
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
      SetSpawnFish1 = GameObject.Find("SetSpawnFish1");
      SetSpawnFish2 = GameObject.Find("SetSpawnFish2");
      if(fish.prefabUI){
        UIModelView._ObjectPrefab = fish.prefabUI.transform;
      }
    }
}
