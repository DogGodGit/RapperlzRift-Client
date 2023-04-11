// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Transparent/TwoSide" //Name of the shader
{
	Properties // This is a properties block, where you create properties that will be used in the shader
	{
		_Color ("Main Color", Color) = (1.0,1.0,1.0,1.0)  // '_' variables that can be adjusted in unity, self name tracking
		_ColorTint ("Color Tint", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex ("Diffuse Texture", 2D) = "white"{}
	}
	
	// Subshader contains the shader
	SubShader
	{
		Tags { "Queue" = "Transparent" } // Tells Unity to render this in the transparent queue, this stops flickering
		Blend srcAlpha oneMinusSrcAlpha
		// Pass is like a render pass in Maya/Softimage
		Pass
		{			
			Lighting Off
			zWrite ON
			Cull off

			
			// Everything outside of CGProgram is Shaderlab language
			CGPROGRAM // Is the language writen by NVIDIA
			
//			//pragmas
			#pragma vertex vert
			#pragma fragment frag
			
			//user defined variables
			//uniform fixed4 _Color;  	//uniform is a CG specific keyword, used to define variables, Unity doesn't need it			
			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform half4 _Color;
			uniform half4 _ColorTint;
			
			
			//base input structs
			// structs are like base class's
			struct vertexInput
			{
				half4 vertex : POSITION; // Assigns the vertex possition to a vertex variable								
				half4 texcoord : TEXCOORD0;
			};
			
			struct vertexOutput
			{
				half4 pos : SV_POSITION; // Needed for DX11
				half4 tex : TEXCOORD1;								
			};
						
			//vertex function
			
			vertexOutput vert(vertexInput v) //writing to vertexOutput, vert is the new name ifunctiont
			{
				vertexOutput o;	 // is the same as o = vertexOutput				
				o.pos = UnityObjectToClipPos(v.vertex);
								
				o.tex = v.texcoord;
								
				return o;			
			}
			
			//fragment functions
			
			fixed4 frag(vertexOutput i) : COLOR
			{			
				//Texture Maps
				fixed4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);							
				return fixed4( tex.xyz * _ColorTint.xyz + _Color.xyz, tex.a * _Color.a * _ColorTint.a);
			}
			
			ENDCG
		}
	}


	// fallback commented out during development
	//Fallback "Flat" // this incase your shader errors out it will use this instead
}