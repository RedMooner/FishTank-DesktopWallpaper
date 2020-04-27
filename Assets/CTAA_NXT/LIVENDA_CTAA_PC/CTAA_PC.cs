//LIVENDA CTAA CINEMATIC TEMPORAL ANTI ALIASING
//Copyright Livenda Labs 2020
//CTAA-NXT V2.0


using UnityEngine;
using System.Collections;




[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/LIVENDA/CTAA_PC")]
public class CTAA_PC : MonoBehaviour
{

    //-------------------------------------------------------
    //Public Parameters
    //------------------------------------------------------- 

    [Space(5)]
    public bool CTAA_Enabled = true;
    [Header("CTAA Settings")]
    [Tooltip("Number of Frames to Blend via Re-Projection")]
    [Range(3, 16)] public int TemporalStability = 6;
    [Space(5)]
    [Tooltip("Anti-Aliasing Response and Strength for HDR Pixels")]
    [Range(0.001f, 4.0f)] public float HdrResponse = 1.2f;
    [Space(5)]
    [Tooltip("Amount of AA Blur in Geometric edges")]
    [Range(0.0f, 2.0f)] public float EdgeResponse = 0.5f;
    [Space(5)]
    [Tooltip("Amount of Automatic Sharpness added based on relative velocities")]
    [Range(0.0f, 1.5f)] public float AdaptiveSharpness = 0.2f;
    [Space(5)]
    [Tooltip("Amount sub-pixel Camera Jitter")]
    [Range(0.0f, 0.5f)] public float TemporalJitterScale = 0.475f;
    [Space(5)]    
    [Tooltip("Eliminates Micro Shimmer - (No Dynamic Objects) Suitable for Architectural Visualisation, CAD, Engineering or non-moving objects. Camera can be moved.")]
    public bool AntiShimmerMode = false;

    private int upscaleFactor = 1;
    private int resizeDownFactor = 1;

    public LayerMask m_ExcludeLayers;

    public int SuperSampleMode = 0;
    public bool ExtendedFeatures = false;
    public bool MSAA_Control = false;
    public int m_MSAA_Level = 0;
    
    public bool m_LayerMaskingEnabled = true;
    
    private Vector4 delValues = new Vector4(0.01f, 2.0f, 0.5f, 0.3f);
    //--------------------------------------------------------------

    private bool PreEnhanceEnabled = true;
    private float preEnhanceStrength = 1.0f;
    private float preEnhanceClamp = 0.005f;
    private float AdaptiveResolve = 3000.0f;
    private float jitterScale = 1.0f;

    private Material ctaaMat;
    private Material mat_enhance;
    private RenderTexture rtAccum0;
    private RenderTexture rtAccum1;    
    private RenderTexture afterPreEnhace;
    private RenderTexture upScaleRT;

    private bool firstFrame;
    private bool swap;
    private int frameCounter;
    private Vector3 camoldpos;


    private float[] x_jit = new float[] { 0.5f, -0.25f, 0.75f, -0.125f, 0.625f, 0.575f, -0.875f, 0.0625f, -0.3f, 0.75f, -0.25f, -0.625f, 0.325f, 0.975f, -0.075f, 0.625f };
    private float[] y_jit = new float[] { 0.33f, -0.66f, 0.51f, 0.44f, -0.77f, 0.12f, -0.55f, 0.88f, -0.83f, 0.14f, 0.71f, -0.34f, 0.87f, -0.12f, 0.75f, 0.08f };

    public bool moveActive = true;
    public float speed = 0.002f;
    private int count = 0;

    void SetCTAA_Parameters()
    {
        PreEnhanceEnabled = AdaptiveSharpness > 0.01 ? true : false;
        preEnhanceStrength = Mathf.Lerp(0.2f, 2.0f, AdaptiveSharpness);
        preEnhanceClamp = Mathf.Lerp(0.005f, 0.12f, AdaptiveSharpness);
        jitterScale = TemporalJitterScale;
        AdaptiveResolve = 3000.0f;
        ctaaMat.SetFloat("_AntiShimmer", (AntiShimmerMode ? 1.0f : 0.0f));
        ctaaMat.SetVector("_delValues", delValues);
    }

    private static Material CreateMaterial(string shadername)
    {
        if (string.IsNullOrEmpty(shadername))
        {
            return null;
        }
        Material material = new Material(Shader.Find(shadername));
        material.hideFlags = HideFlags.HideAndDontSave;
        return material;
    }

    private static void DestroyMaterial(Material mat)
    {
        if (mat != null)
        {
            Object.DestroyImmediate(mat);
            mat = null;
        }
    }




    void Awake()
    {

        if (ctaaMat == null) ctaaMat = CreateMaterial("Hidden/CTAA_PC");
        if (mat_enhance == null) mat_enhance = CreateMaterial("Hidden/CTAA_Enhance_PC");

        firstFrame = true;
        swap = true;
        frameCounter = 0;

        SetCTAA_Parameters();
    }


    void OnEnable()
    {
        if (ctaaMat == null) ctaaMat = CreateMaterial("Hidden/CTAA_PC");
        if (mat_enhance == null) mat_enhance = CreateMaterial("Hidden/CTAA_Enhance_PC");

        firstFrame = true;
        swap = true;
        frameCounter = 0;

        SetCTAA_Parameters();

        Camera mcam = GetComponent<Camera>();
                
        mcam.depthTextureMode |= DepthTextureMode.Depth;
        mcam.depthTextureMode |= DepthTextureMode.MotionVectors;
                
    }


    public void ResetCTAA_CAM()
    {
        count = 0;
        moveActive = true;
    }


    void LateUpdate()
    {
        if (moveActive)
        {
            if (count < 2)
            {
                this.transform.position += new Vector3(0, 1.0f * speed, 0);
                count++;
            }
            else
            {
                if (count < 4)
                {
                    this.transform.position -= new Vector3(0, 1.0f * speed, 0);
                    count++;
                }
                else
                {
                    moveActive = false;
                }
            }

        }
    }



    private void OnDisable()
    {       
        if(ctaaMat != null) DestroyMaterial(ctaaMat);
        if (mat_enhance != null)  DestroyMaterial(mat_enhance);
        if (rtAccum0 != null)  DestroyImmediate(rtAccum0); rtAccum0 = null;
        if (rtAccum1 != null)  DestroyImmediate(rtAccum1); rtAccum1 = null;        
        if (afterPreEnhace != null) DestroyImmediate(afterPreEnhace); afterPreEnhace = null;

        GetComponent<Camera>().targetTexture = null;
        if (upScaleRT != null) DestroyImmediate(upScaleRT); upScaleRT = null;

        if (m_LayerRenderCam != null)
        {
            m_LayerRenderCam.targetTexture = null;
            Destroy(m_LayerRenderCam.gameObject);
        }

        if (m_LayerMaskCam != null)
        {
            m_LayerMaskCam.targetTexture = null;
            Destroy(m_LayerMaskCam.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (ctaaMat != null) DestroyMaterial(ctaaMat);
        if (mat_enhance != null) DestroyMaterial(mat_enhance);
        if (rtAccum0 != null) DestroyImmediate(rtAccum0); rtAccum0 = null;
        if (rtAccum1 != null) DestroyImmediate(rtAccum1); rtAccum1 = null;
        if (afterPreEnhace != null) DestroyImmediate(afterPreEnhace); afterPreEnhace = null;

        GetComponent<Camera>().targetTexture = null;
        if (upScaleRT != null) DestroyImmediate(upScaleRT); upScaleRT = null;

        if (m_LayerRenderCam != null)
        {
            m_LayerRenderCam.targetTexture = null;
            Destroy(m_LayerRenderCam.gameObject);
        }

        if (m_LayerMaskCam != null)
        {
            m_LayerMaskCam.targetTexture = null;
            Destroy(m_LayerMaskCam.gameObject);
        }
    }

    private int startResX;
    private int startResY;
    Camera m_LayerRenderCam;
    Camera m_LayerMaskCam;

    private void Start()
    {

        if (ExtendedFeatures)
        {
            if(SuperSampleMode == 0)//None
            {
                upscaleFactor = 1;
                resizeDownFactor = 1;
            }
            else if (SuperSampleMode == 1)//CinaSoft
            {
                upscaleFactor = 2;
                resizeDownFactor = 2;
            }
            else if (SuperSampleMode == 2)//CinaUltra
            {
                upscaleFactor = 2;
                resizeDownFactor = 1;
            }

            //--------------------------------------------------------------------------------------------------
            //Create Required RT's SuperResolution Mode
            //--------------------------------------------------------------------------------------------------
            Camera mcam = GetComponent<Camera>();

            startResX = Screen.width;
            startResY = Screen.height;
            int sizex = startResX * upscaleFactor;
            int sizey = startResY * upscaleFactor;

            if (((upScaleRT == null) || (upScaleRT.width != sizex)) || (upScaleRT.height != sizey))
            {
                Destroy(upScaleRT);
                upScaleRT = new RenderTexture(sizex, sizey, 0, RenderTextureFormat.ARGB32);
                //upScaleRT.hideFlags = HideFlags.HideAndDontSave;
                upScaleRT.filterMode = FilterMode.Bilinear;
                upScaleRT.wrapMode = TextureWrapMode.Repeat;
                upScaleRT.Create();
                GetComponent<Camera>().targetTexture = upScaleRT;
                //Debug.Log("SuperResolution Changed And Created" + upScaleRT.width + "," + upScaleRT.height);
               // Debug.Log("CTAA SuperResolution Updated");
            }


            //===================
            //Layer Mask System
            //===================
            if (!m_LayerMaskCam)
            {
                GameObject go = new GameObject("LayerMaskRenderCam");
                m_LayerMaskCam = go.AddComponent<Camera>();
                m_LayerMaskCam.CopyFrom(mcam);
                m_LayerMaskCam.transform.position = transform.position;
                m_LayerMaskCam.transform.rotation = transform.rotation;
                LayerMask someMask = ~0;

                m_LayerMaskCam.cullingMask = someMask;
                m_LayerMaskCam.depth = mcam.depth + 1;
                m_LayerMaskCam.clearFlags = CameraClearFlags.Depth;
                m_LayerMaskCam.depthTextureMode = DepthTextureMode.None;
                m_LayerMaskCam.targetTexture = null;
                m_LayerMaskCam.allowMSAA = false;
                m_LayerMaskCam.enabled = false;
                m_LayerMaskCam.renderingPath = RenderingPath.Forward;
                //go.hideFlags = HideFlags.HideAndDontSave;
            }

            //=====================
            //Layer Compose System
            //=====================
            if (!m_LayerRenderCam)
            {
                GameObject go = new GameObject("LayerRenderCam");
                m_LayerRenderCam = go.AddComponent<Camera>();
                m_LayerRenderCam.CopyFrom(mcam);

                m_LayerRenderCam.transform.position = transform.position;
                m_LayerRenderCam.transform.rotation = transform.rotation;
                m_LayerRenderCam.cullingMask = m_ExcludeLayers;
                m_LayerRenderCam.depth = mcam.depth + 1;
                m_LayerRenderCam.clearFlags = CameraClearFlags.Depth;
                m_LayerRenderCam.depthTextureMode = DepthTextureMode.None;
                m_LayerRenderCam.targetTexture = null;

                m_LayerRenderCam.gameObject.AddComponent<RenderPostCTAA>();

                RenderPostCTAA rpCTAA = m_LayerRenderCam.gameObject.GetComponent<RenderPostCTAA>();

                rpCTAA.ctaaPC = GetComponent<CTAA_PC>();
                rpCTAA.ctaaCamTransform = this.transform;
                rpCTAA.MaskRenderCam = m_LayerMaskCam.GetComponent<Camera>();
                rpCTAA.maskRenderShader = Shader.Find("Unlit/CtaaMaskRenderShader");
                rpCTAA.layerPostMat = new Material(Shader.Find("Hidden/CTAA_Layer_Post"));
                m_LayerRenderCam.enabled = true;
                rpCTAA.layerMaskingEnabled = m_LayerMaskingEnabled;
                //go.hideFlags = HideFlags.HideAndDontSave;

            }
            //===================
            //print("CTAA Extended Features Enabled");

            if(MSAA_Control)
                QualitySettings.antiAliasing = m_MSAA_Level;

        }
        else
        {
            print("CTAA Standard Mode Enabled");
            upscaleFactor = 1;
            resizeDownFactor = 1;

            if (MSAA_Control)
                QualitySettings.antiAliasing = m_MSAA_Level;
        }
        
    }

    

    void OnPreCull()
    {
        //----------------------
        //Rescale If Required
        //----------------------
        if (startResX != Screen.width || startResY != Screen.height)
        {
            //print("CTAA Detected Resolution Change: "+ Screen.width + ", " + Screen.height);

            if (ExtendedFeatures)
            {
                if (SuperSampleMode == 0)//None
                {
                    upscaleFactor = 1;
                    resizeDownFactor = 1;
                }
                else if (SuperSampleMode == 1)//CinaSoft
                {
                    upscaleFactor = 2;
                    resizeDownFactor = 2;
                }
                else if (SuperSampleMode == 2)//CinaUltra
                {
                    upscaleFactor = 2;
                    resizeDownFactor = 1;
                }

                //--------------------------------------------------------------------------------------------------
                //Verify Required RT's
                //--------------------------------------------------------------------------------------------------
                Camera mcam = GetComponent<Camera>();

                startResX = Screen.width;// mcam.pixelWidth;
                startResY = Screen.height;// mcam.pixelHeight;
                int sizex = startResX * upscaleFactor;
                int sizey = startResY * upscaleFactor;

                if (((upScaleRT == null) || (upScaleRT.width != sizex)) || (upScaleRT.height != sizey))
                {
                    Destroy(upScaleRT);
                    upScaleRT = new RenderTexture(sizex, sizey, 0, RenderTextureFormat.ARGB32);
                    //upScaleRT.hideFlags = HideFlags.HideAndDontSave;
                    upScaleRT.filterMode = FilterMode.Bilinear;
                    upScaleRT.wrapMode = TextureWrapMode.Repeat;
                    upScaleRT.Create();
                    GetComponent<Camera>().targetTexture = upScaleRT;
                    Debug.Log("CTAA SuperResolution Updated");
                    //Debug.Log("CTAA SuperResolution Updated" + upScaleRT.width + "," + upScaleRT.height);
                }

                moveActive = true;
                count = 0;
            }
        }
               

        if (CTAA_Enabled)
        {
            jitterCam();
        }
    }


    void jitterCam()
    {

        base.GetComponent<Camera>().ResetWorldToCameraMatrix();     // < ----- Unity 2017 Up
        base.GetComponent<Camera>().ResetProjectionMatrix();        // < ----- Unity 2017 Up

        base.GetComponent<Camera>().nonJitteredProjectionMatrix = base.GetComponent<Camera>().projectionMatrix;

        //base.GetComponent<Camera>().ResetWorldToCameraMatrix();	// < ----- Unity 5.6 
        //base.GetComponent<Camera>().ResetProjectionMatrix();      // < ----- Unity 5.6 

        Matrix4x4 matrixx = base.GetComponent<Camera>().projectionMatrix;
        float num = this.x_jit[this.frameCounter] * jitterScale;
        float num2 = this.y_jit[this.frameCounter] * jitterScale;
        matrixx.m02 += ((num * 2f) - 1f) / base.GetComponent<Camera>().pixelRect.width;
        matrixx.m12 += ((num2 * 2f) - 1f) / base.GetComponent<Camera>().pixelRect.height;
        this.frameCounter++;
        this.frameCounter = this.frameCounter % 16;
        base.GetComponent<Camera>().projectionMatrix = matrixx;

    }



   

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        if (CTAA_Enabled)
        {

            SetCTAA_Parameters();           
            

            if (((rtAccum0 == null) || (rtAccum0.width != (source.width / resizeDownFactor) )) || (rtAccum0.height != (source.height / resizeDownFactor)))
            {
                DestroyImmediate(rtAccum0);
                rtAccum0 = new RenderTexture(source.width / resizeDownFactor, source.height / resizeDownFactor, 0, source.format);
                rtAccum0.hideFlags = HideFlags.HideAndDontSave;
                rtAccum0.filterMode = FilterMode.Bilinear;
                rtAccum0.wrapMode = TextureWrapMode.Repeat;
            }

            if (((rtAccum1 == null) || (rtAccum1.width != (source.width / resizeDownFactor))) || (rtAccum1.height != (source.height / resizeDownFactor)))
            {
                DestroyImmediate(rtAccum1);
                rtAccum1 = new RenderTexture(source.width / resizeDownFactor, source.height / resizeDownFactor, 0, source.format);
                rtAccum1.hideFlags = HideFlags.HideAndDontSave;
                rtAccum1.filterMode = FilterMode.Bilinear;
                rtAccum1.wrapMode = TextureWrapMode.Repeat;
            }                        

            //-----------------------------------------------------------
            if (PreEnhanceEnabled)
            {
                if (((afterPreEnhace == null) || (afterPreEnhace.width != source.width)) || (afterPreEnhace.height != source.height))
                {
                    DestroyImmediate(afterPreEnhace);
                    afterPreEnhace = new RenderTexture(source.width, source.height, 0, source.format);
                    afterPreEnhace.hideFlags = HideFlags.HideAndDontSave;
                    afterPreEnhace.filterMode = FilterMode.Point;
                    afterPreEnhace.wrapMode = TextureWrapMode.Clamp;
                }

                mat_enhance.SetFloat("_AEXCTAA", 1.0f / (float)Screen.width);
                mat_enhance.SetFloat("_AEYCTAA", 1.0f / (float)Screen.height);
                mat_enhance.SetFloat("_AESCTAA", preEnhanceStrength);
                mat_enhance.SetFloat("_AEMAXCTAA", preEnhanceClamp);
                Graphics.Blit(source, afterPreEnhace, mat_enhance, 1);

                //-----------------------------------------------------------

                if (firstFrame)
                {
                    Graphics.Blit(afterPreEnhace, rtAccum0);
                    firstFrame = false;
                }

                ctaaMat.SetFloat("_AdaptiveResolve", AdaptiveResolve);
                ctaaMat.SetVector("_ControlParams", new Vector4(1.0f, (float)TemporalStability, HdrResponse, EdgeResponse));

                if (swap)
                {
                    ctaaMat.SetTexture("_Accum", rtAccum0);
                    Graphics.Blit(afterPreEnhace, rtAccum1, ctaaMat);
                    Graphics.Blit(rtAccum1, destination);
                }
                else
                {
                    ctaaMat.SetTexture("_Accum", rtAccum1);
                    Graphics.Blit(afterPreEnhace, rtAccum0, ctaaMat);
                    Graphics.Blit(rtAccum0, destination);
                }
                
                //-----------------------------------------------------------


            }
            else
            {               
                //-----------------------------------------------------------
                //No PreEnhance
                //-----------------------------------------------------------

                if (firstFrame)
                {
                    Graphics.Blit(source, rtAccum0);
                    firstFrame = false;
                }

                ctaaMat.SetFloat("_AdaptiveResolve", AdaptiveResolve);
                ctaaMat.SetVector("_ControlParams", new Vector4(1.0f, (float)TemporalStability, HdrResponse, EdgeResponse));

                if (swap)
                {
                    ctaaMat.SetTexture("_Accum", rtAccum0);
                    Graphics.Blit(source, rtAccum1, ctaaMat);
                    Graphics.Blit(rtAccum1, destination);
                }
                else
                {
                    ctaaMat.SetTexture("_Accum", rtAccum1);
                    Graphics.Blit(source, rtAccum0, ctaaMat);
                    Graphics.Blit(rtAccum0, destination);
                }                

                //-----------------------------------------------------------
            }           

            swap = !swap;

        }
        else
        {
           Graphics.Blit(source, destination);            
        }


    }


    public RenderTexture getCTAA_Render()
    {
        return upScaleRT;
    }

}
