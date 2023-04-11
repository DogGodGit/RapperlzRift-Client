Shader "Shader Copy/DissolveSimple_Copy" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _ClipTex ("ClipTex", 2D) = "white" {}
        [MaterialToggle] _useAlpha ("useAlpha", Float ) = 0
        [MaterialToggle] _doubleSide ("doubleSide", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
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
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 

            uniform sampler2D _ClipTex; uniform float4 _ClipTex_ST;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
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
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float2 node_2451 = float2((i.uv0.b+i.uv1.r),(i.uv0.a+i.uv1.g));
                float4 _ClipTex_var = tex2D(_ClipTex,TRANSFORM_TEX(node_2451, _ClipTex));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float node_3605 = dot(_MainTex_var.rgb,float3(0.3,0.59,0.11));
                clip((i.vertexColor.a*((i.uv2.g*1.0+-0.5)+dot(_ClipTex_var.rgb,float3(0.3,0.59,0.11)))*lerp(node_3605,_MainTex_var.a,_useAlpha)*(isFrontFace+_doubleSide)) - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = (i.vertexColor.rgb*i.uv2.r*lerp(_MainTex_var.rgb,float3(node_3605,node_3605,node_3605),i.uv2.b));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
