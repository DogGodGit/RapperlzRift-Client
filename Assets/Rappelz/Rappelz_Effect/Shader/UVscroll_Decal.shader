Shader "Zfx/Decal" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Mask ("Mask", 2D) = "white" {}
        _Distor ("Distor", 2D) = "white" {}
        [MaterialToggle] _UseAlpha ("UseAlpha", Float ) = 0
        [MaterialToggle] _useAlphaM ("useAlphaM", Float ) = 0
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

            Blend SrcAlpha OneMinusSrcAlpha
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
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform fixed _UseAlpha;
            uniform fixed _useAlphaM;
            struct VertexInput 
			{
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };

            struct VertexOutput 
			{
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
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

            float4 frag(VertexOutput i) : COLOR 
			{
////// Lighting:
////// Emissive:
                float4 node_9164 = _Time + _TimeEditor;
                float2 node_3203 = float2((i.uv1.r*node_9164.g),(i.uv1.g*node_9164.g));
                float2 node_4168 = ((node_3203*i.uv1.a)+i.uv0);
                float4 _Distor_var = tex2D(_Distor,TRANSFORM_TEX(node_4168, _Distor));
                float2 node_5041 = (node_3203+i.uv0+(_Distor_var.r*i.uv1.b));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_5041, _MainTex));
                float3 node_8048 = (lerp(dot(_MainTex_var.rgb,float3(0.3,0.59,0.11)),_MainTex_var.a,_UseAlpha)*(_MainTex_var.rgb*i.vertexColor.rgb*_MainTex_var.a*i.vertexColor.a*i.uv2.r));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float node_1192 = lerp(dot(_Mask_var.rgb,float3(0.3,0.59,0.11)),_Mask_var.a,_useAlphaM);
                float node_9432 = saturate((node_1192*4.0+-2.0));
                float3 emissive = (node_8048*node_9432);
                float3 finalColor = emissive;
                return fixed4(finalColor,(i.vertexColor.a*node_1192));
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
