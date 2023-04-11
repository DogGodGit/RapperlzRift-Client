
Shader "QC_TFT Studio/Rappelz/Custom Hero Simple(Renewal)" {
	Properties {
		_basetexture("Diffuse (RGB) Alpha (A)", 2D)	= "white" {}
		_normalmap	("Normal (RGB)", 2D) = "bump" {}
		_maskmap1	("Mask 1 (RGBA)", 2D) = "black" {}
		_maskmap2	("Mask 2 (RGBA)", 2D) = "black" {}
		
		_ambientscale	("Ambient Scale", Float) = 1.0

		_rimlightcolor ("Rim Light Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_rimlightscale ("Rim Light Scale", Float) = 1.0
		_rimlightblendtofull ("Rim Light Blend To Full", Range(0.0, 1.0)) = 0.0
		
		_specularcolor ("Specular Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_specularexponent ("Specular Exponent", Float) = 1.0
		_specularscale ("Specular Scale", Float) = 1.0
		_specularblendtofull ("Specular Blend To Full", Range(0.0, 1.0)) = 0.0
		
		_selfillumblendtofull ("Self-Illumination Blend To Full", Range(0.0, 1.0)) = 0.0
		
		_fresnelwarp ("Fresnel Warp (RGB)", 2D) = "black" {}
		_fresnelwarpblendtonone ("Fresnel Warp Blend To None", Range(0.0, 1.0)) = 0.0
		
		_envmap ("Environment (RGB)", Cube) = "white" {}
		_envmapintensity ("Environment Intensity", Float) = 0.0
		_maskenvbymetalness ("Mask Environment by Metalness",  Range(0.0, 1.0)) = 0.0
		
		_DissolveMap("Dissolve Map", 2D) = "white" {}
		_DissolveAmount("Dissolve Amount", Range(0, 1.0)) = 0.5
		//_DirectionMap ("Direction Map", 2D) = "white" {}
		_SubTex("Substitute Texture", 2D) = "white" {}
		[Toggle(_DISSOLVEGLOW_ON)] _DissolveGlow("Dissolve Glow", Int) = 1
		_GlowColor("Glow Color", Color) = (1,0.5,0,1)
		_GlowIntensity("Glow Intensity", Float) = 7
		//[Toggle(_EDGEGLOW_ON)] _EdgeGlow ("Edge Glow", Int) = 1
		[Toggle(_COLORBLENDING_ON)] _ColorBlending("Color Blending", Int) = 1
		_OuterEdgeColor("Outer Edge Color", Color) = (1,0,0,1)
		_InnerEdgeColor("Inner Edge Color", Color) = (1,1,0,1)
		_OuterEdgeThickness("Outer Edge Thickness", Range(0.0, 1.0)) = 0.02
		_InnerEdgeThickness("Inner Edge Thickness", Range(0.0, 1.0)) = 0.02
		[Toggle(_GLOWFOLLOW_ON)] _GlowFollow("Follow-Through", Int) = 0

		_EdgeColorRamp("Edge Color Ramp", 2D) = "white" {}
		[Toggle(_EDGECOLORRAMP_USE)] _UseEdgeColorRamp("Use Edge Color Ramp", Int) = 0

	}
	
	SubShader {
		//Tags { "Queue" = "Geometry"   "RenderType" = "Opaque" }
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf CustomHero noambient novertexlights nolightmap nodirlightmap keepalpha noforwardadd
		
			// Update: Edge glow is now a part of dissolve glow
			//#pragma shader_feature _EDGEGLOW_ON
		#pragma shader_feature _COLORBLENDING_ON
		#pragma shader_feature _EDGECOLORRAMP_USE
		#pragma shader_feature _DISSOLVEGLOW_ON
		#pragma shader_feature _NORMALMAP
			// Update: Direction maps are disabled for mobile shaders to improve performance
			// Directions should now be baked into the dissolve maps red channel directly
			//#pragma shader_feature _DIRECTIONMAP
		#pragma shader_feature _SUBMAP
		#pragma shader_feature _GLOWFOLLOW_ON

		#pragma multi_compile __ _DISSOLVEMAP

		#include "./DissolveMobileFunctions.cginc"

		#ifdef SHADER_API_OPENGL	
			#pragma glsl
		#endif

		#define _SIMPLE
		#pragma multi_compile _LINEAR_SPACE _GAMMA_SPACE
		
		// =======================================================================
		
		inline half3 Add(half3 srcColor, half3 dstColor)
		{ return srcColor + dstColor; }

		inline half3 Mod2x(half3 srcColor, half3 dstColor, half srcFactor)
		{
			srcColor *= 2.0;
			srcColor = lerp(float3(1.0, 1.0, 1.0), srcColor, srcFactor);
			return srcColor * dstColor;
		}

		inline half3 Overlay(half3 srcColor, half3 dstColor)
		{
			dstColor = saturate(dstColor);
			half3 check = step(float3(0.5, 0.5, 0.5), dstColor.rgb);
			half3 result = check * (half3(1.0, 1.0, 1.0) - ((half3(1.0, 1.0, 1.0) - 2.0 * (dstColor.rgb - 0.5)) * (1.0 - srcColor.rgb))); 
			result += (1.0 - check) * (2.0 * dstColor.rgb) * srcColor.rgb;
			return result;
		}

		inline float3 toLinear(float3 srcColor)
		{ return pow(srcColor, 2.2); }

		inline float3 toGamma(float3 srcColor)
		{ return pow(srcColor, 1 / 2.2); }

		// =======================================================================

		// =======================================================================

		half		_ambientscale;
		sampler2D	_basetexture;
		sampler2D	_normalmap;
		sampler2D	_maskmap1;
		sampler2D	_maskmap2;

		sampler2D	_diffusewarp;
		half		_diffusewarpblendtofull;
	
		sampler2D	_specularwarp;
		half		_specularwarpintensity;
	
		sampler3D	_fresnelcolorwarp;
		half		_fresnelcolorwarpblendtonone;
	
		sampler3D	_colorwarp;
		half		_colorwarpblendtonone;

		half		_rimlightscale;
		half		_rimlightblendtofull;
		half3		_rimlightcolor;

		half		_selfillumblendtofull;

		half		_specularexponent;
		half		_specularblendtofull;
		half3		_specularcolor;
		half		_specularscale;

		half		_maskenvbymetalness;
		samplerCUBE	_envmap;
		half		_envmapintensity;

		sampler2D	_fresnelwarp;
		half		_fresnelwarpblendtonone;

		struct Input {
			half2 uv_basetexture;
			half3 worldRefl;
			half3 worldNormal;
			INTERNAL_DATA
		};

		struct CustomHeroOutput {
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half3 Specular;	// Used for Environment
			half Alpha;
			half4 Mask1;
			half4 Mask2;
			half3 WorldNormal;
		};

		// =======================================================================

		// =======================================================================

		half4 LightingCustomHero(CustomHeroOutput o, half3 lightDir, half3 viewDir, half atten) {
			half4 color = half4(0.0, 0.0, 0.0, 0.0);
	
			#ifndef USING_DIRECTIONAL_LIGHT
    			lightDir = normalize(lightDir);
			#endif
			viewDir = normalize(viewDir);
  	
  			/////////////// Useful stuff
			half NdotL		= saturate(dot(o.Normal, lightDir));
	
			/////////////// Light color
			half3 lightColor = _LightColor0.rgb;
	
			/////////////// Shadows
			half attenuation = atten;
			if (0.0 != _WorldSpaceLightPos0.w)
			{ attenuation = atten * NdotL; }
	
			/////////////// Half Lambert
			half halfLambert = NdotL * 0.5 + 0.5;
		//	halfLambert = lightColor * halfLambert * atten;
	
			/////////////// Fresnel Warp and Fresnel
			half3 fresnel = pow(1.0 - saturate(dot(o.Normal, viewDir)), 5.0);
			fresnel.b = 1.0 - fresnel.b;
			fresnel = lerp(fresnel, tex2D(_fresnelwarp, half2(saturate(dot(o.Normal, viewDir)), 0)), _fresnelwarpblendtonone);
	
			/////////////// Diffuse lighting
			half3 diffuselighting = half3(1.0, 1.0, 1.0);
			/////////////// Diffuse warp
			half3 diffusewarp = tex2D(_diffusewarp, half2(halfLambert, halfLambert));
			diffuselighting = lerp(half3(halfLambert, halfLambert, halfLambert), diffusewarp, max(o.Mask1.g, _diffusewarpblendtofull));
			diffuselighting *= lightColor;
			diffuselighting *= attenuation;
			// Spherical Harmonics and Ambient Light
			half3 sphericalharmonics = ShadeSH9(float4(o.WorldNormal, 1.0));
			if (0.0 != _WorldSpaceLightPos0.w)	// This is a point light
			{
				#ifndef UNITY_PASS_FORWARDBASE
					sphericalharmonics = 0;
				#endif
			}
			diffuselighting += sphericalharmonics * _ambientscale;
	
			/////////////// Specular lighting
			/////////////// Specular warp
			half3 reflectionVector = normalize(2.0 * o.Normal * halfLambert - lightDir);
			half r = max(0, dot(reflectionVector, viewDir));
			half3 specularlighting = pow(r, o.Mask2.a * _specularexponent);
			half3 specularlightingwarp = tex2D(_specularwarp, half2(pow(r, 1), 1.0 - o.Mask2.a));
			specularlighting = lerp(specularlighting, specularlightingwarp, _specularwarpintensity) * lightColor;
	
			/////////////// Fresnel Color Warp
			o.Albedo = lerp(o.Albedo, tex3D(_fresnelcolorwarp, o.Albedo), o.Mask2.g * fresnel.g * _fresnelcolorwarpblendtonone);
	
			/////////////// Diffuse
			/////////////// Color Warp
			half3 diffuse = lerp(o.Albedo, tex3D(_colorwarp, o.Albedo), _colorwarpblendtonone);
			diffuse *= diffuselighting;
			color.rgb = diffuse;
	
			/////////////// Specular
			half3 specular = specularlighting * _specularscale;
			#ifdef _GAMMA_SPACE
				o.Mask2.r = toLinear(o.Mask2.r);
			#endif
			specular *= max(o.Mask2.r, _specularblendtofull);
			///////////// Tint by Color and Metalness
			specular *= lerp(diffuse + o.Mask1.b, _specularcolor, o.Mask2.b)
						* fresnel.b
						* NdotL;
			specular *= attenuation;
			color.rgb += specular;
	
			///////////// Metalness
			half3 metalness = lerp(color.rgb, specular,  o.Mask1.b);
			color.rgb = metalness;
	
			///////////// Rim Light
			half3 rimlight = max(o.Mask2.g, _rimlightblendtofull)
								 * fresnel.r
								 * _rimlightscale
								 * _rimlightcolor
								 * saturate(dot(UNITY_MATRIX_V[1], o.WorldNormal));
			if (0.0 != _WorldSpaceLightPos0.w)
			{ rimlight *= atten; }
			color.rgb += rimlight;
	
			///////////// Environement
			half3 environment = lerp(o.Specular * _envmapintensity * o.Mask2.r, o.Specular * _envmapintensity * o.Mask1.b, _maskenvbymetalness);
			if (0.0 != _WorldSpaceLightPos0.w)
			{
				#ifndef UNITY_PASS_FORWARDBASE
					environment *= attenuation;
				#endif
			}
			color.rgb += environment;
	
			/////////////// Alpha
			color.a = o.Alpha;
	
			#ifdef _GAMMA_SPACE
				color.rgb = toGamma(color.rgb);
			#endif
			return color;
		}	

		// =======================================================================

		// =======================================================================

		void surf(Input IN, inout CustomHeroOutput o) {
			
			/////////////// Mask1
			o.Mask1 = tex2D(_maskmap1, IN.uv_basetexture);

			/////////////// Mask2
			o.Mask2 = tex2D(_maskmap2, IN.uv_basetexture);

			/////////////// Albedo
			half4 basetexture = tex2D(_basetexture, IN.uv_basetexture);
			fixed4 color = basetexture;
			o.Albedo = basetexture.rgb;
			#ifdef _GAMMA_SPACE
				o.Albedo = toLinear(o.Albedo);
			#endif

			/////////////// Normals
			o.Normal = UnpackNormal(tex2D(_normalmap, IN.uv_basetexture));
			o.WorldNormal = WorldNormalVector(IN, o.Normal);

			/////////////// Environement
			o.Specular = texCUBE(_envmap, WorldReflectionVector (IN, o.Normal)).rgb;
			#ifdef _LINEAR_SPACE
				o.Specular = toLinear(o.Specular);
			#endif

			/////////////// Self Illumination
			o.Emission = max(o.Mask1.a, _selfillumblendtofull) * o.Albedo;


			#ifdef _DISSOLVEMAP
			#if defined(_COLORBLENDING_ON) || defined(_EDGECOLORRAMP_USE)
				fixed totalThickness = _InnerEdgeThickness;
			#else
				fixed totalThickness = _InnerEdgeThickness + _OuterEdgeThickness;
			#endif
				fixed dm = tex2D(_DissolveMap, IN.uv_basetexture).r;
				fixed d = dm;
				d -= _DissolveAmount * 0.8;
				color.rgb = AddEdgeColor(basetexture, d, totalThickness);

			#ifdef _SUBMAP
				AddSubMap(IN.uv_basetexture, color.rgb, d);
			#else
				color *= (0 < d);
			#endif

			#ifdef _DISSOLVEGLOW_ON
				o.Emission = o.Emission * GetDissolveGlow(dm, d, totalThickness, color.rgb);
			#endif
			#endif

			o.Albedo = color;
			o.Alpha = color.a;
		}

		// =======================================================================

		ENDCG
	}
	FallBack "Diffuse"
}
