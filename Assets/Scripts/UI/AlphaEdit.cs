using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaEdit : MonoBehaviour
{
    public InputField editText;
    public float MaxAlpha;
    public float Speed;

    private bool Edit, ColorPl,ColorMin;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if(editText.isFocused){
        if(gameObject.GetComponent<Image>().color.a <= 0){
          ColorPl = true;
        }
        if(gameObject.GetComponent<Image>().color.a >= MaxAlpha){
          ColorMin = true;
        }
        ColorEdit();
      }else{
        gameObject.GetComponent<Image>().color =  new Color(0,0,0,0);
      }
    }

    void ColorEdit(){
      if(ColorPl){
        if(gameObject.GetComponent<Image>().color.a < MaxAlpha){
          ColorMin = false;
          gameObject.GetComponent<Image>().color +=  new Color(0,0,0,Speed * Time.deltaTime);
        }else{
          ColorPl = false;
        }
      }

      if(ColorMin){
        if(gameObject.GetComponent<Image>().color.a > 0){
          gameObject.GetComponent<Image>().color -= new Color(0,0,0,Speed * Time.deltaTime);
        }else{
          ColorMin = false;
        }
      }
    }
  public void EditStart(){
  //  Edit = true;
  }
}
