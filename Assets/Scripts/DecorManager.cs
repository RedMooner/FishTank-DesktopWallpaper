using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.ThreeDimensional;

public class DecorManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Text name;
    public Text price_text;
    int tankSize = globalFlock.tankSize;
    private int player_money;
    private int price;
    public Decoration decoration;
    public UIObject3D UIModelView;


    public void Buy()
    {
        player_money = SaveSystem.Global.global_money;
        if (player_money >= price)
        {
            Debug.Log("Куплено!!!" + decoration.name);
            Debug.LogError(price);
            GlobalVar.buildObject = decoration.prefab; //присваиваем объект, который в дальнейшем будем строить
            CreateObject.CreateObjects(); //вызываем метод для построки

        }
        else
        {
            Debug.Log("Недостаточно денег!!!");
        }
    }
    void Update()
    {
        if (decoration != null)
        {
            name.text = decoration.name;
            price_text.text = decoration.price.ToString();
            price = decoration.price;
        }
        else
        {

        }
    }

    void Start(){
            UIModelView._ObjectPrefab = decoration.prefabUI.transform;
    }
}
