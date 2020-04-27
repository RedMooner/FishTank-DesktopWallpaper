using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ThreeDimensional
{
public class GetInfoFish : MonoBehaviour
{
   public GameObject InfoPanel;
   public UIObject3D model;
   public Text InfoNameFish, ReplaceText;
   public Scrollbar InfoFoodFish;
   public Scrollbar InfoHealthFish;
   public InputField RenameField;
   public GameObject gm;

   public LineRenderer lineR;
   public GameObject SetLineLeft;

   private GameObject ModelFish;

   private int IdMassivFish;
   private RaycastHit hit;
   private Transform PosFish;
   private Camera cam;

    void Start()
    {
 cam = GameObject.Find("Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //  RaycastHit hit;
      if(Input.GetMouseButtonDown(0)){
        if(Physics.Raycast(ray, out hit)){
          if((hit.transform.tag == "Fish")&&(hit.transform.GetComponent<FishBehavior>())){
          Destroy(ModelFish);
          RenameField.text = "";
          InfoPanel.SetActive(false);
          InfoPanel.SetActive(true);
          PosFish = hit.transform;
          ModelFish = (GameObject)Instantiate(hit.transform.gameObject,new Vector3(-100,-100,-100),Quaternion.identity);
          Destroy(ModelFish.GetComponent<flock>());
          Destroy(ModelFish.GetComponent<FishBehavior>());
          model._ObjectPrefab = ModelFish.transform;
          IdMassivFish = hit.transform.GetComponent<FishBehavior>().IDMassiv;
        }
        }
      }
      if((InfoPanel.activeInHierarchy)&&(PosFish)) {
          lineR.enabled = true;
          InfoFoodFish.size = PosFish.transform.GetComponent<FishBehavior>().food * 0.01f;
          InfoHealthFish.size = PosFish.transform.GetComponent<FishBehavior>().health * 0.01f;
          InfoNameFish.text = PosFish.transform.GetComponent<FishBehavior>().name.ToString();
          LineRen(PosFish.transform);
          Debug.Log(hit.transform.name);
      }else{
        InfoPanel.SetActive(false);
         lineR.enabled = false;
      }
          CloseInfoPanel();
    }
    void LineRen(Transform hitPoint){
      var camPos = Camera.main.ScreenToWorldPoint(SetLineLeft.transform.position);
      lineR.SetPosition(0, camPos);
      lineR.SetPosition(1, hitPoint.transform.position);
    }


    public void CloseInfoPanel(){
      if((InfoNameFish.text != ReplaceText.name)&&(ReplaceText.text != "")){
        GlobalVar.Fish[IdMassivFish].GetComponent<FishBehavior>().name = ReplaceText.text;
      }
    }
}
}
