// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Raindrop Wet Surface/Surface" {
	Properties {
		_ColorTex      ("Color", 2D) = "white" {}
		_HeightTex     ("Height", 2D) = "black" {}
		_BumpTex       ("Bump", 2D) = "bump" {}
		_RippleTex     ("Ripple", 2D) = "black" {}
		_MaskTex       ("Mask", 2D) = "black" {}
		_EnvTex        ("Environment", CUBE) = "black" {}
		_FloodLevel1   ("Flood Level 1", Range(0, 1)) = 0
		_FloodLevel2   ("Flood Level 2", Range(0, 2)) = 0.8
		_WetLevel      ("Wet Level", Range(0, 1)) = 0
		_Specular      ("Specular Intensity", Range(0, 1)) = 0.04
		_Refl          ("Reflection Intensity", Range(0, 1)) = 0.2
		_RippleDensity ("Ripple Density", Range(1, 16)) = 5
		_LightPosition ("Light Position", Vector) = (0, 0, 0, 0)
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		
		uniform sampler2D   _ColorTex, _HeightTex, _BumpTex, _RippleTex, _MaskTex;
		uniform float4      _ColorTex_ST;
		uniform samplerCUBE _EnvTex;
		uniform float       _FloodLevel1, _FloodLevel2, _WetLevel, _Specular, _Refl, _RippleDensity;
		uniform float3      _LightPosition;   // object space
		
		struct v2f
		{
        	float4 pos     : POSITION;
            float2 tex     : TEXCOORD0;
			float3 tgsview : TEXCOORD1;
			float3 tgslit  : TEXCOORD2;
        };
        v2f vert (appdata_tan v)
        {
			TANGENT_SPACE_ROTATION;
			float3 lit = _LightPosition - v.vertex.xyz;
			
        	v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
            o.tex = TRANSFORM_TEX(v.texcoord, _ColorTex);
			o.tgsview = mul(rotation, ObjSpaceViewDir(v.vertex));
			o.tgslit = mul(rotation, lit);
			return o;
       	}
		void DoWetShading (inout float3 albedo, inout float gloss, float wetLevel)
		{
			albedo *= lerp(1.0, 0.3, wetLevel);
			gloss = min(gloss * lerp(1.0, 2.5, wetLevel), 1.0);
		}
		float3 SampleEnvmap (float3 R, float gloss)
		{
//			return texCUBElod(_EnvTex, float4(R, (1.0 - gloss) * 8.0));
			return texCUBE(_EnvTex, R).rgb * gloss;
		}
       	float4 frag (v2f i) : SV_TARGET
		{
			float  h = tex2D(_HeightTex, i.tex).r;
			float3 albedo = tex2D(_ColorTex, i.tex).rgb;
			float  gloss = tex2D(_BumpTex, i.tex).a;
			float  mask = 1-tex2D(_MaskTex, i.tex).r;
			gloss = 1.0 - mask;			
			
			float2 waterLevel;
			waterLevel.x = min(_FloodLevel1, 1.0 - h);
			waterLevel.y = saturate((_FloodLevel2 - mask) / 0.5);
			float accumulatedWater = max(waterLevel.x, waterLevel.y);
   
			float3 rippleNormal = tex2D(_RippleTex, i.tex * _RippleDensity).xyz * 2 - 1;
			DoWetShading(albedo, gloss, saturate(_WetLevel + accumulatedWater));

			float3 N = UnpackNormal(tex2D(_BumpTex, i.tex));
			float3 V = normalize(i.tgsview);
			float3 L = normalize(i.tgslit);
			float3 H = normalize(L + V);
			
			N = lerp(N, rippleNormal, accumulatedWater);
			
			float dotVH = saturate(dot(V, H));
			float dotNH = saturate(dot(N, H));
			float dotNL = saturate(dot(N, L));
			float dotNV = saturate(dot(N, V));
			
			gloss = lerp(gloss, 1.0, accumulatedWater);
			float3 spec = lerp(_Specular, 0.02, accumulatedWater);

			float3 R = reflect(V, N);
			float3 reflColor = SampleEnvmap(R, gloss);
			float3 specVH = spec + (1.0 - spec) * pow(1.0 - dotVH, 5.0);
//			float3 specNV = spec + (1.0 - spec) * pow(1.0 - dotNV, 5.0) / (4.0 - 3.0 * gloss);
			float  specPower = exp2(gloss * 11);
			float3 diffuseLighting     = dotNL * albedo;
			float3 specularLighting    = specVH * ((specPower + 2.0) / 8.0) * pow(dotNH, specPower) * dotNL;
//			float3 ambientLighting = reflColor * specNV;
			float3 ambientLighting = reflColor * _Refl;
			float3 final = diffuseLighting + specularLighting + ambientLighting;
			return float4(final, 1.0);
		}
	ENDCG
	SubShader {
		Tags { "RenderType" = "Opaque" }
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
	FallBack "Diffuse"
}