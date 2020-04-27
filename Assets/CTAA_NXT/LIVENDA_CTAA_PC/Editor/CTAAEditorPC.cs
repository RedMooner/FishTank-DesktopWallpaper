//LIVENDA CTAA CINEMATIC TEMPORAL ANTI ALIASING
//Copyright Livenda Labs 2019
//CTAA-NXT V2.0

using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(CTAA_PC))]
public class CTAAEditorPC : Editor
{
	public Texture2D banner;    

    private GUIStyle back1;
	private GUIStyle back2;
	private GUIStyle back3;
	private GUIStyle back4;

	SerializedObject serObj;

	//int bannerHeight = 150;

    CTAA_PC myTarget;


    private Texture2D MakeTex(int width, int height, Color col)
	{
		Color[] pix = new Color[width*height];

		for(int i = 0; i < pix.Length; i++)
			pix[i] = col;

		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix); 
		result.Apply();
		result.hideFlags = HideFlags.HideAndDontSave;
		return result;
	}



	void OnEnable()
	{
		back1 = new GUIStyle();
		back1.normal.background = MakeTex(600, 1, new Color(0.2f, 0.2f, 0.1f, 0.3f));
        back2 = new GUIStyle();
		back2.normal.background = MakeTex(600, 1, new Color(0.3f, 0.3f, 0.4f, 0.2f));
        back3 = new GUIStyle();
		back3.normal.background = MakeTex(600, 1, new Color(0.4f, 0.3f, 0.5f, 0.4f));
		back4 = new GUIStyle();
		back4.normal.background = MakeTex(600, 1, new Color(0.1f, 0.0f, 0.5f, 0.3f));

		serObj = new SerializedObject(target);

		banner = Resources.Load("CTAA_nxt_background", typeof(Texture2D)) as Texture2D;
        
        myTarget = (CTAA_PC)target;

    }

	public override void OnInspectorGUI()
	{
		serObj.Update();

        if (!EditorApplication.isPlaying)
        {
           
            Color oldColor = GUI.backgroundColor;

            GUI.backgroundColor = Color.black;
            if (banner) GUILayout.Box(banner, GUILayout.Height(200), GUILayout.Width(200), GUILayout.ExpandWidth(true));
            GUI.backgroundColor = oldColor;

            GUILayout.BeginVertical(back2);

            var origFontStyle = EditorStyles.label.fontStyle;
            EditorStyles.label.fontStyle = FontStyle.Bold;
            myTarget.CTAA_Enabled = GUILayout.Toggle(myTarget.CTAA_Enabled, "ACTIVE");


            GUILayout.Space(10);

            

            myTarget.TemporalStability = EditorGUILayout.IntSlider(new GUIContent("TemporalStability", "Number of Frames to Blend for AA"), myTarget.TemporalStability, 3, 16);
            GUILayout.Space(5);
            myTarget.HdrResponse = EditorGUILayout.Slider(new GUIContent("HDR Response", "Anti-Aliasing Response and Strength for HDR Pixels"), myTarget.HdrResponse, 0.001f, 4.0f);
            GUILayout.Space(5);
            myTarget.EdgeResponse = EditorGUILayout.Slider(new GUIContent("Edge Response", "Amount of AA Blur in Geometric edges"), myTarget.EdgeResponse, 0.0f, 2.0f);
            GUILayout.Space(5);
            myTarget.AdaptiveSharpness = EditorGUILayout.Slider(new GUIContent("AdaptiveSharpness", "Amount of Automatic Sharpness added based on relative velocities"), myTarget.AdaptiveSharpness, 0.0f, 1.5f);
            GUILayout.Space(5);
            myTarget.TemporalJitterScale = EditorGUILayout.Slider(new GUIContent("JitterScale", "Size of sub-pixel Camera Jitter"), myTarget.TemporalJitterScale, 0.0f, 0.5f);
            GUILayout.Space(5);

            GUI.backgroundColor = new Color(0.3f, 0.3f, 0.34f, 0.4f);
            GUILayout.Box("Anti-Shimmer mode Eliminates Micro sub-pixel Shimmer - (No Dynamic Objects) Suitable for Architectural Visualisation, CAD, Engineering or non-moving objects however the Camera can be moved");
            GUI.backgroundColor = oldColor;

            myTarget.AntiShimmerMode = GUILayout.Toggle(myTarget.AntiShimmerMode, new GUIContent("Anti-Shimmer", "Eliminates Micro Shimmer - (No Dynamic Objects) Suitable for Architectural Visualisation, CAD, Engineering or non-moving objects. Camera can be moved"));
            GUILayout.Space(5);
            EditorStyles.label.fontStyle = origFontStyle;
            GUILayout.Space(10);
            
            GUILayout.EndVertical();

            GUILayout.BeginVertical(back2);
            
            GUILayout.Space(10);
            myTarget.ExtendedFeatures = GUILayout.Toggle(myTarget.ExtendedFeatures, "EXTENDED FEATURES");
            GUILayout.Space(10);
            

            EditorGUI.BeginDisabledGroup(myTarget.ExtendedFeatures == false);

            

            GUI.backgroundColor = new Color(0.3f, 0.3f, 0.34f, 0.4f);
            GUILayout.Box("Select The Layers To Exclude From CTAA. You Must Also UnSelect These Layers From MainCamera: This Layer selection method is ONLY to be used if using the new Super Sampling, please refer to the documentaion for Layer exclusion without super sampling");
            GUI.backgroundColor = oldColor;

            EditorStyles.label.fontStyle = FontStyle.Bold;

            GUILayout.Space(10);
            myTarget.m_LayerMaskingEnabled = GUILayout.Toggle(myTarget.m_LayerMaskingEnabled, new GUIContent("Layer Masking Enabled", "Enable or Disable Layer Masking"));
            //GUILayout.Space(10);

            GUILayout.Space(5);
            //if (myTarget.m_LayerMaskingEnabled)
            //{
            EditorGUI.BeginDisabledGroup(myTarget.m_LayerMaskingEnabled == false);
            var serializedObject = new SerializedObject(target);
                var property = serializedObject.FindProperty("m_ExcludeLayers");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property, true);
                serializedObject.ApplyModifiedProperties();
            EditorGUI.EndDisabledGroup();
            //}
            GUILayout.Space(10);

            //GUILayout.EndVertical();


            GUILayout.Box("", GUILayout.Height(2.0f), GUILayout.ExpandWidth(true));

            //GUILayout.BeginVertical(back1);

           // EditorGUILayout.TextArea("This is my text", GUI.skin.GetStyle("HelpBox"));
            GUILayout.Space(5);
            GUILayout.Label("SUPER SAMPLE SETTINGS", EditorStyles.boldLabel);
            EditorStyles.label.fontStyle = origFontStyle;
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();

            Color oldcol = GUI.color;
            GUI.color = Color.cyan;

            if (myTarget.SuperSampleMode == 0)
            {
                GUI.color = Color.green;
            }
            else
            {
                GUI.color = Color.grey;
            }            

            if (GUILayout.Button("Disabled"))
            {
                myTarget.SuperSampleMode = 0;
            }

            if (myTarget.SuperSampleMode == 1)
            {
                GUI.color = Color.green;
            }
            else
            {
                GUI.color = Color.grey;
            }

            if (GUILayout.Button("CinaSoft"))
            {
                myTarget.SuperSampleMode = 1;
            }

            if (myTarget.SuperSampleMode == 2)
            {
                GUI.color = Color.green;
            }
            else
            {
                GUI.color = Color.grey;
            }
            if (GUILayout.Button("CinaUltra"))
            {
                myTarget.SuperSampleMode = 2;
            }
            GUI.color = oldcol;

            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            myTarget.MSAA_Control = GUILayout.Toggle(myTarget.MSAA_Control, "ALLOW MSAA CONTROL");
            GUILayout.Space(5);

            //==========================
            //MSAA
            //==========================
            GUILayout.BeginHorizontal();

            oldcol = GUI.color;          


            if (myTarget.m_MSAA_Level == 0)
            {
                GUI.color = Color.green;
            }
            else
            {
                GUI.color = Color.grey;
            }
            if (GUILayout.Button("None"))
            {
                myTarget.m_MSAA_Level = 0;
            }


            if (myTarget.m_MSAA_Level == 2)
            {
                GUI.color = Color.green;
            }
            else
            {
                GUI.color = Color.grey;
            }
            if (GUILayout.Button("X2"))
            {
                myTarget.m_MSAA_Level = 2;
            }


            if (myTarget.m_MSAA_Level == 4)
            {
                GUI.color = Color.green;
            }
            else
            {
                GUI.color = Color.grey;
            }
            if (GUILayout.Button("X4"))
            {
                myTarget.m_MSAA_Level = 4;
            }


            if (myTarget.m_MSAA_Level == 8)
            {
                GUI.color = Color.green;
            }
            else
            {
                GUI.color = Color.grey;
            }
            if (GUILayout.Button("X8"))
            {
                myTarget.m_MSAA_Level = 8;
            }


            GUI.color = oldcol;

            GUILayout.EndHorizontal();
            //==========================

            EditorGUI.EndDisabledGroup();
            GUILayout.Space(10);
            GUILayout.EndVertical();

        }
        else
        {   
            //PLAY MODE
            Color oldColor = GUI.backgroundColor;

            GUI.backgroundColor = Color.black;
            if (banner) GUILayout.Box(banner, GUILayout.Height(200), GUILayout.Width(200), GUILayout.ExpandWidth(true));
            GUI.backgroundColor = oldColor;

            GUILayout.BeginVertical(back1);

            var origFontStyle = EditorStyles.label.fontStyle;
            EditorStyles.label.fontStyle = FontStyle.Bold;
            myTarget.CTAA_Enabled = GUILayout.Toggle(myTarget.CTAA_Enabled, "ACTIVE");

            GUILayout.Space(10);

            myTarget.TemporalStability = EditorGUILayout.IntSlider("TemporalStability", myTarget.TemporalStability, 3, 16);
            GUILayout.Space(5);
            myTarget.HdrResponse = EditorGUILayout.Slider("HDR Response", myTarget.HdrResponse, 0.001f, 4.0f);
            GUILayout.Space(5);
            myTarget.EdgeResponse = EditorGUILayout.Slider("Edge Response", myTarget.EdgeResponse, 0.0f, 2.0f);
            GUILayout.Space(5);
            myTarget.AdaptiveSharpness = EditorGUILayout.Slider("AdaptiveSharpness", myTarget.AdaptiveSharpness, 0.0f, 1.5f);
            GUILayout.Space(5);
            myTarget.TemporalJitterScale = EditorGUILayout.Slider("JitterScale", myTarget.TemporalJitterScale, 0.0f, 0.5f);
            GUILayout.Space(5);
            myTarget.AntiShimmerMode = GUILayout.Toggle(myTarget.AntiShimmerMode, "Anti-Shimmer");
            GUILayout.Space(5);
            EditorStyles.label.fontStyle = origFontStyle;
            GUILayout.Space(10);
            
            GUILayout.EndVertical();            

            //GUILayout.Space(5);
            GUILayout.BeginVertical(back1);
            GUILayout.Space(10);
            GUILayout.Box("Editing Extended Features Diabled During Playmode", GUILayout.ExpandWidth(true));
            GUILayout.Space(10);
            GUILayout.EndVertical();
        }
          

        serObj.ApplyModifiedProperties();
        
    }


}


