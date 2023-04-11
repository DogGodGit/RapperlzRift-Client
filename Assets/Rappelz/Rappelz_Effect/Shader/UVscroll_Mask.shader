Shader "Zfx/Distor" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Distor ("Distor", 2D) = "white" {}
        _Clip ("Clip", 2D) = "white" {}
        [MaterialToggle] _useAlpha ("useAlpha", Float ) = 0
        [MaterialToggle] _useAlphaM ("useAlphaM", Float ) = 0
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
            uniform sampler2D _Distor; uniform float4 _Distor_ST;
            uniform sampler2D _Clip; uniform float4 _Clip_ST;
            uniform fixed _useAlpha;
            uniform fixed _useAlphaM;
            uniform fixed _doubleSide;
            struct VertexInput
			{
                float4 vertex : POSITION;
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
                float4 vertexColor : COLOR;
            };

            VertexOutput vert (VertexInput v)
			{
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }

            float4 frag(VertexOutput i, float facing : VFACE) : COLOR
			{
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float2 node_1579 = float2((i.uv0.r+i.uv2.b),(i.uv0.g+i.uv2.a));
                float4 _Distor_var = tex2D(_Distor,TRANSFORM_TEX(node_1579, _Distor));
                float2 node_6702 = ((float2((i.uv0.b+i.uv1.r),(i.uv0.a+i.uv1.g))+i.uv1.a)+(_Distor_var.r*i.uv1.b));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_6702, _MainTex));
                float node_6435 = dot(_MainTex_var.rgb,float3(0.3,0.59,0.11));
                float node_4907 = lerp(node_6435,_MainTex_var.a,_useAlpha);
                float4 _Clip_var = tex2D(_Clip,TRANSFORM_TEX(i.uv0, _Clip));
                float node_3516 = lerp(dot(_Clip_var.rgb,float3(0.3,0.59,0.11)),_Clip_var.a,_useAlphaM);
                float node_9613 = (isFrontFace+_doubleSide);
                float3 emissive = ((_MainTex_var.rgb*i.vertexColor.rgb*i.vertexColor.a*i.uv2.r*_MainTex_var.a)*node_4907*node_3516*node_9613);
                float3 finalColor = emissive;
                return fixed4(finalColor,((i.vertexColor.a*node_6435*node_4907*node_3516)*node_9613));
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
