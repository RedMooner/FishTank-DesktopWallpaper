using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    //public float Scale = 5;
    [Header("Скорость анимации")]

    [Space(15)] [Range(0, 2)] public float AnimSpeed = 1;
    [Space(15)]
    [Header("Кривая")] public AnimationCurve animationCurve;

    [Header("Режим бесконечного проигрывания анмации")]
    public bool loop = false;
    bool isAnim;
    float animTime;

    void OnMouseEnter()
    {
        isAnim = true;
        animTime = 0;
    }

    void Update()
    {
        if (loop == true)
        {
            animTime += Time.deltaTime * AnimSpeed;
            float value = animationCurve.Evaluate(animTime);
            transform.localScale = Vector3.Slerp(Vector3.one, Vector3.one * value, animTime);
            animTime = 0;
        }
        else
        {
            if (AnimSpeed == 0)
                Debug.LogWarning("Скорость анимации равно 0 , анимация проигрываться не будет!");
            if (isAnim)
            {
                animTime += Time.deltaTime * AnimSpeed;
                float value = animationCurve.Evaluate(animTime);
                Debug.Log(animTime);
                if (animTime > 1)
                {
                    isAnim = false;
                }
                else
                {
                    transform.localScale = Vector3.Slerp(Vector3.one, Vector3.one * value, animTime);
                }
            }
        }

    }
    public void OnMouseExit()
    {
        Debug.Log("left");
        transform.localScale = new Vector3(1, 1, 1);  // assuming you want it to return to its original size when your mouse leaves it.
        isAnim = false;
    }

}
