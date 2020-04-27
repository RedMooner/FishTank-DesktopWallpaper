using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{

    public Material CreateMat;
    public Material BlockMat;
    public Material NormalMat;
    public int ID;
    public int IDMass; //ID в массиве

    public Collider Bc1;
   // public Collider Bc2;

    public int Price;

    string tagCreate = "CreateObj";
    string tagComplite = "CompleteObj";
     bool block = false;

    void Start()
    {
        GlobalVar.onTriggerObj = false;
    }


    void Update()
    {
      if((gameObject.tag == tagCreate)&&(GlobalVar.onTriggerObj == false))
        {
            gameObject.GetComponentInChildren <MeshRenderer> ().material = CreateMat;
        }
        if ((gameObject.tag == tagCreate) && (GlobalVar.onTriggerObj == true))
        {
            gameObject.GetComponentInChildren<MeshRenderer>().material = BlockMat;
        }
        if((GlobalVar.activeBuild == false)&&(!block))
        {
            gameObject.GetComponentInChildren<MeshRenderer>().material = NormalMat;
            gameObject.tag = tagComplite;
            Bc1.enabled = true;
            block = true;
          //  Bc2.enabled = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name != "floor")
        {
         //   GlobalVar.onTriggerObj = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        //  GlobalVar.onTriggerObj = false;
    }

}
