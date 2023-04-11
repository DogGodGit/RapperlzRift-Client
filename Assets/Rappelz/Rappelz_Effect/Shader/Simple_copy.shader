Shader "custom/Simple_Copy" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        [MaterialToggle] _doubleSide ("doubleSide", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"

            Blend One OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 

            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform fixed _doubleSide;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float node_9847 = dot(_MainTex_var.rgb,float3(0.3,0.59,0.11));
                float node_3121 = (isFrontFace+_doubleSide);
                float3 emissive = ((lerp(_MainTex_var.rgb,float3(node_9847,node_9847,node_9847),i.uv1.b)*i.vertexColor.rgb*i.uv1.r*i.vertexColor.a*_MainTex_var.a)*node_3121);
                float3 finalColor = emissive;
                return fixed4(finalColor,((lerp(node_9847,_MainTex_var.a,i.uv1.a)*i.uv1.g*_MainTex_var.a)*i.vertexColor.a*node_3121));
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
