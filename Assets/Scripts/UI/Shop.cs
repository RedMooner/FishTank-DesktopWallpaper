using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // Start is called before the first frame update
    public enum ShopType
    {
        FishShop,
        DecorShop,
        FoodShop
    }
    [SerializeField]
    private GameObject ShopPanel;
    [SerializeField]
    private GameObject ItemsPanel;

    public GameObject OpenShopBtn;
    public GameObject CloseShopBtn;

    public GameObject LeftContainer;
    [SerializeField] private SetShop setShop;

    public void OpenShop()
    {
        setShop.set("fish");
        ShopPanel.SetActive(true);
        ItemsPanel.SetActive(true);
        LeftContainer.SetActive(false);
        OpenShopBtn.SetActive(false);
    }
    public void CloseShop()
    {
        ShopPanel.SetActive(false);
        ItemsPanel.SetActive(false);
        LeftContainer.SetActive(true);
        OpenShopBtn.SetActive(true);
    }
    public void OpenShopCategory(ShopType type)
    {
        if (type == ShopType.FishShop)
            setShop.set("fish");
        if (type == ShopType.FoodShop)
            setShop.set("food");
        if (type == ShopType.DecorShop)
            setShop.set("obj");
        //ItemsPanel.SetActive(!ItemsPanel.activeSelf);
    }
    public void OpenFishShop()
    {
        OpenShopCategory(ShopType.FishShop);
    }
    public void OpenFoodShop()
    {
        OpenShopCategory(ShopType.FoodShop);
    }
    public void OpenObjShop()
    {
        OpenShopCategory(ShopType.DecorShop);
    }
}
