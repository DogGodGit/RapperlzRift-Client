// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.36 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.36;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:5783,x:32719,y:32712,varname:node_5783,prsc:2|emission-4182-OUT;n:type:ShaderForge.SFN_VertexColor,id:6068,x:32080,y:33054,varname:node_6068,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4182,x:32455,y:32772,varname:node_4182,prsc:2|A-7039-OUT,B-6068-RGB;n:type:ShaderForge.SFN_Append,id:6758,x:30760,y:32558,varname:node_6758,prsc:2|A-5322-OUT,B-7245-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5322,x:30528,y:32580,ptovrint:False,ptlb:U speed,ptin:_Uspeed,varname:_Uspeed,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Time,id:5214,x:30760,y:32682,varname:node_5214,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:7245,x:30538,y:32716,ptovrint:False,ptlb:v speed,ptin:_vspeed,varname:_vspeed,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_Multiply,id:7320,x:31110,y:32583,varname:node_7320,prsc:2|A-6758-OUT,B-5214-T;n:type:ShaderForge.SFN_TexCoord,id:7711,x:31110,y:32720,varname:node_7711,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:7730,x:31305,y:32583,varname:node_7730,prsc:2|A-7320-OUT,B-7711-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:996,x:31510,y:32573,ptovrint:False,ptlb:waves,ptin:_waves,varname:_waves,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a96dae219af42e14bad7127dc53f53ab,ntxv:0,isnm:False|UVIN-7730-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8523,x:31510,y:32759,ptovrint:False,ptlb:waves power,ptin:_wavespower,varname:_wavespower,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.3;n:type:ShaderForge.SFN_Multiply,id:4386,x:31711,y:32573,varname:node_4386,prsc:2|A-996-RGB,B-8523-OUT;n:type:ShaderForge.SFN_Append,id:2821,x:30952,y:32906,varname:node_2821,prsc:2|A-6359-OUT,B-3042-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6359,x:30720,y:32928,ptovrint:False,ptlb:U speed_mask,ptin:_Uspeed_mask,varname:_Uspeed_mask,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Time,id:5008,x:30952,y:33030,varname:node_5008,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:3042,x:30730,y:33064,ptovrint:False,ptlb:v speed_mask,ptin:_vspeed_mask,varname:_vspeed_mask,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:5708,x:31110,y:32895,varname:node_5708,prsc:2|A-2821-OUT,B-5008-T;n:type:ShaderForge.SFN_Add,id:6876,x:31319,y:32887,varname:node_6876,prsc:2|A-5708-OUT,B-7711-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:7207,x:31510,y:32870,ptovrint:False,ptlb:mask,ptin:_mask,varname:_mask,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c4967c588a3109040ae256b3096ba7de,ntxv:0,isnm:False|UVIN-6876-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1459,x:31510,y:33093,ptovrint:False,ptlb:mask power,ptin:_maskpower,varname:_maskpower,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:12;n:type:ShaderForge.SFN_Multiply,id:4188,x:31707,y:32847,varname:node_4188,prsc:2|A-7207-RGB,B-1459-OUT;n:type:ShaderForge.SFN_Multiply,id:7039,x:31927,y:32701,varname:node_7039,prsc:2|A-4386-OUT,B-4188-OUT,C-3685-OUT,D-9847-OUT;n:type:ShaderForge.SFN_Append,id:3299,x:30937,y:33216,varname:node_3299,prsc:2|A-5384-OUT,B-7623-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5384,x:30705,y:33238,ptovrint:False,ptlb:U speed_map,ptin:_Uspeed_map,varname:_Uspeed_map,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Time,id:965,x:30937,y:33340,varname:node_965,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:7623,x:30715,y:33374,ptovrint:False,ptlb:v speed_map,ptin:_vspeed_map,varname:_vspeed_map,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.15;n:type:ShaderForge.SFN_Multiply,id:3757,x:31111,y:33216,varname:node_3757,prsc:2|A-3299-OUT,B-965-T;n:type:ShaderForge.SFN_Add,id:5075,x:31306,y:33216,varname:node_5075,prsc:2|A-3757-OUT,B-7711-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:9322,x:31506,y:33199,ptovrint:False,ptlb:map,ptin:_map,varname:_map,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4da659def8282fb40af91d4291566704,ntxv:0,isnm:False|UVIN-5075-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2986,x:31506,y:33384,ptovrint:False,ptlb:map power_copy,ptin:_mappower_copy,varname:_mappower_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.3;n:type:ShaderForge.SFN_Multiply,id:9847,x:31713,y:33187,varname:node_9847,prsc:2|A-9322-RGB,B-2986-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3685,x:31713,y:33035,ptovrint:False,ptlb:emissive,ptin:_emissive,varname:_emissive,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:2969,x:31913,y:32573,ptovrint:False,ptlb:power,ptin:_power,varname:node_2969,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;proporder:5322-7245-996-8523-6359-3042-7207-1459-5384-7623-9322-2986-3685-2969;pass:END;sub:END;*/

Shader "Custom/panner00" 
{
    Properties {
        _Uspeed ("U speed", Float ) = 0
        _vspeed ("v speed", Float ) = 0.2
        _waves ("waves", 2D) = "white" {}
        _wavespower ("waves power", Float ) = 0.3
        _Uspeed_mask ("U speed_mask", Float ) = 0
        _vspeed_mask ("v speed_mask", Float ) = 0
        _mask ("mask", 2D) = "white" {}
        _maskpower ("mask power", Float ) = 12
        _Uspeed_map ("U speed_map", Float ) = 0
        _vspeed_map ("v speed_map", Float ) = -0.15
        _map ("map", 2D) = "white" {}
        _mappower_copy ("map power_copy", Float ) = 0.3
        _emissive ("emissive", Float ) = 1
        _power ("power", Float ) = 1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "FORWARD"
			Tags 
			{
			    "LightMode"="ForwardBase"
			}
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#ifndef UNITY_PASS_FORWARDBASE
			#define UNITY_PASS_FORWARDBASE
#endif
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _Uspeed;
            uniform float _vspeed;
            uniform sampler2D _waves; uniform float4 _waves_ST;
            uniform float _wavespower;
            uniform float _Uspeed_mask;
            uniform float _vspeed_mask;
            uniform sampler2D _mask; uniform float4 _mask_ST;
            uniform float _maskpower;
            uniform float _Uspeed_map;
            uniform float _vspeed_map;
            uniform sampler2D _map; uniform float4 _map_ST;
            uniform float _mappower_copy;
            uniform float _emissive;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
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
                float4 node_5214 = _Time + _TimeEditor;
                float2 node_7730 = ((float2(_Uspeed,_vspeed)*node_5214.g)+i.uv0);
                float4 _waves_var = tex2D(_waves,TRANSFORM_TEX(node_7730, _waves));
                float4 node_5008 = _Time + _TimeEditor;
                float2 node_6876 = ((float2(_Uspeed_mask,_vspeed_mask)*node_5008.g)+i.uv0);
                float4 _mask_var = tex2D(_mask,TRANSFORM_TEX(node_6876, _mask));
                float4 node_965 = _Time + _TimeEditor;
                float2 node_5075 = ((float2(_Uspeed_map,_vspeed_map)*node_965.g)+i.uv0);
                float4 _map_var = tex2D(_map,TRANSFORM_TEX(node_5075, _map));
                float3 emissive = (((_waves_var.rgb*_wavespower)*(_mask_var.rgb*_maskpower)*_emissive*(_map_var.rgb*_mappower_copy))*i.vertexColor.rgb);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,0.1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
