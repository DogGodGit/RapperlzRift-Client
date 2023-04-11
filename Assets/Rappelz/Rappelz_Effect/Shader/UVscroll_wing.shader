Shader "custom/UVscroll_wing" {
    Properties {
        _Color ("Color", Color) = (0.5147059,0.5147059,0.5147059,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _Clip ("Clip", 2D) = "white" {}
        [MaterialToggle] _useAlpha ("useAlpha", Float ) = 0
        [MaterialToggle] _useAlphaM ("useAlphaM", Float ) = 1
        [MaterialToggle] _doubleSide ("doubleSide", Float ) = 0
        _Uspeed ("Uspeed", Float ) = 0.2
        _Vspeed ("Vspeed", Float ) = 0.2
        _Desaturate ("Desaturate", Float ) = 0
        _Emissive ("Emissive", Range(0, 10)) = 0
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

            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _Clip; uniform float4 _Clip_ST;
            uniform fixed _useAlpha;
            uniform fixed _useAlphaM;
            uniform fixed _doubleSide;
            uniform float _Uspeed;
            uniform float _Vspeed;
            uniform float _Desaturate;
            uniform float4 _Color;
            uniform float _Emissive;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_4158 = _Time + _TimeEditor;
                float2 node_8551 = (float2((_Uspeed*node_4158.g),(_Vspeed*node_4158.g))+i.uv0);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_8551, _MainTex));
                float node_6435 = dot(_MainTex_var.rgb,float3(0.3,0.59,0.11));
                float4 _Clip_var = tex2D(_Clip,TRANSFORM_TEX(i.uv0, _Clip));
                float node_3516 = lerp(dot(_Clip_var.rgb,float3(0.3,0.59,0.11)),_Clip_var.a,_useAlphaM);
                float node_9613 = (isFrontFace+_doubleSide);
                float3 emissive = (lerp(_MainTex_var.rgb,float3(node_6435,node_6435,node_6435),_Desaturate)*((_MainTex_var.rgb*i.vertexColor.rgb*i.vertexColor.a*_MainTex_var.a*_Emissive)*node_3516*node_9613)*_Color.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,((i.vertexColor.a*node_6435*lerp(node_6435,_MainTex_var.a,_useAlpha)*node_3516)*node_9613));
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
