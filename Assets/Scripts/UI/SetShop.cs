using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class SetShop : MonoBehaviour
{
    // Start is called before the first frame update

    public bool active_fish = false;
    public bool active_objects = false;
    public bool active_food = false;
    [SerializeField]
    private Text title_gameobject;
    public GameObject Panel;
    public GameObject fishItem_prf;
    public GameObject decorItem_prf;
    public string[] titles;
    public Fish[] items_fish;
    public GameObject[] items_food;
    public Decoration[] items_obj;


    void Start()
    {
        Panel = GameObject.Find("ManagerUI").GetComponent<ManagerUI>().ShopPan;
    }
    public void set(string type)
    {
        if (type == "fish")
        {

            active_fish = true;
            title_gameobject.text = titles[0];
            UpdatePanel();

        }
        else if (type == "food")
        {
            active_food = true;
            title_gameobject.text = titles[1];
            UpdatePanel();
        }
        else if (type == "obj")
        {
            active_objects = true;
            title_gameobject.text = titles[2];
            UpdatePanel();
        }
        else
        {
            Debug.LogError("Введите тип магазина!!!");
        }
    }


    // Update is called once per frame
    void UpdatePanel()
    {
        var childList = Panel.transform.Cast<Transform>().ToList();
        print(childList.Count);
        foreach (Transform childTransform in childList)
        {
            Destroy(childTransform.gameObject);
        }
        if (active_fish == true)
        {

            Debug.Log("Fish");
            for (int i = 0; i < items_fish.Length; i++)
            {

                GameObject myPrefabClone = Instantiate(fishItem_prf) as GameObject;

                myPrefabClone.GetComponent<FisgManager>().fish = items_fish[i];
                myPrefabClone.transform.parent = Panel.transform; //устанавливаем родителей (CartsShop)
                myPrefabClone.transform.localScale = new Vector2(1, 1); //устанавливаем локальный размер
                myPrefabClone.transform.localPosition = new Vector2(0, 0); //устанавливаем локальную позицию
                myPrefabClone.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            active_fish = false;
        }
        else
        if (active_food == true)
        {
            Debug.Log("Food");

        }
        else
        if (active_objects == true)
        {
            Debug.Log("obj");
            for (int i = 0; i < items_obj.Length; i++)
            {

                GameObject myPrefabClone = Instantiate(decorItem_prf) as GameObject;

                myPrefabClone.GetComponent<DecorManager>().decoration = items_obj[i];
                myPrefabClone.transform.parent = Panel.transform; //устанавливаем родителей (CartsShop)
                myPrefabClone.transform.localScale = new Vector2(1, 1); //устанавливаем локальный размер
                myPrefabClone.transform.localPosition = new Vector2(0, 0); //устанавливаем локальную позицию
                myPrefabClone.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            active_objects = false;
        }
    }

}
