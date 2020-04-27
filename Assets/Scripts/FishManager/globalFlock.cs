using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalFlock : MonoBehaviour
{
    public GameObject fishPrefab;
    public GameObject goalPrefab;
    public static int tankSize = 5;

    [SerializeField]
    public static int numFish = 1;
    public static GameObject[] allFish = new GameObject[numFish];
    public static Vector3 goalPos = Vector3.zero;

    private GlobalVar GlobalV;
    // Start is called before the first frame update
    void Start()
    {
        GlobalV = GameObject.Find("ManagerUI").GetComponent<GlobalVar>();
        //RenderSettings.fogColor = Camera.main.backgroundColor;
        //  RenderSettings.fogDensity = 0.3F;
        // RenderSettings.fog = true;
        GlobalVar.Fish = new GameObject[GlobalVar.sv.CountFish];
        for (int i = 0; i < GlobalVar.sv.CountFish; i++)
        {
             Vector3 pos = new Vector3(Random.Range(-tankSize, tankSize),
                                     Random.Range(-tankSize, tankSize),
                                    Random.Range(-tankSize, tankSize));
            GlobalVar.Fish[i] = Instantiate(GlobalV.AllFish[GlobalVar.sv.IDFish[i]], GlobalVar.sv.PositionFish[i], Quaternion.identity);
            GlobalVar.Fish[i].transform.rotation = GlobalVar.sv.RotationFish[i];
            GlobalVar.Fish[i].transform.localScale = GlobalVar.sv.ScaleFish[i];
            GlobalVar.Fish[i].GetComponent<FishBehavior>().health = GlobalVar.sv.HealthFish[i];
            GlobalVar.Fish[i].GetComponent<FishBehavior>().food = GlobalVar.sv.foodFish[i];
            GlobalVar.Fish[i].GetComponent<FishBehavior>().name = GlobalVar.sv.NameFish[i];
            GlobalVar.Fish[i].GetComponent<FishBehavior>().oxygen = GlobalVar.sv.oxygenFish[i];
            GlobalVar.Fish[i].GetComponent<FishBehavior>().DeathCount = GlobalVar.sv.DeadCountFish[i];
            GlobalVar.Fish[i].GetComponent<FishBehavior>().IDMassiv = i;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator UpdatePositionGoalPos() //Обновление позиции goalPrefab
    {
        while (true)
        {
            yield return new WaitForSeconds(2); // каждые 2 секунды будет обновляться позиция
            goalPos = new Vector3(Random.Range(-tankSize, tankSize),
                                    Random.Range(-tankSize, tankSize),
                                    Random.Range(-tankSize, tankSize));
            goalPrefab.transform.position = goalPos;
        }
    }
}
