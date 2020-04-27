using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerUI : MonoBehaviour
{
    public GameObject canvas;
    public GameObject canvasDesk;
    public GameObject ShopPan;
    public GameObject ShopingPanel;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVar.livePaper)
        {
            canvas.SetActive(false);
            canvasDesk.SetActive(true);
        }
        else
        {
            canvas.SetActive(true);
            canvasDesk.SetActive(false);
        }
        if (GlobalVar.activeBuild)
        {
            ShopingPanel.SetActive(false);
        }

    }
}
