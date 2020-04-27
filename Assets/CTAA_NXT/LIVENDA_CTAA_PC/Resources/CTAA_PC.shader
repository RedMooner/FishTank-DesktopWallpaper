//LIVENDA CTAA CINEMATIC TEMPORAL ANTI ALIASING

// CTAA NXT Cinematic Temporal Anti-Aliasing Copyright 2017-2020 Livenda Labs Pty Ltd 


Shader "Hidden/CTAA_PC" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	
}

SubShader {
	ZTest Always Cull Off ZWrite Off Fog { Mode Off }
	Pass {

CGPROGRAM
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#pragma glsl
#pragma exclude_renderers d3d11_9x
#include "UnityCG.cginc"

            
float4 _MainTex_TexelSize;

uniform sampler2D _MainTex;
uniform sampler2D _Accum;
uniform sampler2D _Motion0;
float _CamMotion;

uniform sampler2D _CameraDepthTexture;
uniform float _motionDelta;
uniform float _motionDeltaDynamic;
uniform float _AdaptiveResolve;

float4 _ControlParams;
sampler2D_half _CameraMotionVectorsTexture;

float _AntiShimmer;
float4 _delValues;


float DSJFDHSDKJD34SDF(float3 UEWRR46DRF5GGgfgFDFD)
{
	return (UEWRR46DRF5GGgfgFDFD.g * 2.0) + (UEWRR46DRF5GGgfgFDFD.r + UEWRR46DRF5GGgfgFDFD.b);
}

float DSJFDHSfdgfdgfgFDFD(float UEWRR46DRF5GGgfgFDFD, float UTTEWRR46DF5GGgfgFDFD) 
{
	return 4.0 * rcp(UEWRR46DRF5GGgfgFDFD * (-UTTEWRR46DF5GGgfgFDFD) + 1.0);
}

float DSJFDTETT35GGgfgFDFD(float UEWditFDYUEVTTEEDFBM, float UTTEWRR46DF5GGgfgFDFD) 
{
	return UEWditFDYUEVTTEEDFBM * DSJFDHSfdgfdgfgFDFD(UEWditFDYUEVTTEEDFBM, UTTEWRR46DF5GGgfgFDFD);
}

float DSJFDOPTRIJERH35GGgfgFDFD(float3 UEWRR46DRF5GGgfgFDFD) 
{
	
		
	
		return dot(UEWRR46DRF5GGgfgFDFD, float3(0.2126, 0.7152, 0.0722));
	
}

inline float DSJFWWRR46DF5GGgfgFDFD(float3 UEWRR46DRF5GGgfgFDFD, float UTTEWRR46DF5GGgfgFDFD) 
{
	
	
	return rcp(DSJFDHSDKJD34SDF(UEWRR46DRF5GGgfgFDFD) * UTTEWRR46DF5GGgfgFDFD + 4.0);
}

float UEWRfdgfgrRF5GGgfgFDFD(float3 UEWRR46DRF5GGgfgFDFD, float UTTEWRR46DF5GGgfgFDFD) 
{
	return rcp(UEWRR46DRF5GGgfgFDFD.g * UTTEWRR46DF5GGgfgFDFD + 1.0);
}

float UEWRfdgfgrRF5yyeGGgfgFDFD(float UEWRR46DRF5GGgfgFDFD, float UTTEWRR46DF5GGgfgFDFD) 
{
	return rcp(UEWRR46DRF5GGgfgFDFD * UTTEWRR46DF5GGgfgFDFD + 1.0);
}



float UEWRfdgfgrRF54GgfgFDFD(float UEWRR46DRF5GGgfgFDFD, float UTTEWRR46DF5GGgfgFDFD) 
{
	return rcp(UEWRR46DRF5GGgfgFDFD * UTTEWRR46DF5GGgfgFDFD + 4.0);
}


inline float UEWRfdgGgfgFDFDfdgdz(float3 UEWRR46DRF5GGgfgFDFD, float UTTEWRR46DF5GGgfgFDFD) 
{
	return 4.0 * rcp(DSJFDHSDKJD34SDF(UEWRR46DRF5GGgfgFDFD) * (-UTTEWRR46DF5GGgfgFDFD) + 1.0);
}

float UEWRfdgGgfgFDFEERT(float3 UEWRR46DRF5GGgfgFDFD, float UTTEWRR46DF5GGgfgFDFD) 
{
	return rcp(UEWRR46DRF5GGgfgFDFD.g * (-UTTEWRR46DF5GGgfgFDFD) + 1.0);
}



float UEWRfdgGgfgFDVVGED(float UEWRR46DRF5GGgfgFDFD, float UTTEWRR46DF5GGgfgFDFD) 
{
	return rcp(UEWRR46DRF5GGgfgFDFD * (-UTTEWRR46DF5GGgfgFDFD) + 1.0);
}





float UEWRfdgGgfgFDVVGEDfdgDFB(float UEWditFDYUEVTTEEDFBM, float UTTEWRR46DF5GGgfgFDFD) 
{
	return UEWditFDYUEVTTEEDFBM * UEWRfdgGgfgFDVVGED(UEWditFDYUEVTTEEDFBM, UTTEWRR46DF5GGgfgFDFD);
}


float UEWRfdgGgfgFDYUEVVGEDFB(float3 UEWRR46DRF5GGgfgFDFD, float UTTEWRR46DF5GGgfgFDFD) 
{
	float L = DSJFDHSDKJD34SDF(UEWRR46DRF5GGgfgFDFD);
	return L * UEWRfdgfgrRF54GgfgFDFD(L, UTTEWRR46DF5GGgfgFDFD);
}

float UEWRfdittgGgfgFDYUEVVGEDFB(float3 UEWRR46DRF5GGgfgFDFD, float UTTEWRR46DF5GGgfgFDFD) 
{
	return UEWRR46DRF5GGgfgFDFD.g * UEWRfdgfgrRF5yyeGGgfgFDFD(UEWRR46DRF5GGgfgFDFD.g, UTTEWRR46DF5GGgfgFDFD);
}



float UEWRfditFDYUEVTTEVDeEDFB(float UEWditFDYUEVTTEEDFBM) 
{
	return UEWditFDYUEVTTEEDFBM * rcp(1.0 + UEWditFDYUEVTTEEDFBM);
}
	
float UEWditFzxTTEEDFBM(float UEWditFDYUEVTTEEDFBM) 
{
	return UEWditFDYUEVTTEEDFBM * rcp(1.0 - UEWditFDYUEVTTEEDFBM);
}

float PerceptualLuma(float3 UEWRR46DRF5GGgfgFDFD, float UTTEWRR46DF5GGgfgFDFD) 
{
	return sqrt(UEWRfditFDYUEVTTEVDeEDFB(DSJFDOPTRIJERH35GGgfgFDFD(UEWRR46DRF5GGgfgFDFD) * UTTEWRR46DF5GGgfgFDFD));
}

float LinearLuma(float UEWditFDYUEVTTEEDFBM) 
{
	
	return UEWditFzxTTEEDFBM(UEWditFDYUEVTTEEDFBM * UEWditFDYUEVTTEEDFBM);
}


inline float UEWditFzxTTEEDFBMghfgfRT(float3 UEWditFzxTTEEDghfgfRTuyi, float3 UEWditFzxEEWWE, float3 UEWditFzxEEWWEJJJRF)
{
	if(min(min(abs(UEWditFzxTTEEDghfgfRTuyi.x), abs(UEWditFzxTTEEDghfgfRTuyi.y)), abs(UEWditFzxTTEEDghfgfRTuyi.z)) < (1.0/65536.0)) return 1.0;
	float3 RcpDir = rcp(UEWditFzxTTEEDghfgfRTuyi);
	float3 TNeg = (  UEWditFzxEEWWEJJJRF  - UEWditFzxEEWWE) * RcpDir;
	float3 TPos = ((-UEWditFzxEEWWEJJJRF) - UEWditFzxEEWWE) * RcpDir;
	return max(max(min(TNeg.x, TPos.x), min(TNeg.y, TPos.y)), min(TNeg.z, TPos.z));
}



inline float UEWditFzxEEWWLOJ(float3 UEWditFzRERTdWLOJ, float3 UEWditFzRERTffhgfgedWLOJ, float3 UEWditFzRERTffhgfgefgdWLOJ, float3 UEWditfhgfgedWLOJfgfg, float UEWditfhgfgeggfF)
{
	float3 Min = min(UEWditFzRERTffhgfgedWLOJ, min(UEWditFzRERTffhgfgefgdWLOJ, UEWditfhgfgedWLOJfgfg));
	float3 Max = max(UEWditFzRERTffhgfgedWLOJ, max(UEWditFzRERTffhgfgefgdWLOJ, UEWditfhgfgedWLOJfgfg));	

	float3 Avg2 = Max + Min;
	
	float3 UEWditFzxTTEEDghfgfRTuyi = UEWditFzRERTffhgfgedWLOJ - UEWditFzRERTdWLOJ;
	float3 UEWditFzxEEWWE = UEWditFzRERTdWLOJ - Avg2 * 0.5;
	float3 Scale = Max - Avg2 * UEWditfhgfgeggfF;
	return saturate(UEWditFzxTTEEDFBMghfgfRT(UEWditFzxTTEEDghfgfRTuyi, UEWditFzxEEWWE, Scale));	
}

float UEWditfhgfgeggfFoiytyt(float3 UEWRR46DRF5GGgfgFDFD, float UTTEWRR46DF5GGgfgFDFD) 
{
	return rcp(max(DSJFDOPTRIJERH35GGgfgFDFD(UEWRR46DRF5GGgfgFDFD) * UTTEWRR46DF5GGgfgFDFD, 1.0));
}

float4 UEWditfhgfgeggfFoiytytgfhF(float4 UEWditfhgfgeggyyfFoiytytgfhF, float4 UEWditfhgfgetrggyyfFoiytytgfhF, float UEWdifgetrggyyfFoiytfytgfhF, float UTTEWRR46DF5GGgfgFDFD) 
{
	float BlendA = (1.0 - UEWdifgetrggyyfFoiytfytgfhF) * UEWditfhgfgeggfFoiytyt(UEWditfhgfgeggyyfFoiytytgfhF.rgb, UTTEWRR46DF5GGgfgFDFD);
	float BlendB =        UEWdifgetrggyyfFoiytfytgfhF  * UEWditfhgfgeggfFoiytyt(UEWditfhgfgetrggyyfFoiytytgfhF.rgb, UTTEWRR46DF5GGgfgFDFD);
	float RcpBlend = rcp(BlendA + BlendB);
	BlendA *= RcpBlend;
	BlendB *= RcpBlend;
	return UEWditfhgfgeggyyfFoiytytgfhF * BlendA + UEWditfhgfgetrggyyfFoiytytgfhF * BlendB;
}




struct v2f {
	float4 pos : POSITION;
	float2 uv : TEXCOORD0;
};

v2f vert( appdata_img v )
{
	v2f o;
	o.pos = UnityObjectToClipPos (v.vertex);
	o.uv = v.texcoord.xy;

	return o;
}

float4 frag (v2f i) : COLOR
{



 // ------------------------------------------------

 float2 UEWdifgetyfFoiytfytgfhF;
 float2 UEWdifgetyffgghiiFoiytfytgfhF;
		
 float  UEWdghiiFoiytfytgfhFdfgd = 1;
  
 float2  UEWdghiiFoiytfytgfhFdfgdzgfD = _MainTex_TexelSize.xy;

 

 //###################################################
 
 
 float YNHHFDdghiiFfytgfhFdfgdzgfD = 1-Linear01Depth(tex2D (_CameraDepthTexture, i.uv).x);
 
 float YNHHFDdghiiFfyt754gdzgfD = 1;
 float2 YNHHFdnniFfyt754gdzgfD[4];
 
 YNHHFdnniFfyt754gdzgfD[0] = float2( -UEWdghiiFoiytfytgfhFdfgdzgfD.x, -UEWdghiiFoiytfytgfhFdfgdzgfD.y )*YNHHFDdghiiFfyt754gdzgfD;
 YNHHFdnniFfyt754gdzgfD[1] = float2(  UEWdghiiFoiytfytgfhFdfgdzgfD.x, -UEWdghiiFoiytfytgfhFdfgdzgfD.y )*YNHHFDdghiiFfyt754gdzgfD;
 YNHHFdnniFfyt754gdzgfD[2] = float2( -UEWdghiiFoiytfytgfhFdfgdzgfD.x,  UEWdghiiFoiytfytgfhFdfgdzgfD.y )*YNHHFDdghiiFfyt754gdzgfD;
 YNHHFdnniFfyt754gdzgfD[3] = float2(  UEWdghiiFoiytfytgfhFdfgdzgfD.x,  UEWdghiiFoiytfytgfhFdfgdzgfD.y )*YNHHFDdghiiFfyt754gdzgfD;
 
 float hDGFGHRHDFOPRgrRGH[4];
 hDGFGHRHDFOPRgrRGH[0] = 1-Linear01Depth(tex2D (_CameraDepthTexture, i.uv + YNHHFdnniFfyt754gdzgfD[0] ).x);
 hDGFGHRHDFOPRgrRGH[1] = 1-Linear01Depth(tex2D (_CameraDepthTexture, i.uv + YNHHFdnniFfyt754gdzgfD[1] ).x);
 hDGFGHRHDFOPRgrRGH[2] = 1-Linear01Depth(tex2D (_CameraDepthTexture, i.uv + YNHHFdnniFfyt754gdzgfD[2] ).x);
 hDGFGHRHDFOPRgrRGH[3] = 1-Linear01Depth(tex2D (_CameraDepthTexture, i.uv + YNHHFdnniFfyt754gdzgfD[3] ).x);
 
 int dIndx0;
 if(hDGFGHRHDFOPRgrRGH[0] > hDGFGHRHDFOPRgrRGH[1]) dIndx0 = 0;
 else dIndx0 = 1;
 
 int dIndx1;
 if(hDGFGHRHDFOPRgrRGH[2] > hDGFGHRHDFOPRgrRGH[3]) dIndx1 = 2;
 else dIndx1 = 3;
 
 int dIndx2;
 if(hDGFGHRHDFOPRgrRGH[dIndx0] > hDGFGHRHDFOPRgrRGH[dIndx1]) dIndx2 = dIndx0;
 else dIndx2 = dIndx1;
 
 //-----------------------------------
 int dIndx0C;
 if(hDGFGHRHDFOPRgrRGH[0] < hDGFGHRHDFOPRgrRGH[1]) dIndx0C = 0;
 else dIndx0C = 1;
 
 int dIndx1C;
 if(hDGFGHRHDFOPRgrRGH[2] < hDGFGHRHDFOPRgrRGH[3]) dIndx1C = 2;
 else dIndx1C = 3;
 
 int dIndx2C;
 if(hDGFGHRHDFOPRgrRGH[dIndx0C] < hDGFGHRHDFOPRgrRGH[dIndx1C]) dIndx2C = dIndx0C;
 else dIndx2C = dIndx1C;
 
 //-----------------------------------

 float2 hDGFGHRHDFOgfhPRgrRGH = float2(0,0);
 
 if( hDGFGHRHDFOPRgrRGH[dIndx2] > YNHHFDdghiiFfytgfhFdfgdzgfD)
 {
 	hDGFGHRHDFOgfhPRgrRGH = YNHHFdnniFfyt754gdzgfD[dIndx2];
 }

 

 //###################################################

 //Use Motion Vectors Unity
 float2 hDGFGHRHDfghmmFOgfhPRgrRGH = tex2D(_CameraMotionVectorsTexture, i.uv+hDGFGHRHDFOgfhPRgrRGH).rg;

 UEWdifgetyfFoiytfytgfhF =   hDGFGHRHDfghmmFOgfhPRgrRGH;

 //###################################################


 float hDGFhmmFOgfhPRgrRGH5fz = 1;
 float hDGFhmmFOgfhPGH5fzGD = saturate(abs(UEWdifgetyfFoiytfytgfhF.x) * hDGFhmmFOgfhPRgrRGH5fz + abs(UEWdifgetyfFoiytfytgfhF.y) * hDGFhmmFOgfhPRgrRGH5fz);
 	
	half2  uv = i.uv ;

	
					
	half4 hDGFhmmFO34sdgfhPGH5fzGD = tex2D(_MainTex, uv.xy - UEWdghiiFoiytfytgfhFdfgdzgfD );
	half4 hDGFhm34sdgfytyuhPGH5fzGD = tex2D(_MainTex, uv.xy + float2(  0, -UEWdghiiFoiytfytgfhFdfgdzgfD.y ) );
	half4 IKDSJHFGKJHEUIygEHJGFHG = tex2D(_MainTex, uv.xy + float2(  UEWdghiiFoiytfytgfhFdfgdzgfD.x, -UEWdghiiFoiytfytgfhFdfgdzgfD.y ) );
	half4 IKIFDHJrGKJHEUIygEHJGFHG = tex2D(_MainTex, uv.xy + float2(  -UEWdghiiFoiytfytgfhFdfgdzgfD.x, 0 ) );
	half4 IKIDFOIKfdJHEUIygEHJGFHG = tex2D(_MainTex, uv.xy);
	half4 IKIDFOIKfdJHEUgfht6dIygFHG = tex2D(_MainTex, uv.xy + float2(   UEWdghiiFoiytfytgfhFdfgdzgfD.x, 0 ) );
	half4 IKOIKfdJHEUgfht6dIfgygFHG = tex2D(_MainTex, uv.xy + float2( -UEWdghiiFoiytfytgfhFdfgdzgfD.x,  UEWdghiiFoiytfytgfhFdfgdzgfD.y ) );
	half4 IKOIKfdJHEUIfgygFHGghzF = tex2D(_MainTex, uv.xy + float2(  0,  UEWdghiiFoiytfytgfhFdfgdzgfD.y ) );
	half4 IKOIKfdJHEUIFHGghzFGFD = tex2D(_MainTex, uv.xy + UEWdghiiFoiytfytgfhFdfgdzgfD );

	    UEWdghiiFoiytfytgfhFdfgd = _ControlParams.z;
        		
		hDGFhmmFO34sdgfhPGH5fzGD.rgb *= DSJFWWRR46DF5GGgfgFDFD(hDGFhmmFO34sdgfhPGH5fzGD.rgb, UEWdghiiFoiytfytgfhFdfgd);
		hDGFhm34sdgfytyuhPGH5fzGD.rgb *= DSJFWWRR46DF5GGgfgFDFD(hDGFhm34sdgfytyuhPGH5fzGD.rgb, UEWdghiiFoiytfytgfhFdfgd);
		IKDSJHFGKJHEUIygEHJGFHG.rgb *= DSJFWWRR46DF5GGgfgFDFD(IKDSJHFGKJHEUIygEHJGFHG.rgb, UEWdghiiFoiytfytgfhFdfgd);
		IKIFDHJrGKJHEUIygEHJGFHG.rgb *= DSJFWWRR46DF5GGgfgFDFD(IKIFDHJrGKJHEUIygEHJGFHG.rgb, UEWdghiiFoiytfytgfhFdfgd);
		IKIDFOIKfdJHEUIygEHJGFHG.rgb *= DSJFWWRR46DF5GGgfgFDFD(IKIDFOIKfdJHEUIygEHJGFHG.rgb, UEWdghiiFoiytfytgfhFdfgd);
		IKIDFOIKfdJHEUgfht6dIygFHG.rgb *= DSJFWWRR46DF5GGgfgFDFD(IKIDFOIKfdJHEUgfht6dIygFHG.rgb, UEWdghiiFoiytfytgfhFdfgd);
		IKOIKfdJHEUgfht6dIfgygFHG.rgb *= DSJFWWRR46DF5GGgfgFDFD(IKOIKfdJHEUgfht6dIfgygFHG.rgb, UEWdghiiFoiytfytgfhFdfgd);
		IKOIKfdJHEUIfgygFHGghzF.rgb *= DSJFWWRR46DF5GGgfgFDFD(IKOIKfdJHEUIfgygFHGghzF.rgb, UEWdghiiFoiytfytgfhFdfgd);
		IKOIKfdJHEUIFHGghzFGFD.rgb *= DSJFWWRR46DF5GGgfgFDFD(IKOIKfdJHEUIFHGghzFGFD.rgb, UEWdghiiFoiytfytgfhFdfgd);
						
		half4 UEWditFzRERTffhgfgedWLOJ= 
			hDGFhmmFO34sdgfhPGH5fzGD * 0.0625 + 
			hDGFhm34sdgfytyuhPGH5fzGD * 0.125 +
			IKDSJHFGKJHEUIygEHJGFHG * 0.0625 +
			IKIFDHJrGKJHEUIygEHJGFHG * 0.125 +
			IKIDFOIKfdJHEUIygEHJGFHG * 0.25 +
			IKIDFOIKfdJHEUgfht6dIygFHG * 0.125 +
			IKOIKfdJHEUgfht6dIfgygFHG * 0.0625 +
			IKOIKfdJHEUIfgygFHGghzF * 0.125 +
			IKOIKfdJHEUIFHGghzFGFD * 0.0625;


						
			
		float4	 IKKfdJHEUIFfghdfr7HGghzFGFD = UEWditFzRERTffhgfgedWLOJ;	
		half4 IKKfdJHdfr7HGghzFGFDfgsfhD = min(min(hDGFhmmFO34sdgfhPGH5fzGD, IKDSJHFGKJHEUIygEHJGFHG), min(IKOIKfdJHEUgfht6dIfgygFHG, IKOIKfdJHEUIFHGghzFGFD));		
		half4 IKKfdJHdfr7HGghFDfgsfhDGRE = max(max(hDGFhmmFO34sdgfhPGH5fzGD, IKDSJHFGKJHEUIygEHJGFHG), max(IKOIKfdJHEUgfht6dIfgygFHG, IKOIKfdJHEUIFHGghzFGFD));		
		half4 UEWditFzRERTffhgfgefgdWLOJ = min(min(min(hDGFhm34sdgfytyuhPGH5fzGD, IKIFDHJrGKJHEUIygEHJGFHG), min(IKIDFOIKfdJHEUIygEHJGFHG, IKIDFOIKfdJHEUgfht6dIygFHG)), IKOIKfdJHEUIfgygFHGghzF);		
		half4 UEWditfhgfgedWLOJfgfg = max(max(max(hDGFhm34sdgfytyuhPGH5fzGD, IKIFDHJrGKJHEUIygEHJGFHG), max(IKIDFOIKfdJHEUIygEHJGFHG, IKIDFOIKfdJHEUgfht6dIygFHG)), IKOIKfdJHEUIfgygFHGghzF);		
		IKKfdJHdfr7HGghzFGFDfgsfhD = min(IKKfdJHdfr7HGghzFGFDfgsfhD, UEWditFzRERTffhgfgefgdWLOJ);
		IKKfdJHdfr7HGghFDfgsfhDGRE = max(IKKfdJHdfr7HGghFDfgsfhDGRE, UEWditfhgfgedWLOJfgfg);
	    UEWditFzRERTffhgfgefgdWLOJ = UEWditFzRERTffhgfgefgdWLOJ * 0.5 + IKKfdJHdfr7HGghzFGFDfgsfhD * 0.5;
		UEWditfhgfgedWLOJfgfg = UEWditfhgfgedWLOJfgfg * 0.5 + IKKfdJHdfr7HGghFDfgsfhDGRE * 0.5; 		
		float4 IKKfdJHdfr7HGgSDFGsfhDGRE = tex2D(_Accum, i.uv-UEWdifgetyfFoiytfytgfhF);	
			   IKKfdJHdfr7HGgSDFGsfhDGRE.rgb *= DSJFWWRR46DF5GGgfgFDFD(IKKfdJHdfr7HGgSDFGsfhDGRE.rgb, UEWdghiiFoiytfytgfhFdfgd);	
		float OJDDfdJHdfr7HGgSDFGsfGRE = DSJFDHSDKJD34SDF(UEWditFzRERTffhgfgefgdWLOJ.rgb);
		float OJDDfdJHdfgSDFGsfGRFGE = DSJFDHSDKJD34SDF(UEWditfhgfgedWLOJfgfg.rgb);
		float OJDDfdJHdfgSDFGRFGEhgj = DSJFDHSDKJD34SDF(IKKfdJHdfr7HGgSDFGsfhDGRE.rgb);
		float OJDDfdJHdfgSDFGRlkghFGEhgj = OJDDfdJHdfgSDFGsfGRFGE - OJDDfdJHdfr7HGgSDFGsfGRE;
				float2	UEWditfhgfgeggfF = lerp( float2(_delValues.x, _delValues.y), float2(_delValues.z, _delValues.w), saturate(length(UEWdifgetyfFoiytfytgfhF)*1000000) );
				
				if(_AntiShimmer < 0.5)
				{
				 UEWditfhgfgeggfF = float2(0.5, 1.0);
				}

		_ControlParams.y = _ControlParams.y * UEWditfhgfgeggfF.y ;

		float OJDDfdJHdfRlkghFGEhggferj = UEWditFzxEEWWLOJ(IKKfdJHdfr7HGgSDFGsfhDGRE.rgb, IKKfdJHEUIFfghdfr7HGghzFGFD.rgb, UEWditFzRERTffhgfgefgdWLOJ.rgb, UEWditfhgfgedWLOJfgfg.rgb, UEWditfhgfgeggfF.x);
			  OJDDfdJHdfRlkghFGEhggferj = saturate( OJDDfdJHdfRlkghFGEhggferj );
			  IKKfdJHdfr7HGgSDFGsfhDGRE.rgb = lerp(IKKfdJHdfr7HGgSDFGsfhDGRE.rgb, IKKfdJHEUIFfghdfr7HGghzFGFD.rgb, OJDDfdJHdfRlkghFGEhggferj );
		float OJDDfdJghFGEhggferjhgF = saturate(hDGFhmmFOgfhPGH5fzGD) * 0.5;
		float OJDDfdJHGVDferjhgFJHGfd =  _ControlParams.w;
		OJDDfdJghFGEhggferjhgF = saturate(OJDDfdJghFGEhggferjhgF + rcp(1.0 + OJDDfdJHdfgSDFGRlkghFGEhgj * OJDDfdJHGVDferjhgFJHGfd));
		UEWditFzRERTffhgfgedWLOJ.rgb = lerp(UEWditFzRERTffhgfgedWLOJ.rgb, IKIDFOIKfdJHEUIygEHJGFHG.rgb, OJDDfdJghFGEhggferjhgF);
		_ControlParams.y = _ControlParams.y* saturate(1-length(UEWdifgetyfFoiytfytgfhF*UEWdifgetyfFoiytfytgfhF)*90+1);
		float OJDDfdJHPPwDferjhgFJHGfd = (1.0/_ControlParams.y) + hDGFhmmFOgfhPGH5fzGD * (1.0/_ControlParams.y);
		float OJDDfdJHPPrjhgFJHGfdgzxf = OJDDfdJHdfgSDFGRFGEhgj * OJDDfdJHPPwDferjhgFJHGfd * (1.0 + hDGFhmmFOgfhPGH5fzGD * OJDDfdJHPPwDferjhgFJHGfd * 4.0);
		float OJDDfdJHPPrjhgFJHGfdgRezxf = saturate(OJDDfdJHPPrjhgFJHGfdgzxf * rcp(OJDDfdJHdfgSDFGRFGEhgj + OJDDfdJHdfgSDFGRlkghFGEhgj));
		float OJDDfdJHjhgFJHGfdhggRezxf = lerp(OJDDfdJHPPrjhgFJHGfdgRezxf, (sqrt(OJDDfdJHPPrjhgFJHGfdgRezxf)), saturate(length(UEWdifgetyfFoiytfytgfhF)*_AdaptiveResolve) );
		IKKfdJHdfr7HGgSDFGsfhDGRE = lerp(IKKfdJHdfr7HGgSDFGsfhDGRE, UEWditFzRERTffhgfgedWLOJ, OJDDfdJHjhgFJHGfdhggRezxf);
		IKKfdJHdfr7HGgSDFGsfhDGRE.rgb *= UEWRfdgGgfgFDFDfdgdz(IKKfdJHdfr7HGgSDFGsfhDGRE.rgb, UEWdghiiFoiytfytgfhFdfgd);
		IKKfdJHdfr7HGgSDFGsfhDGRE.rgb = -min(-IKKfdJHdfr7HGgSDFGsfhDGRE.rgb, 0.0);
	 	return IKKfdJHdfr7HGgSDFGsfhDGRE;
	 		
}
ENDCG
	}
}

Fallback off

}