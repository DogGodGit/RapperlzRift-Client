// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "FXMaker/Mask Additive Tint (No Fog)" 
{
	Properties 
	{
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Particle Texture", 2D) = "white" {}
	_Mask ("Mask", 2D) = "white" {}
}

	Category 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha One
		Cull Off Lighting Off ZWrite Off

		SubShader
		{
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityShaderVariables.cginc"
				#pragma multi_compile_fog
				#include "UnityCG.cginc"
				#define USING_FOG (defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2))

				// uniforms
				float4 _MainTex_ST;
				float4 _Mask_ST;

				// vertex shader input data
				struct appdata
				{
					float3 pos : POSITION;
					half4 color : COLOR;
					float3 uv0 : TEXCOORD0;
				};

				// vertex-to-fragment interpolators
				struct v2f 
				{
					fixed4 color : COLOR0;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
					float4 pos : SV_POSITION;
					UNITY_FOG_COORDS(2)
				};

				// vertex shader
				v2f vert (appdata IN) 
				{
					v2f o;
					o.color = saturate(IN.color);
					// compute texture coordinates
					o.uv0 = IN.uv0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					o.uv1 = IN.uv0.xy * _Mask_ST.xy + _Mask_ST.zw;
					// transform position
					o.pos = UnityObjectToClipPos(float4(IN.pos,1));
					// fog
					UNITY_TRANSFER_FOG(o,o.pos);
					return o;
				}

				// textures
				sampler2D _MainTex;
				sampler2D _Mask;
				fixed4 _TintColor;

				// fragment shader
				fixed4 frag (v2f IN) : SV_Target 
				{
					fixed4 col;
					//tex = tex2D (_MainTex, IN.uv0.xy);
					col = _TintColor * IN.color;
					// SetTexture #1
					col *= tex2D (_MainTex, IN.uv0.xy);
					// SetTexture #2
					col *= tex2D (_Mask, IN.uv1.xy);
					col *= 2;
					// fog
					UNITY_APPLY_FOG_COLOR(IN.fogCoord, col, fixed4(0,0,0,0));
					return col;
				}
				ENDCG
			}
		}
	}
}
