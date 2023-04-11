// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Triplebrick/OpacityDither"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MaskClipValue( "Mask Clip Value", Float ) = 0.1
		_Normal("Normal", 2D) = "bump" {}
		_Smoothness("Smoothness", 2D) = "white" {}
		_BasecolorOpacity("BasecolorOpacity", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPosition;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _BasecolorOpacity;
		uniform float4 _BasecolorOpacity_ST;
		uniform sampler2D _Smoothness;
		uniform float4 _Smoothness_ST;
		uniform float _MaskClipValue = 0.1;


		inline float Dither4x4Bayer( int x, int y )
		{
			const float dither[ 16 ] = {
				 1,  9,  3, 11,
				13,  5, 15,  7,
				 4, 12,  2, 10,
				16,  8, 14,  6 };
			int r = y * 4 + x;
			return (dither[r]-1) / 15;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			o.screenPosition = ase_screenPos;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_BasecolorOpacity = i.uv_texcoord * _BasecolorOpacity_ST.xy + _BasecolorOpacity_ST.zw;
			float4 tex2DNode5 = tex2D( _BasecolorOpacity, uv_BasecolorOpacity );
			o.Albedo = tex2DNode5.rgb;
			float2 uv_Smoothness = i.uv_texcoord * _Smoothness_ST.xy + _Smoothness_ST.zw;
			o.Smoothness = tex2D( _Smoothness, uv_Smoothness ).r;
			o.Alpha = 1;
			float2 clipScreen6 = ( i.screenPosition.xy / i.screenPosition.w ) * _ScreenParams.xy;
			float dither6 = Dither4x4Bayer( fmod(clipScreen6.x, 4), fmod(clipScreen6.y, 4) );
			clip( ( tex2DNode5.a * dither6 ) - _MaskClipValue );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=12001
0;92;1002;707;996.0984;448.3999;1.3;True;False
Node;AmplifyShaderEditor.SamplerNode;5;-391.7991,-267.7999;Float;True;Property;_BasecolorOpacity;BasecolorOpacity;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DitheringNode;6;-287.3993,246.5001;Float;False;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;12;-633.9987,346.6;Float;True;Property;_Smoothness;Smoothness;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;13;-597.2981,-43.59995;Float;True;Property;_Normal;Normal;1;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-158.0987,128.4;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;Standard;Triplebrick/OpacityDither;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.1;True;True;0;True;TransparentCutout;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;5;4
WireConnection;10;1;6;0
WireConnection;0;0;5;0
WireConnection;0;1;13;0
WireConnection;0;4;12;1
WireConnection;0;10;10;0
ASEEND*/
//CHKSM=45D3C54A8E59A5B170B39C4213A35C0B9D3D35D1