


///  CTAA  SPS  CTAA NXT V2 Copyright Livenda Labs Pty Ltd 2020

Shader "Hidden/CTAA301_SPS_SHADER" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}

	}

		SubShader{
		ZTest Always Cull Off ZWrite Off Fog{ Mode Off }
		Pass{

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
	half4 _MainTex_ST;
	uniform sampler2D _Accum;
	uniform sampler2D _Motion0;
	uniform float4 _Jitter;

	uniform sampler2D _CameraDepthTexture;
	uniform float _motionDelta;
	uniform float _AdaptiveResolve;

	float4 _ControlParams;

	float _RenderPath;

	sampler2D _CameraDepthNormalsTexture;


	uniform float4 _Sensitivity;
	uniform float _SampleDistance;

	inline half OJDJHGfdhggRezxfdsf(half2 centerNormal, float centerDepth, half4 theSample)
	{


		if (_RenderPath == 0)
		{
			///////// Deferred mode
			return 1;

			_Sensitivity.y = 0.0;
			_Sensitivity.x = 0.0;	//Depth

		}
		else {
			///////// Forward mode
			_Sensitivity.y = 1;
			_Sensitivity.x = 1;	//Depth
		}

		
		half2 diff = abs(centerNormal - theSample.xy) * _Sensitivity.y;
		int isSameNormal = (diff.x + diff.y) * _Sensitivity.y < 0.1;
		float sampleDepth = DecodeFloatRG(theSample.zw);
		float zdiff = abs(centerDepth - sampleDepth);
		int isSameDepth = zdiff * _Sensitivity.x < 0.09 * centerDepth;

		return isSameNormal * isSameDepth ? 1.0 : 0.0;


	}



	float OJDJHGfdhgghygjghRezxfdsf(float3 OJDJHGfghygjghRezjhDxfdsf)
	{
		return (OJDJHGfghygjghRezjhDxfdsf.g * 2.0) + (OJDJHGfghygjghRezjhDxfdsf.r + OJDJHGfghygjghRezjhDxfdsf.b);
	}

	
	inline float OJDJHGfghyezjhDxfdsfEEDFF(float3 OJDJHGfghygjghRezjhDxfdsf, float OJDJHGfghygjghretsezjhDxfdsf)
	{
		return rcp(OJDJHGfdhgghygjghRezxfdsf(OJDJHGfghygjghRezjhDxfdsf) * OJDJHGfghygjghretsezjhDxfdsf + 4.0);
	}

	float OJDJHGfghXCvjhDxfdsfEEDFF(float3 OJDJHGfghygjghRezjhDxfdsf, float OJDJHGfghygjghretsezjhDxfdsf)
	{
		return rcp(OJDJHGfghygjghRezjhDxfdsf.g * OJDJHGfghygjghretsezjhDxfdsf + 1.0);
	}

	float OJDJHGfghXCxfdsfEEDFFDF(float OJDJHGfghygjghRezjhDxfdsf, float OJDJHGfghygjghretsezjhDxfdsf)
	{
		return rcp(OJDJHGfghygjghRezjhDxfdsf * OJDJHGfghygjghretsezjhDxfdsf + 1.0);
	}



	float OJDJHGfghXCxfdEDFFDFGD(float OJDJHGfghygjghRezjhDxfdsf, float OJDJHGfghygjghretsezjhDxfdsf)
	{
		return rcp(OJDJHGfghygjghRezjhDxfdsf * OJDJHGfghygjghretsezjhDxfdsf + 4.0);
	}


	inline float OJDJHGfgfdEDFFDFGDfgafdg(float3 OJDJHGfghygjghRezjhDxfdsf, float OJDJHGfghygjghretsezjhDxfdsf)
	{
		return 4.0 * rcp(OJDJHGfdhgghygjghRezxfdsf(OJDJHGfghygjghRezjhDxfdsf) * (-OJDJHGfghygjghretsezjhDxfdsf) + 1.0);
	}

	float OJDJHGfgfdEDGDfgafdgFG(float3 OJDJHGfghygjghRezjhDxfdsf, float OJDJHGfghygjghretsezjhDxfdsf)
	{
		return rcp(OJDJHGfghygjghRezjhDxfdsf.g * (-OJDJHGfghygjghretsezjhDxfdsf) + 1.0);
	}

	float OJDfgafdgFGFFFDFD(float OJDJHGfghygjghRezjhDxfdsf, float OJDJHGfghygjghretsezjhDxfdsf)
	{
		return 4.0 * rcp(OJDJHGfghygjghRezjhDxfdsf * (-OJDJHGfghygjghretsezjhDxfdsf) + 1.0);
	}

	float OJDfgafdgFGFFFDFDDDffd(float OJDJHGfghygjghRezjhDxfdsf, float OJDJHGfghygjghretsezjhDxfdsf)
	{
		return rcp(OJDJHGfghygjghRezjhDxfdsf * (-OJDJHGfghygjghretsezjhDxfdsf) + 1.0);
	}



	float OJDfgafdgFGFFFDFDhghgDDffd(float OJDfgFGFFFDFDhghgDgfDffd, float OJDJHGfghygjghretsezjhDxfdsf)
	{
		return OJDfgFGFFFDFDhghgDgfDffd * OJDfgafdgFGFFFDFD(OJDfgFGFFFDFDhghgDgfDffd, OJDJHGfghygjghretsezjhDxfdsf);
	}


	float OJDfgFGFFFgDgfDffdFDFEeD(float OJDfgFGFFFDFDhghgDgfDffd, float OJDJHGfghygjghretsezjhDxfdsf)
	{
		return OJDfgFGFFFDFDhghgDgfDffd * OJDfgafdgFGFFFDFDDDffd(OJDfgFGFFFDFDhghgDgfDffd, OJDJHGfghygjghretsezjhDxfdsf);
	}


	float OJDfgDgfDffdFDFEeDDEERR(float3 OJDJHGfghygjghRezjhDxfdsf, float OJDJHGfghygjghretsezjhDxfdsf)
	{
		float L = OJDJHGfdhgghygjghRezxfdsf(OJDJHGfghygjghRezjhDxfdsf);
		return L * OJDJHGfghXCxfdEDFFDFGD(L, OJDJHGfghygjghretsezjhDxfdsf);
	}

	float OJDfgDgfDfgfgffdFDFEeDDEERR(float3 OJDJHGfghygjghRezjhDxfdsf, float OJDJHGfghygjghretsezjhDxfdsf)
	{
		return OJDJHGfghygjghRezjhDxfdsf.g * OJDJHGfghXCxfdsfEEDFFDF(OJDJHGfghygjghRezjhDxfdsf.g, OJDJHGfghygjghretsezjhDxfdsf);
	}

	float OJDffgDgfDfgfgffeDdfDEERRfd(float3 OJDJHGfghygjghRezjhDxfdsf)
	{
#if 1

		return dot(OJDJHGfghygjghRezjhDxfdsf, float3(0.299, 0.587, 0.114));
#else

		return dot(OJDJHGfghygjghRezjhDxfdsf, float3(0.2126, 0.7152, 0.0722));
#endif
	}

	float OJDffgDgfDfgDEERRfdggF(float OJDfgFGFFFDFDhghgDgfDffd)
	{
		return OJDfgFGFFFDFDhghgDgfDffd * rcp(1.0 + OJDfgFGFFFDFDhghgDgfDffd);
	}

	float OJDffgDgfDfgDEERgffRfdggFr(float OJDfgFGFFFDFDhghgDgfDffd)
	{
		return OJDfgFGFFFDFDhghgDgfDffd * rcp(1.0 - OJDfgFGFFFDFDhghgDgfDffd);
	}

	float OJDfffDfgDEERgffRfdggFrgfd(float3 OJDJHGfghygjghRezjhDxfdsf, float OJDJHGfghygjghretsezjhDxfdsf)
	{
		return sqrt(OJDffgDgfDfgDEERRfdggF(OJDffgDgfDfgfgffeDdfDEERRfd(OJDJHGfghygjghRezjhDxfdsf) * OJDJHGfghygjghretsezjhDxfdsf));
	}

	float OJDfDEERgffRfdggFFDergfd(float OJDfgFGFFFDFDhghgDgfDffd)
	{
		
		return OJDffgDgfDfgDEERgffRfdggFr(OJDfgFGFFFDFDhghgDgfDffd * OJDfgFGFFFDFDhghgDgfDffd);
	}


	inline float OJDfDEERfdggFFDergfdFrmn(float3 OJDfDttEERfdggFFDergfdFrmn, float3 OJDfDttEEy5RfdggFFDergfdFrmn, float3 OJDfDttEEy5trRfdggFFDergfdFrmn)
	{

		float3 EEy5trRfdgfgt5gFFDergfdfFrmn = rcp(OJDfDttEERfdggFFDergfdFrmn);
		float3 TNeg = (OJDfDttEEy5trRfdggFFDergfdFrmn - OJDfDttEEy5RfdggFFDergfdFrmn) * EEy5trRfdgfgt5gFFDergfdfFrmn;
		float3 TPos = ((-OJDfDttEEy5trRfdggFFDergfdFrmn) - OJDfDttEEy5RfdggFFDergfdFrmn) * EEy5trRfdgfgt5gFFDergfdfFrmn;
		return max(max(min(TNeg.x, TPos.x), min(TNeg.y, TPos.y)), min(TNeg.z, TPos.z));
	}

	inline float ghDGFGRrhgrRGRHRHB(float3 ghDGFGRfghrhgrRGRHRHB, float3 ghDGFGRfth5ghrhgrRGRHRHB, float3 ghDGFGRftr55th5ghrhgrHRHB, float3 ghDGFGRft55th5ghrhgtr4rHRHB)
	{
		float3 ghDGFGRf5ghrhgtr4rtrHRHB = min(ghDGFGRfth5ghrhgrRGRHRHB, min(ghDGFGRftr55th5ghrhgrHRHB, ghDGFGRft55th5ghrhgtr4rHRHB));
		float3 ghDGFGRf5ghrh6fghgtr4rRHB = max(ghDGFGRfth5ghrhgrRGRHRHB, max(ghDGFGRftr55th5ghrhgrHRHB, ghDGFGRft55th5ghrhgtr4rHRHB));
		float3 ghDGFGRf5ghrh6fguy5hgtr4rRHB = ghDGFGRf5ghrh6fghgtr4rRHB + ghDGFGRf5ghrhgtr4rtrHRHB;
		float3 OJDfDttEERfdggFFDergfdFrmn = ghDGFGRfth5ghrhgrRGRHRHB - ghDGFGRfghrhgrRGRHRHB;
		float3 OJDfDttEEy5RfdggFFDergfdFrmn = ghDGFGRfghrhgrRGRHRHB - ghDGFGRf5ghrh6fguy5hgtr4rRHB * 0.5;
		float3 Scale = ghDGFGRf5ghrh6fghgtr4rRHB - ghDGFGRf5ghrh6fguy5hgtr4rRHB * 0.5;
		return saturate(OJDfDEERfdggFFDergfdFrmn(OJDfDttEERfdggFFDergfdFrmn, OJDfDttEEy5RfdggFFDergfdFrmn, Scale));
	}

	float ghDGFGRf5guy5hgrttr4rRHB(float3 OJDJHGfghygjghRezjhDxfdsf, float OJDJHGfghygjghretsezjhDxfdsf)
	{
		return rcp(max(OJDffgDgfDfgfgffeDdfDEERRfd(OJDJHGfghygjghRezjhDxfdsf) * OJDJHGfghygjghretsezjhDxfdsf, 1.0));
	}

	float4 DRERGRGrrgERGGR4EDF(float4 DRERGRGrrgERGfghGR4EDF, float4 tRHHRGGGRGRFBRhg5fdg, float tRHHRGGGRGRfgzhFBRhg5fdg, float OJDJHGfghygjghretsezjhDxfdsf)
	{
		float tRHHGGzhFBRfgd4hg5fdg = (1.0 - tRHHRGGGRGRfgzhFBRhg5fdg) * ghDGFGRf5guy5hgrttr4rRHB(DRERGRGrrgERGfghGR4EDF.rgb, OJDJHGfghygjghretsezjhDxfdsf);
		float tRHHGGzhFBRfghgjd4hg5fdg4f = tRHHRGGGRGRfgzhFBRhg5fdg * ghDGFGRf5guy5hgrttr4rRHB(tRHHRGGGRGRFBRhg5fdg.rgb, OJDJHGfghygjghretsezjhDxfdsf);
		float tRHFBRfghgjd4hg5fdg4ffDG = rcp(tRHHGGzhFBRfgd4hg5fdg + tRHHGGzhFBRfghgjd4hg5fdg4f);
		tRHHGGzhFBRfgd4hg5fdg *= tRHFBRfghgjd4hg5fdg4ffDG;
		tRHHGGzhFBRfghgjd4hg5fdg4f *= tRHFBRfghgjd4hg5fdg4ffDG;
		return DRERGRGrrgERGfghGR4EDF * tRHHGGzhFBRfgd4hg5fdg + tRHHRGGGRGRFBRhg5fdg * tRHHGGzhFBRfghgjd4hg5fdg4f;
	}



	struct v2f {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
	};

	v2f vert(appdata_img v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;

		
		return o;
	}

	float4 frag(v2f i) : COLOR
	{


		
		// ------------------------------------------------
		float2 tRfghggfzj4ffDGFGHfg4G;


	float  fgDSGFGerrrGRGSDGSDEg = 0.1;



	float2  fgDSGFtyjuGerrrGRGSDGSDEg = _MainTex_TexelSize.xy*0.55;



	float fgDSGjuGerrrGRGSDGSDEg = 0.5;
	float2 fgDSGjuGrGRGSDGSDEgdfr[4];

	fgDSGjuGrGRGSDGSDEgdfr[0] = float2(-fgDSGFtyjuGerrrGRGSDGSDEg.x, -fgDSGFtyjuGerrrGRGSDGSDEg.y)*fgDSGjuGerrrGRGSDGSDEg;
	fgDSGjuGrGRGSDGSDEgdfr[1] = float2(fgDSGFtyjuGerrrGRGSDGSDEg.x, -fgDSGFtyjuGerrrGRGSDGSDEg.y)*fgDSGjuGerrrGRGSDGSDEg;
	fgDSGjuGrGRGSDGSDEgdfr[2] = float2(-fgDSGFtyjuGerrrGRGSDGSDEg.x,  fgDSGFtyjuGerrrGRGSDGSDEg.y)*fgDSGjuGerrrGRGSDGSDEg;
	fgDSGjuGrGRGSDGSDEgdfr[3] = float2(fgDSGFtyjuGerrrGRGSDGSDEg.x,  fgDSGFtyjuGerrrGRGSDGSDEg.y)*fgDSGjuGerrrGRGSDGSDEg;


	float2 fgDSGjuGrGRGSDEgdfrftgh = float2(0,0);
	
	float4 fgDSGjuGrGRGfgrSDEgdfrftgh = tex2D(_Motion0, i.uv);

	tRfghggfzj4ffDGFGHfg4G = (fgDSGjuGrGRGfgrSDEgdfrftgh.xy);

	float fgDSGjuGrSDEgdfrfDfftgh = 2;
	float fgDSGjuGEgdfrfDfftghfdZ = saturate(abs(tRfghggfzj4ffDGFGHfg4G.x) * fgDSGjuGrSDEgdfrfDfftgh + abs(tRfghggfzj4ffDGFGHfg4G.y) * fgDSGjuGrSDEgdfrfDfftgh);
	
	float2  uv = i.uv;


	float4 fgDSGjuGfghtEgdfrfDfftghfdZ = tex2D(_MainTex, uv.xy - fgDSGFtyjuGerrrGRGSDGSDEg);
	float4 DFGERGFGVfdffrfDfftghfdZ = tex2D(_MainTex, uv.xy + float2(0, -fgDSGFtyjuGerrrGRGSDGSDEg.y));
	float4 DGERHGFVrFGVfdffrfDfftghfdZ = tex2D(_MainTex, uv.xy + float2(fgDSGFtyjuGerrrGRGSDGSDEg.x, -fgDSGFtyjuGerrrGRGSDGSDEg.y));
	float4 RRfgDFefdffrfDfftghfdZ = tex2D(_MainTex, uv.xy + float2(-fgDSGFtyjuGerrrGRGSDGSDEg.x, 0));
	float4 RRfgDFefdffrdfrgfdfDfftghfdZ = tex2D(_MainTex, uv.xy);
	float4 RRfgDFefdfrgfdfDfftgdfhfdZ = tex2D(_MainTex, uv.xy + float2(fgDSGFtyjuGerrrGRGSDGSDEg.x, 0));
	float4 RRfgefdfrgfdGfdfDfftgdfhfdZ = tex2D(_MainTex, uv.xy + float2(-fgDSGFtyjuGerrrGRGSDGSDEg.x,  fgDSGFtyjuGerrrGRGSDGSDEg.y));
	float4 RRfgefdfrgyDFGghfdZ = tex2D(_MainTex, uv.xy + float2(0,  fgDSGFtyjuGerrrGRGSDGSDEg.y));
	float4 RRfgefdfrgyDFfgheGghfdZ = tex2D(_MainTex, uv.xy + fgDSGFtyjuGerrrGRGSDGSDEg);
	//half RRfgFGGrgheGghfdZ = (OJDffgDgfDfgfgffeDdfDEERRfd(tex2D(_MainTex, uv.xy + float2(0, -fgDSGFtyjuGerrrGRGSDGSDEg.y))));
	//half RRfgFGGrGFGghfdZ = (OJDffgDgfDfgfgffeDdfDEERRfd(tex2D(_MainTex, uv.xy + float2(0,  fgDSGFtyjuGerrrGRGSDGSDEg.y))));
	//half RRfgFGGrGfDGFGghfdZ = (OJDffgDgfDfgfgffeDdfDEERRfd(tex2D(_MainTex, uv.xy + float2(fgDSGFtyjuGerrrGRGSDGSDEg.x , 0))));
	//half RRfgFGGrGFGfDGFGghfdZ = (OJDffgDgfDfgfgffeDdfDEERRfd(tex2D(_MainTex, uv.xy + float2(-fgDSGFtyjuGerrrGRGSDGSDEg.x , 0))));
	//half RRfgFGFGfDGFGfg4fghfdZ = (OJDffgDgfDfgfgffeDdfDEERRfd(tex2D(_MainTex, uv.xy))) - (RRfgFGGrgheGghfdZ + RRfgFGGrGFGghfdZ + RRfgFGGrGfDGFGghfdZ + RRfgFGGrGFGfDGFGghfdZ)*0.25;
	half RRfgFGFGFGfg4fghffdgzgdZ = 0.05;//HARDCODE in for SPS V2.1 NXT
	fgDSGFGerrrGRGSDGSDEg = 2;
	fgDSGjuGfghtEgdfrfDfftghfdZ.rgb *= OJDJHGfghyezjhDxfdsfEEDFF(fgDSGjuGfghtEgdfrfDfftghfdZ.rgb, fgDSGFGerrrGRGSDGSDEg);
	DFGERGFGVfdffrfDfftghfdZ.rgb *= OJDJHGfghyezjhDxfdsfEEDFF(DFGERGFGVfdffrfDfftghfdZ.rgb, fgDSGFGerrrGRGSDGSDEg);
	DGERHGFVrFGVfdffrfDfftghfdZ.rgb *= OJDJHGfghyezjhDxfdsfEEDFF(DGERHGFVrFGVfdffrfDfftghfdZ.rgb, fgDSGFGerrrGRGSDGSDEg);
	RRfgDFefdffrfDfftghfdZ.rgb *= OJDJHGfghyezjhDxfdsfEEDFF(RRfgDFefdffrfDfftghfdZ.rgb, fgDSGFGerrrGRGSDGSDEg);
	RRfgDFefdffrdfrgfdfDfftghfdZ.rgb *= OJDJHGfghyezjhDxfdsfEEDFF(RRfgDFefdffrdfrgfdfDfftghfdZ.rgb, fgDSGFGerrrGRGSDGSDEg);
	RRfgDFefdfrgfdfDfftgdfhfdZ.rgb *= OJDJHGfghyezjhDxfdsfEEDFF(RRfgDFefdfrgfdfDfftgdfhfdZ.rgb, fgDSGFGerrrGRGSDGSDEg);
	RRfgefdfrgfdGfdfDfftgdfhfdZ.rgb *= OJDJHGfghyezjhDxfdsfEEDFF(RRfgefdfrgfdGfdfDfftgdfhfdZ.rgb, fgDSGFGerrrGRGSDGSDEg);
	RRfgefdfrgyDFGghfdZ.rgb *= OJDJHGfghyezjhDxfdsfEEDFF(RRfgefdfrgyDFGghfdZ.rgb, fgDSGFGerrrGRGSDGSDEg);
	RRfgefdfrgyDFfgheGghfdZ.rgb *= OJDJHGfghyezjhDxfdsfEEDFF(RRfgefdfrgyDFfgheGghfdZ.rgb, fgDSGFGerrrGRGSDGSDEg);
	

	float4 ghDGFGRfth5ghrhgrRGRHRHB =
		fgDSGjuGfghtEgdfrfDfftghfdZ * 0.0625 +
		DFGERGFGVfdffrfDfftghfdZ * 0.125 +
		DGERHGFVrFGVfdffrfDfftghfdZ * 0.0625 +
		RRfgDFefdffrfDfftghfdZ * 0.125 +
		RRfgDFefdffrdfrgfdfDfftghfdZ * 0.25 +
		RRfgDFefdfrgfdfDfftgdfhfdZ * 0.125 +
		RRfgefdfrgfdGfdfDfftgdfhfdZ * 0.0625 +
		RRfgefdfrgyDFGghfdZ * 0.125 +
		RRfgefdfrgyDFfgheGghfdZ * 0.0625;


	float4	 RRfgFGfg5gGfg4fghffdgzgdZ = ghDGFGRfth5ghrhgrRGRHRHB;
	float4 RRfgFGfg5ytytgGfg4fghffdgzgdZ = min(min(fgDSGjuGfghtEgdfrfDfftghfdZ, DGERHGFVrFGVfdffrfDfftghfdZ), min(RRfgefdfrgfdGfdfDfftgdfhfdZ, RRfgefdfrgyDFfgheGghfdZ));
	float4 RRfgFtytgGfg4fghffdgtfyhSzgdZ = max(max(fgDSGjuGfghtEgdfrfDfftghfdZ, DGERHGFVrFGVfdffrfDfftghfdZ), max(RRfgefdfrgfdGfdfDfftgdfhfdZ, RRfgefdfrgyDFfgheGghfdZ));
	float4 ghDGFGRftr55th5ghrhgrHRHB = min(min(min(DFGERGFGVfdffrfDfftghfdZ, RRfgDFefdffrfDfftghfdZ), min(RRfgDFefdffrdfrgfdfDfftghfdZ, RRfgDFefdfrgfdfDfftgdfhfdZ)), RRfgefdfrgyDFGghfdZ);
	float4 ghDGFGRft55th5ghrhgtr4rHRHB = max(max(max(DFGERGFGVfdffrfDfftghfdZ, RRfgDFefdffrfDfftghfdZ), max(RRfgDFefdffrdfrgfdfDfftghfdZ, RRfgDFefdfrgfdfDfftgdfhfdZ)), RRfgefdfrgyDFGghfdZ);
	RRfgFGfg5ytytgGfg4fghffdgzgdZ = min(RRfgFGfg5ytytgGfg4fghffdgzgdZ, ghDGFGRftr55th5ghrhgrHRHB);
	RRfgFtytgGfg4fghffdgtfyhSzgdZ = max(RRfgFtytgGfg4fghffdgtfyhSzgdZ, ghDGFGRft55th5ghrhgtr4rHRHB);
	ghDGFGRftr55th5ghrhgrHRHB = ghDGFGRftr55th5ghrhgrHRHB * 0.5 + RRfgFGfg5ytytgGfg4fghffdgzgdZ * 0.5;
	ghDGFGRft55th5ghrhgtr4rHRHB = ghDGFGRft55th5ghrhgtr4rHRHB * 0.5 + RRfgFtytgGfg4fghffdgtfyhSzgdZ * 0.5;	
	float4 Gfg4fghffdGRrgtfyhSzgdZ = tex2D(_Accum, i.uv - tRfghggfzj4ffDGFGHfg4G);	
	Gfg4fghffdGRrgtfyhSzgdZ.rgb *= OJDJHGfghyezjhDxfdsfEEDFF(Gfg4fghffdGRrgtfyhSzgdZ.rgb, fgDSGFGerrrGRGSDGSDEg);	
	float Gfg4fghffRrgty5ftfyhSzgdZ = OJDJHGfdhgghygjghRezxfdsf(ghDGFGRftr55th5ghrhgrHRHB.rgb);
	float Gfg4fghgty5ftfyhSzgdZfygtC = OJDJHGfdhgghygjghRezxfdsf(ghDGFGRft55th5ghrhgtr4rHRHB.rgb);
	float DFFGGhhgjhytfGfyudeDFer = OJDJHGfdhgghygjghRezxfdsf(Gfg4fghffdGRrgtfyhSzgdZ.rgb);
	float DFFGGhhgjhytfghfGfyudeDFer = Gfg4fghgty5ftfyhSzgdZfygtC - Gfg4fghffRrgty5ftfyhSzgdZ;
	float DFFGGhhgjhyfgrtfghfGfyudeDFer = ghDGFGRrhgrRGRHRHB(Gfg4fghffdGRrgtfyhSzgdZ.rgb, RRfgFGfg5gGfg4fghffdgzgdZ.rgb, ghDGFGRftr55th5ghrhgrHRHB.rgb, ghDGFGRft55th5ghrhgtr4rHRHB.rgb);	
	Gfg4fghffdGRrgtfyhSzgdZ.rgb = lerp(Gfg4fghffdGRrgtfyhSzgdZ.rgb, RRfgFGfg5gGfg4fghffdgzgdZ.rgb, DFFGGhhgjhyfgrtfghfGfyudeDFer);	
	float DFFGGhhgfGFRGRrfg = saturate(fgDSGjuGEgdfrfDfftghfdZ) * 0.5;
	float DFFGGhhgfGFRGRoif56rfg = 0;
	DFFGGhhgfGFRGRrfg = saturate(DFFGGhhgfGFRGRrfg + 1 / (1 + DFFGGhhgjhytfghfGfyudeDFer * DFFGGhhgfGFRGRoif56rfg));
	ghDGFGRfth5ghrhgrRGRHRHB.rgb = lerp(ghDGFGRfth5ghrhgrRGRHRHB.rgb, RRfgDFefdffrdfrgfdfDfftghfdZ.rgb, DFFGGhhgfGFRGRrfg);	
	float _TD = 1 / (RRfgFGFGFGfg4fghffdgzgdZ * 30 + _ControlParams.y);	
	float DFFGGhhgfGFRGRoif56uiFRrfg = (_TD + fgDSGjuGEgdfrfDfftghfdZ * _TD);	
	float DFFGGhhgfGF56uiFRrfgFR = DFFGGhhgjhytfGfyudeDFer * DFFGGhhgfGFRGRoif56uiFRrfg * (1.0 + fgDSGjuGEgdfrfDfftghfdZ * DFFGGhhgfGFRGRoif56uiFRrfg * 4.0);
	float DFFGGhhgfGF56uiFRrfgFFR = saturate(DFFGGhhgfGF56uiFRrfgFR * rcp(DFFGGhhgjhytfGfyudeDFer + DFFGGhhgjhytfghfGfyudeDFer));	
	float DFFGGhhgfGF56uiuyyFRrfgFFR = lerp(DFFGGhhgfGF56uiFRrfgFFR, (sqrt(DFFGGhhgfGF56uiFRrfgFFR)), saturate(length(tRfghggfzj4ffDGFGHfg4G)));
	Gfg4fghffdGRrgtfyhSzgdZ = lerp(Gfg4fghffdGRrgtfyhSzgdZ , ghDGFGRfth5ghrhgrRGRHRHB, DFFGGhhgfGF56uiuyyFRrfgFFR);
	Gfg4fghffdGRrgtfyhSzgdZ.rgb *= OJDJHGfgfdEDFFDFGDfgafdg(Gfg4fghffdGRrgtfyhSzgdZ.rgb, fgDSGFGerrrGRGSDGSDEg);
	Gfg4fghffdGRrgtfyhSzgdZ.rgb = -min(-Gfg4fghffdGRrgtfyhSzgdZ.rgb, 0.0);	
	return Gfg4fghffdGRrgtfyhSzgdZ;


	}
		ENDCG
	}
	}

		Fallback off

}