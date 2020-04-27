using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveFile : MonoBehaviour
{
    public GlobalVar GlobalV;

    void Start()
    {
        GlobalV = GameObject.Find("ManagerUI").GetComponent<GlobalVar>();
        GlobalVar.path = Path.Combine(Application.dataPath, "Save.json");
        if (File.Exists(GlobalVar.path))
        {
            GlobalVar.sv = JsonUtility.FromJson<Save>(File.ReadAllText(GlobalVar.path));
        }
        else
        {
            File.WriteAllText(GlobalVar.path, JsonUtility.ToJson(GlobalVar.sv)); //сохраняемся
        }
        Load();
    }


    void Update()
    {
    }

    void Load()
    {
       GlobalVar.Objects = new GameObject[GlobalVar.sv.CountObjects];
        for (int f = 0; f < GlobalVar.sv.CountObjects; f++)
        {
            GlobalVar.Objects[f] = Instantiate(GlobalV.AllObjects[GlobalVar.sv.IDObject[f]], GlobalVar.sv.PositionObject[f], GlobalVar.sv.RotationObject[f]);
            GlobalVar.Objects[f].transform.position = GlobalVar.sv.PositionObject[f];
            GlobalVar.Objects[f].transform.rotation = GlobalVar.sv.RotationObject[f];
        }
    }

    private void OnApplicationQuit()
    {
      GlobalVar.sv.PositionFish = new Vector3[GlobalVar.sv.CountFish];
      GlobalVar.sv.RotationFish = new Quaternion[GlobalVar.sv.CountFish];
      GlobalVar.sv.IDFish = new int[GlobalVar.sv.CountFish];
      GlobalVar.sv.HealthFish = new int[GlobalVar.sv.CountFish];
      GlobalVar.sv.foodFish = new int[GlobalVar.sv.CountFish];
      GlobalVar.sv.oxygenFish = new float[GlobalVar.sv.CountFish];
      GlobalVar.sv.DeadCountFish = new double[GlobalVar.sv.CountFish];
      GlobalVar.sv.oxygenFish = new float[GlobalVar.sv.CountFish];
      GlobalVar.sv.NameFish = new string[GlobalVar.sv.CountFish];
      GlobalVar.sv.ScaleFish = new Vector3[GlobalVar.sv.CountFish];
      GlobalVar.sv.IDObject = new int[GlobalVar.sv.CountObjects];
      GlobalVar.sv.PositionObject = new Vector3[GlobalVar.sv.CountObjects];
      GlobalVar.sv.RotationObject = new Quaternion[GlobalVar.sv.CountObjects];

      for (int i = 0; i < GlobalVar.sv.CountFish; i++)
      {
          if (GlobalVar.Fish[i])
          {
              GlobalVar.sv.IDFish[i] = GlobalVar.Fish[i].GetComponent<flock>().ID;
              GlobalVar.sv.PositionFish[i] = GlobalVar.Fish[i].transform.position;
              GlobalVar.sv.RotationFish[i] = GlobalVar.Fish[i].transform.rotation;
              GlobalVar.sv.HealthFish[i] = GlobalVar.Fish[i].GetComponent<FishBehavior>().health;
              GlobalVar.sv.foodFish[i] = GlobalVar.Fish[i].GetComponent<FishBehavior>().food;
              GlobalVar.sv.NameFish[i] = GlobalVar.Fish[i].GetComponent<FishBehavior>().name;
              GlobalVar.sv.oxygenFish[i] = GlobalVar.Fish[i].GetComponent<FishBehavior>().oxygen;
              GlobalVar.sv.DeadCountFish[i] = GlobalVar.Fish[i].GetComponent<FishBehavior>().DeathCount;
              GlobalVar.sv.ScaleFish[i] = GlobalVar.Fish[i].transform.localScale;
          }
      }
      for (int obj = 0; obj < GlobalVar.sv.CountObjects; obj++)
      {
          GlobalVar.sv.IDObject[obj] = GlobalVar.Objects[obj].GetComponent<Object>().ID;
          GlobalVar.sv.PositionObject[obj] = GlobalVar.Objects[obj].transform.position;
          GlobalVar.sv.RotationObject[obj] = GlobalVar.Objects[obj].transform.rotation;
      }

      File.WriteAllText(GlobalVar.path, JsonUtility.ToJson(GlobalVar.sv)); //сохраняемся
      Debug.Log("Сохранено File!");
    }

    public static void Saving()
    {
        GlobalVar.sv.PositionFish = new Vector3[GlobalVar.sv.CountFish];
        GlobalVar.sv.RotationFish = new Quaternion[GlobalVar.sv.CountFish];
        GlobalVar.sv.IDFish = new int[GlobalVar.sv.CountFish];
        GlobalVar.sv.HealthFish = new int[GlobalVar.sv.CountFish];
        GlobalVar.sv.foodFish = new int[GlobalVar.sv.CountFish];
        GlobalVar.sv.oxygenFish = new float[GlobalVar.sv.CountFish];
        GlobalVar.sv.DeadCountFish = new double[GlobalVar.sv.CountFish];
        GlobalVar.sv.oxygenFish = new float[GlobalVar.sv.CountFish];
        GlobalVar.sv.NameFish = new string[GlobalVar.sv.CountFish];
        GlobalVar.sv.ScaleFish = new Vector3[GlobalVar.sv.CountFish];
        GlobalVar.sv.IDObject = new int[GlobalVar.sv.CountObjects];
        GlobalVar.sv.PositionObject = new Vector3[GlobalVar.sv.CountObjects];
        GlobalVar.sv.RotationObject = new Quaternion[GlobalVar.sv.CountObjects];

        for (int i = 0; i < GlobalVar.sv.CountFish; i++)
        {
            if (GlobalVar.Fish[i])
            {
                GlobalVar.sv.IDFish[i] = GlobalVar.Fish[i].GetComponent<flock>().ID;
                GlobalVar.sv.PositionFish[i] = GlobalVar.Fish[i].transform.position;
                GlobalVar.sv.RotationFish[i] = GlobalVar.Fish[i].transform.rotation;
                GlobalVar.sv.HealthFish[i] = GlobalVar.Fish[i].GetComponent<FishBehavior>().health;
                GlobalVar.sv.foodFish[i] = GlobalVar.Fish[i].GetComponent<FishBehavior>().food;
                GlobalVar.sv.NameFish[i] = GlobalVar.Fish[i].GetComponent<FishBehavior>().name;
                GlobalVar.sv.oxygenFish[i] = GlobalVar.Fish[i].GetComponent<FishBehavior>().oxygen;
                GlobalVar.sv.DeadCountFish[i] = GlobalVar.Fish[i].GetComponent<FishBehavior>().DeathCount;
                GlobalVar.sv.ScaleFish[i] = GlobalVar.Fish[i].transform.localScale;
            }
        }
        for (int obj = 0; obj < GlobalVar.sv.CountObjects; obj++)
        {
            GlobalVar.sv.IDObject[obj] = GlobalVar.Objects[obj].GetComponent<Object>().ID;
            GlobalVar.sv.PositionObject[obj] = GlobalVar.Objects[obj].transform.position;
            GlobalVar.sv.RotationObject[obj] = GlobalVar.Objects[obj].transform.rotation;
        }

        File.WriteAllText(GlobalVar.path, JsonUtility.ToJson(GlobalVar.sv)); //сохраняемся
        Debug.Log("Сохранено File!");
    }
}
[Serializable]
public class Save
{
    public int CountFish = 0; //пока что не используется, но лучше использовать эту переменную
    public int[] IDFish; //здесь хранятся все ID рыб
    public Vector3[] PositionFish; //здесь хранятся позиции рыб
    public Vector3[] ScaleFish;
    public Quaternion[] RotationFish; //здесь хранятся наклон рыб

    //сохранение объектов на сцене
    public int CountObjects;
    public int[] IDObject;
    public Vector3[] PositionObject;
    public Quaternion[] RotationObject;

    //Сохраняем состояние рыбы
    public int[] HealthFish;
    public int[] foodFish;
    public string[] NameFish;
    public float[] oxygenFish;
    public double[] DeadCountFish;

}
