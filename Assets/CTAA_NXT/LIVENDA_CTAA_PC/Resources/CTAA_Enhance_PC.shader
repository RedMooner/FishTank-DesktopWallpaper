//LIVENDA CTAA - CINEMATIC TEMPORAL ANTI ALIASING
// CTAA NXT Cinematic Temporal Anti-Aliasing Copyright 2017-2020 Livenda Labs Pty Ltd 

Shader "Hidden/CTAA_Enhance_PC"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AEXCTAA ("Pixel Width", Float) = 1
		_AEYCTAA ("Pixel Height", Float) = 1 
		_AESCTAA ("Strength", Range(0, 5.0)) = 0.60
		_AEMAXCTAA ("Clamp", Range(0, 1.0)) = 0.05
	}

	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				half _AEXCTAA;
				half _AEYCTAA;
				half _AESCTAA;
				half _StrengthMAX;
				half _AEMAXCTAA;
			
				
				uniform sampler2D _Motion0;
				uniform float _motionDelta;
				sampler2D_half _CameraMotionVectorsTexture;

				uniform sampler2D _Motion0Dynamic;
				uniform float _motionDeltaDynamic;
				uniform float _AdaptiveEnhanceStrength;

				fixed4 frag(v2f_img i):COLOR
				{
					half2 coords = i.uv;
					half4 color = tex2D(_MainTex, coords);
					half4 original = color;
					
					/*
					float4 mo1 = tex2D(_Motion0, i.uv  );
 					float2 ssVel = ( mo1.xy * 2 -1 ) * mo1.z;
 					ssVel *=  _motionDelta;

 					float4 mo2 = tex2D(_Motion0Dynamic, i.uv  );
 					float2 ssVel2 = ( mo2.xy * 2 -1 ) * mo2.z;
 					ssVel2 *=  _motionDeltaDynamic;
					

 					ssVel += ssVel2;
					*/

					half2 motionJ = tex2D(_CameraMotionVectorsTexture, i.uv).rg;
					
					half4 blur  = tex2D(_MainTex, coords + half2(0.5 *  _AEXCTAA,       -_AEYCTAA));
						  blur += tex2D(_MainTex, coords + half2(      -_AEXCTAA, 0.5 * -_AEYCTAA));
						  blur += tex2D(_MainTex, coords + half2(       _AEXCTAA, 0.5 *  _AEYCTAA));
						  blur += tex2D(_MainTex, coords + half2(0.5 * -_AEXCTAA,        _AEYCTAA));
					blur /= 4;
					
					float delta = lerp(_AESCTAA, _StrengthMAX, saturate(length(motionJ)*_AdaptiveEnhanceStrength) );
					
					half4 lumaStrength = half4(0.2126, 0.7152, 0.0722, 0) * (delta) * 0.666;

					half4 sharp = color - blur;
					color += clamp(dot(sharp, lumaStrength), -_AEMAXCTAA, _AEMAXCTAA);

					return color; 
				}

			ENDCG
		}

		//=====================================================================

		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				sampler2D_half _CameraMotionVectorsTexture;
				half _AEXCTAA;
				half _AEYCTAA;
				half _AESCTAA;
				half _AEMAXCTAA;

				fixed4 frag(v2f_img i):COLOR
				{
					
					half2 motionJ = tex2D(_CameraMotionVectorsTexture, i.uv).rg;
					half2 coords = i.uv;
					half4 color = tex2D(_MainTex, coords);

					half4 blur  = tex2D(_MainTex, coords + half2(0.5 *  _AEXCTAA,       -_AEYCTAA));
						  blur += tex2D(_MainTex, coords + half2(      -_AEXCTAA, 0.5 * -_AEYCTAA));
						  blur += tex2D(_MainTex, coords + half2(       _AEXCTAA, 0.5 *  _AEYCTAA));
						  blur += tex2D(_MainTex, coords + half2(0.5 * -_AEXCTAA,        _AEYCTAA));
					blur /= 4;

					float delta = lerp(0.2, 1.2, saturate(length(motionJ)*500));

					half4 lumaStrength = half4(0.2126, 0.7152, 0.0722, 0)*(_AESCTAA*delta+delta);
					half4 sharp = color - blur;
					_AEMAXCTAA = 0.009;//// + delta;
					color += clamp(dot(sharp, lumaStrength), -_AEMAXCTAA, _AEMAXCTAA);

					return color;
				}

			ENDCG
		}

		//=====================================================================
	}

	FallBack off
}
