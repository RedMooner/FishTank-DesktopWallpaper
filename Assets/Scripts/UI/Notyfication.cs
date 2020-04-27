using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notyfication : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text TitleText;
    [SerializeField]
    private Text BodyText;
    [SerializeField]
    private Text ButtonText;
    [SerializeField]
    private GameObject Noty;

    public void ShowNotyfication(string title, string body, string button)
    {
        TitleText.text = title;
        BodyText.text = body;
        ButtonText.text = button;
        Noty.SetActive(true);
    }
    public void Close()
    {
        Noty.SetActive(false);

        TitleText.text = null;
        BodyText.text = null;
        ButtonText.text = null;
    }
}
