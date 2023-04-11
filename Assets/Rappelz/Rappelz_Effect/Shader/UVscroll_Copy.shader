Shader "Zfx/Distor2" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Clip ("Clip", 2D) = "white" {}
        _Distor ("Distor", 2D) = "white" {}
        [MaterialToggle] _UseAlpha ("UseAlpha", Float ) = 0
        [MaterialToggle] _FresnelInvert ("Fresnel Invert", Float ) = 0
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

            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _Distor; uniform float4 _Distor_ST;
            uniform sampler2D _Clip; uniform float4 _Clip_ST;
            uniform fixed _UseAlpha;
            uniform fixed _FresnelInvert;
            uniform fixed _doubleSide;
            struct VertexInput 
			{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };

            struct VertexOutput
			{
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float4 vertexColor : COLOR;
            };

            VertexOutput vert (VertexInput v) 
			{
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }

            float4 frag(VertexOutput i, float facing : VFACE) : COLOR 
			{
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float node_2701 = 0.0;
                float4 node_9164 = _Time + _TimeEditor;
                float2 node_3203 = float2((i.uv1.r*node_9164.g),(i.uv1.g*node_9164.g));
                float2 node_4168 = ((node_3203*i.uv1.a)+i.uv0);
                float4 _Distor_var = tex2D(_Distor,TRANSFORM_TEX(node_4168, _Distor));
                float node_3309 = (_Distor_var.r*i.uv1.b);
                float2 node_5041 = (node_3203+i.uv0+node_3309);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_5041, _MainTex));
                float2 node_2149 = (node_3309+float2(i.uv0.b,(i.uv0.a+i.uv2.g)));
                float4 _Clip_var = tex2D(_Clip,TRANSFORM_TEX(node_2149, _Clip));
                float node_7662 = saturate(pow(_Clip_var.r,i.uv2.b));
                float node_6435 = dot(_MainTex_var.rgb,float3(0.3,0.59,0.11));
                float node_9000 = lerp(node_6435,_MainTex_var.a,_UseAlpha);
                float node_7198 = pow(1.0-max(0,dot(normalDirection, viewDirection)),i.uv2.a);
                float node_3143 = 4.0;
                float _FresnelInvert_var = lerp( node_7198, pow((1.0 - pow(node_7198,node_3143)),node_3143), _FresnelInvert );
                float node_3771 = (isFrontFace+_doubleSide);
                float3 emissive = (lerp(float3(node_2701,node_2701,node_2701),((_MainTex_var.rgb*i.vertexColor.rgb*_MainTex_var.a*i.vertexColor.a*i.uv2.r)*node_7662*node_9000),_FresnelInvert_var)*node_3771);
                float3 finalColor = emissive;
                return fixed4(finalColor,(saturate(lerp(node_2701,(i.vertexColor.a*node_6435*node_7662*node_9000),_FresnelInvert_var))*node_3771));
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
