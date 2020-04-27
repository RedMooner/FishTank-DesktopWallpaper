//LIVENDA CTAA CINEMATIC TEMPORAL ANTI ALIASING
//Copyright Livenda Labs 2020
//CTAA-NXT V2.0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTAA_EXCLUDE : MonoBehaviour
{
    public bool useAlpha = false;
    private Material[] mats;
    public bool m_IncludeChildren = false;
    public bool UI = false;
    
    void Start()
    {
        if (GetComponent<Renderer>() != null)
        {
            mats = GetComponent<Renderer>().materials;

            if (mats.Length > 0)
            {
                foreach (Material mat in mats)
                {
                    mat.SetFloat("rtmask", 1.0f);
                    mat.SetInt("_useAlpha", (useAlpha ? 1 : 0));
                }
            }                        
            
        }

        Material myMat = null;
        if (GetComponent<CanvasRenderer>() != null)
        {
            myMat = GetComponent<CanvasRenderer>().GetMaterial();
        }
        else
        {

            if (GetComponent<Renderer>() != null)
                myMat = GetComponent<Renderer>().material;
        }

        if (myMat != null)
        {
            myMat.SetFloat("rtmask", 1.0f);
            myMat.SetInt("_useAlpha", (useAlpha ? 1 : 0));
        }

        //Children

        if (m_IncludeChildren)
        {
            Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform trns in ts)
            {
                if (trns.gameObject.GetComponent<Renderer>() != null)
                {
                    mats = trns.gameObject.GetComponent<Renderer>().materials;

                    if (mats.Length > 0)
                    {
                        foreach (Material mat in mats)
                        {
                            mat.SetFloat("rtmask", 1.0f);
                            mat.SetInt("_useAlpha", (useAlpha ? 1 : 0));
                        }
                    }

                    Material myMatChild;
                    if (trns.gameObject.GetComponent<CanvasRenderer>() != null)
                        myMatChild = trns.gameObject.GetComponent<CanvasRenderer>().GetMaterial();
                    else
                        myMatChild = trns.gameObject.GetComponent<Renderer>().material;

                    if (myMatChild != null)
                    {
                        myMatChild.SetFloat("rtmask", 1.0f);
                        myMatChild.SetInt("_useAlpha", (useAlpha ? 1 : 0));
                    }
                }
            }
        }


    }

    
    void Update()
    {
        if (UI)
        {
            Material myMat = null;
            if (GetComponent<CanvasRenderer>() != null)
                myMat = GetComponent<CanvasRenderer>().GetMaterial();

            if (myMat != null)
            {
                myMat.SetFloat("rtmask", 1.0f);
                myMat.SetInt("_useAlpha", (useAlpha ? 1 : 0));
            }
        }
    }
}
