//LIVENDA CTAA CINEMATIC TEMPORAL ANTI ALIASING
//Copyright Livenda Labs 2019
//CTAA-NXT V2.0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderPostCTAA : MonoBehaviour
{
    public CTAA_PC ctaaPC;
    public Transform ctaaCamTransform;
    public Camera MaskRenderCam;    
    public Shader maskRenderShader;
    public RenderTexture maskTexRT;
    public bool layerMaskingEnabled;
    public Material layerPostMat;    

    void LateUpdate()
    {
        this.transform.position = ctaaCamTransform.position;
        this.transform.rotation = ctaaCamTransform.rotation;
        MaskRenderCam.transform.position = ctaaCamTransform.position;
        MaskRenderCam.transform.rotation = ctaaCamTransform.rotation;
    }

    private void OnDisable()
    {
        if (maskTexRT != null) DestroyImmediate(maskTexRT); maskTexRT = null;
        if(MaskRenderCam !=null) MaskRenderCam.targetTexture = null;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (((maskTexRT == null) || (maskTexRT.width != source.width)) || (maskTexRT.height != source.height))
        {
            DestroyImmediate(maskTexRT);
            maskTexRT = new RenderTexture(source.width, source.height, 16, source.format);
            maskTexRT.hideFlags = HideFlags.HideAndDontSave;
            maskTexRT.filterMode = FilterMode.Bilinear;
            maskTexRT.wrapMode = TextureWrapMode.Repeat;
            MaskRenderCam.targetTexture = maskTexRT;
        }
        
        if (layerMaskingEnabled)
            MaskRenderCam.RenderWithShader(maskRenderShader, "");
        
        
        RenderTexture tmprt = ctaaPC.getCTAA_Render();

        if (tmprt != null)
        {
            layerPostMat.SetTexture("_CTAA_RENDER", tmprt);
            layerPostMat.SetTexture("_maskTexRT", maskTexRT);

            if(layerMaskingEnabled)
                Graphics.Blit(source, destination, layerPostMat);
            else
                Graphics.Blit(tmprt, destination);

        }
        else
        {
            Graphics.Blit(source, destination);
        }

    }

}
