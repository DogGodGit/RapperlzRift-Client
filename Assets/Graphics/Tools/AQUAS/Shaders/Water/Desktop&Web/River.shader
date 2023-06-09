Shader "AQUAS/Desktop and Web/Double-Sided/River" {
    Properties {
        [NoScaleOffset]_SmallWavesTexture ("Small Waves Texture", 2D) = "bump" {}
        _SmallWavesTiling ("Small Waves Tiling", Float ) = 1
        _SmallWavesSpeed ("Small Waves Speed", Float ) = -20
        _SmallWaveRrefraction ("Small Wave Rrefraction", Range(0, 3)) = 2.2
        [NoScaleOffset]_MediumWavesTexture ("Medium Waves Texture", 2D) = "bump" {}
        _MediumWavesTiling ("Medium Waves Tiling", Float ) = 0.5
        _MediumWavesSpeed ("Medium Waves Speed", Float ) = 40
        _MediumWaveRefraction ("Medium Wave Refraction", Range(0, 3)) = 1.8
        [NoScaleOffset]_LargeWavesTexture ("Large Waves Texture", 2D) = "bump" {}
        _LargeWavesTiling ("Large Waves Tiling", Float ) = 0.3
        _LargeWavesSpeed ("Large Waves Speed", Float ) = -40
        _LargeWaveRefraction ("Large Wave Refraction", Range(0, 3)) = 1.5
        _MainColor ("Main Color", Color) = (0,0.4627451,1,1)
        _DeepWaterColor ("Deep Water Color", Color) = (0,0.3411765,0.6235294,1)
        _Fade ("Fade", Float ) = 1.45
        _Density ("Density", Range(0, 10)) = 1.74
        _DepthTransparency ("Depth Transparency", Float ) = 1.5
        _ShoreFade ("Shore Fade", Float ) = 0.3
        _ShoreTransparency ("Shore Transparency", Float ) = 0.04
        _WaveBlend ("Wave Blend", Float ) = 0.77
        _WaveFade ("Wave Fade", Float ) = 1.19
        [MaterialToggle] _EnableReflections ("Enable Reflections", Float ) = 0
        _ReflectionIntensity ("Reflection Intensity", Range(0, 1)) = 0.5
        _Distortion ("Distortion", Range(0, 2)) = 0.3
        _Specular ("Specular", Float ) = 1
        _Gloss ("Gloss", Range(0, 1)) = 0.55
        _LightWrapping ("Light Wrapping", Float ) = 0
        [HideInInspector]_ReflectionTex ("Reflection Tex", 2D) = "white" {}
        [NoScaleOffset]_FoamTexture ("Foam Texture", 2D) = "white" {}
        _FoamTiling ("Foam Tiling", Float ) = 3
        _FoamBlend ("Foam Blend", Float ) = 0.15
        _FoamVisibility ("Foam Visibility", Range(0, 1)) = 0.3
        _FoamIntensity ("Foam Intensity", Float ) = 10
        _FoamContrast ("Foam Contrast", Range(0, 0.5)) = 0.25
        _FoamColor ("Foam Color", Color) = (0.3823529,0.3879758,0.3879758,1)
        _FoamSpeed ("Foam Speed", Float ) = 120
        _FoamDistFalloff ("Foam Dist. Falloff", Float ) = 16
        _FoamDistFade ("Foam Dist. Fade", Float ) = 9.5
        [MaterialToggle] _UnderwaterMode ("Underwater Mode", Float ) = 0
        [MaterialToggle] _EnableCustomFog ("Enable Custom Fog", Float ) = 3.654843
        _FogColor ("Fog Color", Color) = (1,1,1,1)
        _FogDistance ("Fog Distance", Float ) = 1000
        _FogFade ("Fog Fade", Float ) = 1
        [NoScaleOffset]_FlowMap ("Flow Map", 2D) = "white" {}
        _FlowSpeed ("Flow Speed", Float ) = 5
        [MaterialToggle] _UsingLinearColorSpace ("Using Linear Color Space", Float ) = 0
        _MediumTilingDistance ("Medium Tiling Distance", Float ) = 1500
        _LongTilingDistance ("Long Tiling Distance", Float ) = 3000
        _DistanceTilingFade ("Distance Tiling Fade", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 128
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform float _Gloss;
            uniform float _SmallWaveRrefraction;
            uniform float _Density;
            uniform float4 _MainColor;
            uniform float _Fade;
            uniform float4 _DeepWaterColor;
            uniform float _SmallWavesSpeed;
            uniform float _SmallWavesTiling;
            uniform sampler2D _ReflectionTex; uniform float4 _ReflectionTex_ST;
            uniform float _ReflectionIntensity;
            uniform fixed _EnableReflections;
            uniform float _MediumWavesTiling;
            uniform float _MediumWavesSpeed;
            uniform float _MediumWaveRefraction;
            uniform float _LargeWaveRefraction;
            uniform float _LargeWavesTiling;
            uniform float _LargeWavesSpeed;
            uniform float _DepthTransparency;
            uniform float _FoamBlend;
            uniform float4 _FoamColor;
            uniform float _FoamIntensity;
            uniform float _FoamContrast;
            uniform sampler2D _FoamTexture;
            uniform float _FoamSpeed;
            uniform float _FoamTiling;
            uniform float _FoamDistFalloff;
            uniform float _FoamDistFade;
            uniform float _FoamVisibility;
            uniform fixed _UnderwaterMode;
            uniform float _ShoreFade;
            uniform float _ShoreTransparency;
            uniform float _LightWrapping;
            uniform float _Specular;
            uniform float _Distortion;
            uniform fixed _EnableCustomFog;
            uniform float4 _FogColor;
            uniform float _FogDistance;
            uniform float _FogFade;
            uniform sampler2D _FlowMap;
            uniform float _FlowSpeed;
            uniform sampler2D _SmallWavesTexture;
            uniform sampler2D _LargeWavesTexture;
            uniform sampler2D _MediumWavesTexture;
            uniform float _MediumTilingDistance;
            uniform float _DistanceTilingFade;
            uniform float _LongTilingDistance;
            uniform fixed _UsingLinearColorSpace;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
                float4 projPos : TEXCOORD6;
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                float3 objScale = 1.0/recipObjScale;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                float3 objScale = 1.0/recipObjScale;
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float _value1 = 1000.0;
                float2 _division1 = ((objScale.rb*_SmallWavesTiling)/_value1);
                float4 _timer1 = _Time + _TimeEditor;
                float2 _smallWavesPanner = (i.uv0+(float3((_SmallWavesSpeed/_division1),0.0)*(_timer1.r/100.0)));
                float4 _FlowMap_var = tex2D(_FlowMap,i.uv0);
                float2 node_316 = _FlowMap_var.rgb.rg;
                float2 _flowReMap = ((lerp( node_316, pow(node_316,0.4545455), _UsingLinearColorSpace )*1.0+-0.5)*(_FlowSpeed*(-1.0)));
                float4 _flowMapTimer = _Time + _TimeEditor;
                float _flowMapTimeRead = frac(_flowMapTimer.r);
                float2 _flowMapStretch1 = (_flowReMap*_flowMapTimeRead);
                float2 _smallWavesPanFlow1 = (_smallWavesPanner+_flowMapStretch1);
                float2 _multiplier2 = (_smallWavesPanFlow1*_division1);
                float3 _SmallWavesTexNF = UnpackNormal(tex2D(_SmallWavesTexture,_multiplier2));
                float _flowMapLerpValue = 0.5;
                float2 _flowMapStretch2 = (_flowReMap*frac((_flowMapTimer.r+_flowMapLerpValue)));
                float2 _smallWavesPanFlow2 = (_smallWavesPanner+_flowMapStretch2);
                float2 _multiplier11 = (_smallWavesPanFlow2*_division1);
                float3 _SmallWavesTexF = UnpackNormal(tex2D(_SmallWavesTexture,_multiplier11));
                float _flowMapLerpControl = abs(((_flowMapLerpValue-_flowMapTimeRead)/_flowMapLerpValue));
                float _IO1 = _flowMapLerpControl;
                float _value5 = 10.0;
                float2 _division6 = (_division1/_value5);
                float2 _multiplier12 = (_smallWavesPanFlow1*_division6);
                float3 _SmallWavesTexNF2 = UnpackNormal(tex2D(_SmallWavesTexture,_multiplier12));
                float2 _multiplier13 = (_smallWavesPanFlow2*_division6);
                float3 _SmallWavesTexF2 = UnpackNormal(tex2D(_SmallWavesTexture,_multiplier13));
                float _distance1 = distance(i.posWorld.rgb,_WorldSpaceCameraPos);
                float _clampedMdiumDistance = saturate(pow((_distance1/_MediumTilingDistance),_DistanceTilingFade));
                float _value6 = 20.0;
                float2 _division7 = (_division1/_value6);
                float2 _multiplier14 = (_smallWavesPanFlow1*_division7);
                float3 _SmallWavesTexNF3 = UnpackNormal(tex2D(_SmallWavesTexture,_multiplier14));
                float2 _multiplier15 = (_smallWavesPanFlow2*_division7);
                float3 _SmallWavesTexF3 = UnpackNormal(tex2D(_SmallWavesTexture,_multiplier15));
                float _clampedLongDistance = saturate(pow((_distance1/_LongTilingDistance),_DistanceTilingFade));
                float node_1961 = 2.0;
                float node_9572 = (_SmallWaveRrefraction/node_1961);
                float2 _division2 = ((objScale.rb*_MediumWavesTiling)/_value1);
                float4 _timer2 = _Time + _TimeEditor;
                float2 _mediumWavesPanner = (i.uv0+(float3((_MediumWavesSpeed/_division2),0.0)*(_timer2.r/100.0)));
                float2 _mediumWavesPanFlow1 = (_mediumWavesPanner+_flowMapStretch1);
                float2 _multiplier3 = (_mediumWavesPanFlow1*_division2);
                float3 _MediumWavesTexNF = UnpackNormal(tex2D(_MediumWavesTexture,_multiplier3));
                float2 _mediumWavesPanFlow2 = (_mediumWavesPanner+_flowMapStretch2);
                float2 _multiplier16 = (_mediumWavesPanFlow2*_division2);
                float3 _MediumWavesTexF = UnpackNormal(tex2D(_MediumWavesTexture,_multiplier16));
                float2 _division8 = (_division2/_value5);
                float2 _multiplier17 = (_mediumWavesPanFlow1*_division8);
                float3 _MediumWavesTexNF2 = UnpackNormal(tex2D(_MediumWavesTexture,_multiplier17));
                float2 _multiplier18 = (_mediumWavesPanFlow2*_division8);
                float3 _MediumWavesTexF2 = UnpackNormal(tex2D(_MediumWavesTexture,_multiplier18));
                float2 _division9 = (_division2/_value6);
                float2 _multiplier20 = (_mediumWavesPanFlow1*_division9);
                float3 _MediumWavesTexNF3 = UnpackNormal(tex2D(_MediumWavesTexture,_multiplier20));
                float2 _multiplier21 = (_mediumWavesPanFlow2*_division9);
                float3 _MediumWavesTexF3 = UnpackNormal(tex2D(_MediumWavesTexture,_multiplier21));
                float node_2013 = 2.0;
                float node_4664 = (_MediumWaveRefraction/node_2013);
                float2 _division3 = ((objScale.rb*_LargeWavesTiling)/_value1);
                float4 _timer3 = _Time + _TimeEditor;
                float2 _largeWavesPanner = (i.uv0+(float3(0.0,(_LargeWavesSpeed/_division3))*(_timer3.r/100.0)));
                float2 _largeWavesPanFlow1 = (_largeWavesPanner+_flowMapStretch1);
                float2 _multiplier4 = (_largeWavesPanFlow1*_division3);
                float3 _LargeWavesTexNF = UnpackNormal(tex2D(_LargeWavesTexture,_multiplier4));
                float2 _largeWavesPanFlow2 = (_largeWavesPanner+_flowMapStretch2);
                float2 _multiplier22 = (_largeWavesPanFlow2*_division3);
                float3 _LargeWavesTexF = UnpackNormal(tex2D(_LargeWavesTexture,_multiplier22));
                float2 _division10 = (_division3/_value5);
                float2 _multiplier23 = (_largeWavesPanFlow1*_division10);
                float3 _LargeWavesTexNF2 = UnpackNormal(tex2D(_LargeWavesTexture,_multiplier23));
                float2 _multiplier24 = (_largeWavesPanFlow2*_division10);
                float3 _LargeWavesTexF2 = UnpackNormal(tex2D(_LargeWavesTexture,_multiplier24));
                float2 _division11 = (_division3/_value6);
                float2 _multiplier25 = (_largeWavesPanFlow1*_division11);
                float3 _LargeWavesTexNF3 = UnpackNormal(tex2D(_LargeWavesTexture,_multiplier25));
                float2 _multiplier26 = (_largeWavesPanFlow2*_division11);
                float3 _LargeWavesTexF3 = UnpackNormal(tex2D(_LargeWavesTexture,_multiplier26));
                float node_6805 = 2.0;
                float node_2947 = (_LargeWaveRefraction/node_6805);
                float node_6863 = lerp(lerp(_LargeWaveRefraction,node_2947,_clampedMdiumDistance),(node_2947/node_6805),_clampedLongDistance);
                float3 _add1 = (lerp(float3(0,0,1),lerp(lerp(lerp(_SmallWavesTexNF.rgb,_SmallWavesTexF.rgb,_IO1),lerp(_SmallWavesTexNF2.rgb,_SmallWavesTexF2.rgb,_IO1),_clampedMdiumDistance),lerp(_SmallWavesTexNF3.rgb,_SmallWavesTexF3.rgb,_IO1),_clampedLongDistance),lerp(lerp(_SmallWaveRrefraction,node_9572,_clampedMdiumDistance),(node_9572/node_1961),_clampedLongDistance))+lerp(float3(0,0,1),lerp(lerp(lerp(_MediumWavesTexNF.rgb,_MediumWavesTexF.rgb,_flowMapLerpControl),lerp(_MediumWavesTexNF2.rgb,_MediumWavesTexF2.rgb,_flowMapLerpControl),_clampedMdiumDistance),lerp(_MediumWavesTexNF3.rgb,_MediumWavesTexF3.rgb,_flowMapLerpControl),_clampedLongDistance),lerp(lerp(_MediumWaveRefraction,node_4664,_clampedMdiumDistance),(node_4664/node_2013),_clampedLongDistance))+lerp(float3(0,0,1),lerp(lerp(lerp(_LargeWavesTexNF.rgb,_LargeWavesTexF.rgb,_flowMapLerpControl),lerp(_LargeWavesTexNF2.rgb,_LargeWavesTexF2.rgb,_flowMapLerpControl),_clampedMdiumDistance),lerp(_LargeWavesTexNF3.rgb,_LargeWavesTexF3.rgb,_flowMapLerpControl),_clampedLongDistance),node_6863));
                float3 normalLocal = _add1;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float _multiplier1 = (pow(saturate((sceneZ-partZ)/_DepthTransparency),_ShoreFade)*saturate((sceneZ-partZ)/_ShoreTransparency));
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + ((_add1.rg*(_MediumWaveRefraction*0.02))*_multiplier1);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _Gloss;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMin[0] = unity_SpecCube0_BoxMin;
                    d.boxMin[1] = unity_SpecCube1_BoxMin;
                #endif
                #if UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMax[0] = unity_SpecCube0_BoxMax;
                    d.boxMax[1] = unity_SpecCube1_BoxMax;
                    d.probePosition[0] = unity_SpecCube0_ProbePosition;
                    d.probePosition[1] = unity_SpecCube1_ProbePosition;
                #endif
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float3 specularColor = (_Specular*_LightColor0.rgb);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 indirectSpecular = (gi.indirect.specular)*specularColor;
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float3 w = float3(_LightWrapping,_LightWrapping,_LightWrapping)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = forwardLight * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 _vector31 = float3(0,0,0);
                float _value2 = 1.0;
                float3 _blend1 = saturate((_DeepWaterColor.rgb+(sceneColor.rgb*pow(saturate((_value2 + ( ((sceneZ-partZ) - _vector31) * (0.5 - _value2) ) / ((_MainColor.rgb*(10.0/_Density)) - _vector31))),_Fade))));
                float2 _mask1 = _add1.rg;
                float2 _remap = ((i.screenPos.rg+(float2(_mask1.r,_mask1.g)*_Distortion))*0.5+0.5);
                float4 _ReflectionTex_var = tex2D(_ReflectionTex,TRANSFORM_TEX(_remap, _ReflectionTex));
                float _rotator1_ang = 1.5708;
                float _rotator1_spd = 1.0;
                float _rotator1_cos = cos(_rotator1_spd*_rotator1_ang);
                float _rotator1_sin = sin(_rotator1_spd*_rotator1_ang);
                float2 _rotator1_piv = float2(0.5,0.5);
                float2 _rotator1 = (mul(i.uv0-_rotator1_piv,float2x2( _rotator1_cos, -_rotator1_sin, _rotator1_sin, _rotator1_cos))+_rotator1_piv);
                float2 _mask2 = objScale.rb;
                float _value4 = 1000.0;
                float2 _division4 = ((_mask2*_FoamTiling)/_value4);
                float4 _timer4 = _Time + _TimeEditor;
                float3 _multiplier6 = (float3((_FoamSpeed/_division4),0.0)*(_timer4.r/100.0));
                float2 _add2 = (_rotator1+_multiplier6);
                float2 _multiplier7 = (_add2*_division4);
                float4 _texture4 = tex2D(_FoamTexture,_multiplier7);
                float2 _add3 = (i.uv0+_multiplier6);
                float2 _multiplier8 = (_add3*_division4);
                float4 _texture5 = tex2D(_FoamTexture,_multiplier8);
                float2 _division5 = ((_mask2*(_FoamTiling/3.0))/_value4);
                float2 _multiplier9 = (_add2*_division5);
                float4 _texture6 = tex2D(_FoamTexture,_multiplier9);
                float2 _multiplier10 = (_add3*_division5);
                float4 _texture7 = tex2D(_FoamTexture,_multiplier10);
                float _value3 = 0.0;
                float3 _multiplier5 = ((((_value3 + ( (dot(lerp((_texture4.rgb-_texture5.rgb),(_texture6.rgb-_texture7.rgb),saturate(pow((distance(i.posWorld.rgb,_WorldSpaceCameraPos)/_FoamDistFade),_FoamDistFalloff))),float3(0.3,0.59,0.11)) - _FoamContrast) * (1.0 - _value3) ) / ((1.0 - _FoamContrast) - _FoamContrast))*_FoamColor.rgb)*(_FoamIntensity*(-1.0)))*(saturate((sceneZ-partZ)/_FoamBlend)*-1.0+1.0));
                float3 _lerp1 = lerp(lerp( _blend1, lerp(_blend1,_ReflectionTex_var.rgb,_ReflectionIntensity), _EnableReflections ),(_multiplier5*_multiplier5),_FoamVisibility);
                float3 diffuseColor = lerp( _lerp1, lerp(_lerp1,_FogColor.rgb,saturate(pow((distance(i.posWorld.rgb,_WorldSpaceCameraPos)/_FogDistance),_FogFade))), _EnableCustomFog );
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,lerp( _multiplier1, 0.2, _UnderwaterMode )),1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform float _Gloss;
            uniform float _SmallWaveRrefraction;
            uniform float _Density;
            uniform float4 _MainColor;
            uniform float _Fade;
            uniform float4 _DeepWaterColor;
            uniform float _SmallWavesSpeed;
            uniform float _SmallWavesTiling;
            uniform sampler2D _ReflectionTex; uniform float4 _ReflectionTex_ST;
            uniform float _ReflectionIntensity;
            uniform fixed _EnableReflections;
            uniform float _MediumWavesTiling;
            uniform float _MediumWavesSpeed;
            uniform float _MediumWaveRefraction;
            uniform float _LargeWaveRefraction;
            uniform float _LargeWavesTiling;
            uniform float _LargeWavesSpeed;
            uniform float _DepthTransparency;
            uniform float _FoamBlend;
            uniform float4 _FoamColor;
            uniform float _FoamIntensity;
            uniform float _FoamContrast;
            uniform sampler2D _FoamTexture;
            uniform float _FoamSpeed;
            uniform float _FoamTiling;
            uniform float _FoamDistFalloff;
            uniform float _FoamDistFade;
            uniform float _FoamVisibility;
            uniform fixed _UnderwaterMode;
            uniform float _ShoreFade;
            uniform float _ShoreTransparency;
            uniform float _LightWrapping;
            uniform float _Specular;
            uniform float _Distortion;
            uniform fixed _EnableCustomFog;
            uniform float4 _FogColor;
            uniform float _FogDistance;
            uniform float _FogFade;
            uniform sampler2D _FlowMap;
            uniform float _FlowSpeed;
            uniform sampler2D _SmallWavesTexture;
            uniform sampler2D _LargeWavesTexture;
            uniform sampler2D _MediumWavesTexture;
            uniform float _MediumTilingDistance;
            uniform float _DistanceTilingFade;
            uniform float _LongTilingDistance;
            uniform fixed _UsingLinearColorSpace;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
                float4 projPos : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                float3 objScale = 1.0/recipObjScale;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                float3 objScale = 1.0/recipObjScale;
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float _value1 = 1000.0;
                float2 _division1 = ((objScale.rb*_SmallWavesTiling)/_value1);
                float4 _timer1 = _Time + _TimeEditor;
                float2 _smallWavesPanner = (i.uv0+(float3((_SmallWavesSpeed/_division1),0.0)*(_timer1.r/100.0)));
                float4 _FlowMap_var = tex2D(_FlowMap,i.uv0);
                float2 node_316 = _FlowMap_var.rgb.rg;
                float2 _flowReMap = ((lerp( node_316, pow(node_316,0.4545455), _UsingLinearColorSpace )*1.0+-0.5)*(_FlowSpeed*(-1.0)));
                float4 _flowMapTimer = _Time + _TimeEditor;
                float _flowMapTimeRead = frac(_flowMapTimer.r);
                float2 _flowMapStretch1 = (_flowReMap*_flowMapTimeRead);
                float2 _smallWavesPanFlow1 = (_smallWavesPanner+_flowMapStretch1);
                float2 _multiplier2 = (_smallWavesPanFlow1*_division1);
                float3 _SmallWavesTexNF = UnpackNormal(tex2D(_SmallWavesTexture,_multiplier2));
                float _flowMapLerpValue = 0.5;
                float2 _flowMapStretch2 = (_flowReMap*frac((_flowMapTimer.r+_flowMapLerpValue)));
                float2 _smallWavesPanFlow2 = (_smallWavesPanner+_flowMapStretch2);
                float2 _multiplier11 = (_smallWavesPanFlow2*_division1);
                float3 _SmallWavesTexF = UnpackNormal(tex2D(_SmallWavesTexture,_multiplier11));
                float _flowMapLerpControl = abs(((_flowMapLerpValue-_flowMapTimeRead)/_flowMapLerpValue));
                float _IO1 = _flowMapLerpControl;
                float _value5 = 10.0;
                float2 _division6 = (_division1/_value5);
                float2 _multiplier12 = (_smallWavesPanFlow1*_division6);
                float3 _SmallWavesTexNF2 = UnpackNormal(tex2D(_SmallWavesTexture,_multiplier12));
                float2 _multiplier13 = (_smallWavesPanFlow2*_division6);
                float3 _SmallWavesTexF2 = UnpackNormal(tex2D(_SmallWavesTexture,_multiplier13));
                float _distance1 = distance(i.posWorld.rgb,_WorldSpaceCameraPos);
                float _clampedMdiumDistance = saturate(pow((_distance1/_MediumTilingDistance),_DistanceTilingFade));
                float _value6 = 20.0;
                float2 _division7 = (_division1/_value6);
                float2 _multiplier14 = (_smallWavesPanFlow1*_division7);
                float3 _SmallWavesTexNF3 = UnpackNormal(tex2D(_SmallWavesTexture,_multiplier14));
                float2 _multiplier15 = (_smallWavesPanFlow2*_division7);
                float3 _SmallWavesTexF3 = UnpackNormal(tex2D(_SmallWavesTexture,_multiplier15));
                float _clampedLongDistance = saturate(pow((_distance1/_LongTilingDistance),_DistanceTilingFade));
                float node_1961 = 2.0;
                float node_9572 = (_SmallWaveRrefraction/node_1961);
                float2 _division2 = ((objScale.rb*_MediumWavesTiling)/_value1);
                float4 _timer2 = _Time + _TimeEditor;
                float2 _mediumWavesPanner = (i.uv0+(float3((_MediumWavesSpeed/_division2),0.0)*(_timer2.r/100.0)));
                float2 _mediumWavesPanFlow1 = (_mediumWavesPanner+_flowMapStretch1);
                float2 _multiplier3 = (_mediumWavesPanFlow1*_division2);
                float3 _MediumWavesTexNF = UnpackNormal(tex2D(_MediumWavesTexture,_multiplier3));
                float2 _mediumWavesPanFlow2 = (_mediumWavesPanner+_flowMapStretch2);
                float2 _multiplier16 = (_mediumWavesPanFlow2*_division2);
                float3 _MediumWavesTexF = UnpackNormal(tex2D(_MediumWavesTexture,_multiplier16));
                float2 _division8 = (_division2/_value5);
                float2 _multiplier17 = (_mediumWavesPanFlow1*_division8);
                float3 _MediumWavesTexNF2 = UnpackNormal(tex2D(_MediumWavesTexture,_multiplier17));
                float2 _multiplier18 = (_mediumWavesPanFlow2*_division8);
                float3 _MediumWavesTexF2 = UnpackNormal(tex2D(_MediumWavesTexture,_multiplier18));
                float2 _division9 = (_division2/_value6);
                float2 _multiplier20 = (_mediumWavesPanFlow1*_division9);
                float3 _MediumWavesTexNF3 = UnpackNormal(tex2D(_MediumWavesTexture,_multiplier20));
                float2 _multiplier21 = (_mediumWavesPanFlow2*_division9);
                float3 _MediumWavesTexF3 = UnpackNormal(tex2D(_MediumWavesTexture,_multiplier21));
                float node_2013 = 2.0;
                float node_4664 = (_MediumWaveRefraction/node_2013);
                float2 _division3 = ((objScale.rb*_LargeWavesTiling)/_value1);
                float4 _timer3 = _Time + _TimeEditor;
                float2 _largeWavesPanner = (i.uv0+(float3(0.0,(_LargeWavesSpeed/_division3))*(_timer3.r/100.0)));
                float2 _largeWavesPanFlow1 = (_largeWavesPanner+_flowMapStretch1);
                float2 _multiplier4 = (_largeWavesPanFlow1*_division3);
                float3 _LargeWavesTexNF = UnpackNormal(tex2D(_LargeWavesTexture,_multiplier4));
                float2 _largeWavesPanFlow2 = (_largeWavesPanner+_flowMapStretch2);
                float2 _multiplier22 = (_largeWavesPanFlow2*_division3);
                float3 _LargeWavesTexF = UnpackNormal(tex2D(_LargeWavesTexture,_multiplier22));
                float2 _division10 = (_division3/_value5);
                float2 _multiplier23 = (_largeWavesPanFlow1*_division10);
                float3 _LargeWavesTexNF2 = UnpackNormal(tex2D(_LargeWavesTexture,_multiplier23));
                float2 _multiplier24 = (_largeWavesPanFlow2*_division10);
                float3 _LargeWavesTexF2 = UnpackNormal(tex2D(_LargeWavesTexture,_multiplier24));
                float2 _division11 = (_division3/_value6);
                float2 _multiplier25 = (_largeWavesPanFlow1*_division11);
                float3 _LargeWavesTexNF3 = UnpackNormal(tex2D(_LargeWavesTexture,_multiplier25));
                float2 _multiplier26 = (_largeWavesPanFlow2*_division11);
                float3 _LargeWavesTexF3 = UnpackNormal(tex2D(_LargeWavesTexture,_multiplier26));
                float node_6805 = 2.0;
                float node_2947 = (_LargeWaveRefraction/node_6805);
                float node_6863 = lerp(lerp(_LargeWaveRefraction,node_2947,_clampedMdiumDistance),(node_2947/node_6805),_clampedLongDistance);
                float3 _add1 = (lerp(float3(0,0,1),lerp(lerp(lerp(_SmallWavesTexNF.rgb,_SmallWavesTexF.rgb,_IO1),lerp(_SmallWavesTexNF2.rgb,_SmallWavesTexF2.rgb,_IO1),_clampedMdiumDistance),lerp(_SmallWavesTexNF3.rgb,_SmallWavesTexF3.rgb,_IO1),_clampedLongDistance),lerp(lerp(_SmallWaveRrefraction,node_9572,_clampedMdiumDistance),(node_9572/node_1961),_clampedLongDistance))+lerp(float3(0,0,1),lerp(lerp(lerp(_MediumWavesTexNF.rgb,_MediumWavesTexF.rgb,_flowMapLerpControl),lerp(_MediumWavesTexNF2.rgb,_MediumWavesTexF2.rgb,_flowMapLerpControl),_clampedMdiumDistance),lerp(_MediumWavesTexNF3.rgb,_MediumWavesTexF3.rgb,_flowMapLerpControl),_clampedLongDistance),lerp(lerp(_MediumWaveRefraction,node_4664,_clampedMdiumDistance),(node_4664/node_2013),_clampedLongDistance))+lerp(float3(0,0,1),lerp(lerp(lerp(_LargeWavesTexNF.rgb,_LargeWavesTexF.rgb,_flowMapLerpControl),lerp(_LargeWavesTexNF2.rgb,_LargeWavesTexF2.rgb,_flowMapLerpControl),_clampedMdiumDistance),lerp(_LargeWavesTexNF3.rgb,_LargeWavesTexF3.rgb,_flowMapLerpControl),_clampedLongDistance),node_6863));
                float3 normalLocal = _add1;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float _multiplier1 = (pow(saturate((sceneZ-partZ)/_DepthTransparency),_ShoreFade)*saturate((sceneZ-partZ)/_ShoreTransparency));
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + ((_add1.rg*(_MediumWaveRefraction*0.02))*_multiplier1);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
				UNITY_LIGHT_ATTENUATION(attenuation, i, i.posWorld.xyz);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _Gloss;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float3 specularColor = (_Specular*_LightColor0.rgb);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float3 w = float3(_LightWrapping,_LightWrapping,_LightWrapping)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = forwardLight * attenColor;
                float3 _vector31 = float3(0,0,0);
                float _value2 = 1.0;
                float3 _blend1 = saturate((_DeepWaterColor.rgb+(sceneColor.rgb*pow(saturate((_value2 + ( ((sceneZ-partZ) - _vector31) * (0.5 - _value2) ) / ((_MainColor.rgb*(10.0/_Density)) - _vector31))),_Fade))));
                float2 _mask1 = _add1.rg;
                float2 _remap = ((i.screenPos.rg+(float2(_mask1.r,_mask1.g)*_Distortion))*0.5+0.5);
                float4 _ReflectionTex_var = tex2D(_ReflectionTex,TRANSFORM_TEX(_remap, _ReflectionTex));
                float _rotator1_ang = 1.5708;
                float _rotator1_spd = 1.0;
                float _rotator1_cos = cos(_rotator1_spd*_rotator1_ang);
                float _rotator1_sin = sin(_rotator1_spd*_rotator1_ang);
                float2 _rotator1_piv = float2(0.5,0.5);
                float2 _rotator1 = (mul(i.uv0-_rotator1_piv,float2x2( _rotator1_cos, -_rotator1_sin, _rotator1_sin, _rotator1_cos))+_rotator1_piv);
                float2 _mask2 = objScale.rb;
                float _value4 = 1000.0;
                float2 _division4 = ((_mask2*_FoamTiling)/_value4);
                float4 _timer4 = _Time + _TimeEditor;
                float3 _multiplier6 = (float3((_FoamSpeed/_division4),0.0)*(_timer4.r/100.0));
                float2 _add2 = (_rotator1+_multiplier6);
                float2 _multiplier7 = (_add2*_division4);
                float4 _texture4 = tex2D(_FoamTexture,_multiplier7);
                float2 _add3 = (i.uv0+_multiplier6);
                float2 _multiplier8 = (_add3*_division4);
                float4 _texture5 = tex2D(_FoamTexture,_multiplier8);
                float2 _division5 = ((_mask2*(_FoamTiling/3.0))/_value4);
                float2 _multiplier9 = (_add2*_division5);
                float4 _texture6 = tex2D(_FoamTexture,_multiplier9);
                float2 _multiplier10 = (_add3*_division5);
                float4 _texture7 = tex2D(_FoamTexture,_multiplier10);
                float _value3 = 0.0;
                float3 _multiplier5 = ((((_value3 + ( (dot(lerp((_texture4.rgb-_texture5.rgb),(_texture6.rgb-_texture7.rgb),saturate(pow((distance(i.posWorld.rgb,_WorldSpaceCameraPos)/_FoamDistFade),_FoamDistFalloff))),float3(0.3,0.59,0.11)) - _FoamContrast) * (1.0 - _value3) ) / ((1.0 - _FoamContrast) - _FoamContrast))*_FoamColor.rgb)*(_FoamIntensity*(-1.0)))*(saturate((sceneZ-partZ)/_FoamBlend)*-1.0+1.0));
                float3 _lerp1 = lerp(lerp( _blend1, lerp(_blend1,_ReflectionTex_var.rgb,_ReflectionIntensity), _EnableReflections ),(_multiplier5*_multiplier5),_FoamVisibility);
                float3 diffuseColor = lerp( _lerp1, lerp(_lerp1,_FogColor.rgb,saturate(pow((distance(i.posWorld.rgb,_WorldSpaceCameraPos)/_FogDistance),_FogFade))), _EnableCustomFog );
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * lerp( _multiplier1, 0.2, _UnderwaterMode ),0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Legacy Shaders/Diffuse"
}
