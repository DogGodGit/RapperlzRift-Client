Shader "custom/Dissolve_Copy" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Distor ("Distor", 2D) = "white" {}
        [MaterialToggle] _useAlpha ("useAlpha", Float ) = 0
        [MaterialToggle] _doubleSide ("doubleSide", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
			//Tags 
			//{
			//    "LightMode"="ForwardBase"
			//}
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 

            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _Distor; uniform float4 _Distor_ST;
            uniform fixed _useAlpha;
            uniform fixed _doubleSide;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 uv2 : TEXCOORD2;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float2 node_7194 = float2(i.uv1.r,i.uv1.g);
                float2 node_3208 = (float2(i.uv0.b,i.uv0.a)+(i.uv1.a*node_7194));
                float4 _Distor_var = tex2D(_Distor,TRANSFORM_TEX(node_3208, _Distor));
                float2 node_9150 = ((i.uv1.b*_Distor_var.rgb.rg)+(i.uv0+(node_7194*i.uv2.a)));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_9150, _MainTex));
                float3 emissive = (_MainTex_var.rgb*i.vertexColor.rgb*i.vertexColor.a*i.uv2.r);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,(_MainTex_var.a*i.vertexColor.a*saturate((lerp( dot(_MainTex_var.rgb,float3(0.3,0.59,0.11)), _MainTex_var.a, _useAlpha )-(1.0 - i.uv2.g)))*(isFrontFace+_doubleSide)));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
