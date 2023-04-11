// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.36 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.36;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32716,y:32678,varname:node_4795,prsc:2|emission-865-OUT,alpha-865-OUT,refract-2859-OUT;n:type:ShaderForge.SFN_Tex2d,id:7910,x:32164,y:32895,ptovrint:False,ptlb:Main,ptin:_Main,varname:node_7910,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True|UVIN-7210-OUT;n:type:ShaderForge.SFN_ComponentMask,id:7164,x:32351,y:32895,varname:node_7164,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-7910-RGB;n:type:ShaderForge.SFN_Multiply,id:2859,x:32544,y:32978,varname:node_2859,prsc:2|A-7164-OUT,B-3484-A,C-5452-U,D-9113-OUT;n:type:ShaderForge.SFN_VertexColor,id:3484,x:32349,y:32744,varname:node_3484,prsc:2;n:type:ShaderForge.SFN_Vector1,id:865,x:32485,y:32680,varname:node_865,prsc:2,v1:0;n:type:ShaderForge.SFN_TexCoord,id:5452,x:32351,y:33083,varname:node_5452,prsc:2,uv:2,uaff:True;n:type:ShaderForge.SFN_Vector1,id:9113,x:32351,y:33243,varname:node_9113,prsc:2,v1:0.1;n:type:ShaderForge.SFN_TexCoord,id:9550,x:31819,y:32769,varname:node_9550,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:7210,x:31986,y:32895,varname:node_7210,prsc:2|A-9550-UVOUT,B-8277-OUT;n:type:ShaderForge.SFN_TexCoord,id:4373,x:31461,y:32895,varname:node_4373,prsc:2,uv:1,uaff:True;n:type:ShaderForge.SFN_Time,id:8490,x:31461,y:33046,varname:node_8490,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4467,x:31653,y:32895,varname:node_4467,prsc:2|A-4373-U,B-8490-T;n:type:ShaderForge.SFN_Multiply,id:1604,x:31653,y:33046,varname:node_1604,prsc:2|A-4373-V,B-8490-T;n:type:ShaderForge.SFN_Append,id:8277,x:31819,y:32942,varname:node_8277,prsc:2|A-4467-OUT,B-1604-OUT;proporder:7910;pass:END;sub:END;*/

Shader "Shader Copy/Distortion_Copy" {
    Properties {
        _Main ("Main", 2D) = "bump" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
			//Tags 
			//{
			//    "LightMode"="ForwardBase"
			//}
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 

            uniform sampler2D _GrabTexture;
            uniform float4 _TimeEditor;
            uniform sampler2D _Main; uniform float4 _Main_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 uv2 : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float4 node_8490 = _Time + _TimeEditor;
                float2 node_7210 = (i.uv0+float2((i.uv1.r*node_8490.g),(i.uv1.g*node_8490.g)));
                float3 _Main_var = UnpackNormal(tex2D(_Main,TRANSFORM_TEX(node_7210, _Main)));
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (_Main_var.rgb.rg*i.vertexColor.a*i.uv2.r*0.1);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
////// Emissive:
                float node_865 = 0.0;
                float3 emissive = float3(node_865,node_865,node_865);
                float3 finalColor = emissive;
                return fixed4(lerp(sceneColor.rgb, finalColor,node_865),1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
