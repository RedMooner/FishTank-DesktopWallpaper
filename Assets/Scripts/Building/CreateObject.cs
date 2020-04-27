using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void CreateObjects()
    {
       GameObject pref = GameObject.Instantiate(Resources.Load(GlobalVar.buildObject.name) as GameObject); //спавним объект на сцену
        pref.tag = ("CreateObj"); //присваиваем тег
        GlobalVar.activeBuild = true; //активируем режим строительства
    }
}
