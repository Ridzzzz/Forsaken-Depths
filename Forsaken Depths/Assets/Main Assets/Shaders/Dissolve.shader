﻿Shader "Custom/Dissolve"
{
    Properties
	{
		[Header(Base)]
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		[Header(Glow)]
		_RimColor("Rim Color", Color) = (1, 1, 1, 1)
		_RimPower("Rim Power", Range(1.0, 6.0)) = 3.0

		[Space]
		[Header(Noise)]
		_Noise("Noise Texture (RGB)", 2D) = "white" {}
		_Scale("Noise Texture Scale", Range(0,5)) = 1
		[Toggle(LOCAL)] _LOCAL("Local Texture?", Float) = 0

		[Space]
		[Header(Dissolve)]
		_DisAmount("Dissolve Amount", Range(-2,2)) = 0.0
		[Toggle(INVERT)] _INVERT("Inverse Direction?", Float) = 1
		_DisColor("Dissolve Color", Color) = (1,1,0,1)
		_DissolveColorWidth("Dissolve Color Width", Range(0,0.1)) = 0.01
		_Brightness("Dissolve Color Brightness", Range(0,20)) = 10
		_Cutoff("Noise Cutoff", Range(0,1)) = 0.5
		_Smoothness("Cutoff Smoothness", Range(0,2)) = 0.05

		[Space]
		[Header(Vertex Displacement)]
		_ScaleV("Displacement Scale", Range(0,1)) = 0.1
		_Offset("Displacement Y Offset", Range(-1,1)) = 0.7
		_DisplacementWidth("Displacement Segment Width", Range(0,1)) = 0.3
		_HeightScale("Height Displacement Amount", Range(0,0.2)) = 0.0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 200
			Cull Off

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard vertex:vert keepalpha addshadow
			#pragma shader_feature INVERT
			#pragma shader_feature LOCAL
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.5


			float _DisAmount, _Scale, _ScaleV;
			float _HeightScale;
			float _DisplacementWidth;
			float _Offset;

			struct Input
			{
				float2 uv_MainTex;
				float2 uv_BumpMap;
				float4 pos : POSITION;
				float3 worldPos;
				float3 worldNormal;
				float3 local;
				float4 color : Color;
				float3 viewDir;
				INTERNAL_DATA
			};



			void vert(inout appdata_full v, out Input o)
			{
				UNITY_INITIALIZE_OUTPUT(Input, o);
				// vertex position
				o.pos = mul(unity_ObjectToWorld, v.vertex.xyz);

				// local position that also takes rotation into account
				float3 rotatedLocal = mul((float3x3)unity_WorldToObject, o.pos);
				o.local = rotatedLocal;

				// position on model
				float dispPos = (o.pos.y + _DisAmount + _Offset);
				// clamped segment of model
				float dispPosClamped = smoothstep(0, 0.15, dispPos) * smoothstep(dispPos, dispPos + 0.15, _DisplacementWidth);
#if INVERT
				// position on model
				dispPos = 1 - (o.pos.y + _DisAmount + _Offset);
				// clamped segment of model
				dispPosClamped = smoothstep(0, 0.15, dispPos) * smoothstep(dispPos, dispPos + 0.15, _DisplacementWidth);

				//distort the mesh up
				v.vertex.y += (dispPosClamped * _HeightScale);
#else
				//or down
				v.vertex.y -= (dispPosClamped * _HeightScale);
#endif

				//distort the mesh sideways
				v.vertex.xz += dispPosClamped * (_ScaleV * (v.normal.xz));

				// do this again to account for displacement
				o.pos = mul(unity_ObjectToWorld, v.vertex.xyz);

			}

			sampler2D _MainTex, _Noise;
			half _Glossiness;
			half _Metallic;
			float _DissolveColorWidth, _Brightness, _Cutoff, _Smoothness;
			fixed4 _Color, _DisColor;

			//GLOW
			sampler2D _BumpMap;
			float4 _RimColor;
			float _RimPower;


			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				//UNITY_DEFINE_INSTANCED_PROP(float, _DisAmount) // uncomment this to use it per-instance
				// put more per-instance properties here
				UNITY_INSTANCING_BUFFER_END(Props)

				void surf(Input IN, inout SurfaceOutputStandard o)
				{
				// Main texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color * IN.color;

				
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				float3 blendNormal = saturate(pow(WorldNormalVector(IN, o.Normal) * 1.4, 4));
				
				float3 adjustedworldpos = IN.worldPos;
#if LOCAL
				adjustedworldpos = IN.local;
#endif

#if INVERT
				adjustedworldpos.y -= _Time.x;
#else
				adjustedworldpos.y += _Time.x;
#endif
				// normal noise triplanar for x, y, z sides
				float3 xn = tex2D(_Noise, adjustedworldpos.zy * _Scale);
				float3 yn = tex2D(_Noise, adjustedworldpos.zx * _Scale);
				float3 zn = tex2D(_Noise, adjustedworldpos.xy * _Scale);

				// lerped together all sides for noise texture
				float3 noisetexture = zn;
				noisetexture = lerp(noisetexture, xn, blendNormal.x);
				noisetexture = lerp(noisetexture, yn, blendNormal.y);

				float noise = noisetexture.r;

				// position on model
				float MovingPosOnModel = _DisAmount + IN.pos.y;
				// add noise
				MovingPosOnModel *= noise;

				// glowing bit that's a bit longer
				float maintexturePart = smoothstep(0, _Smoothness, MovingPosOnModel - _DissolveColorWidth);
				maintexturePart = step(_Cutoff, maintexturePart);

				// normal texture
				float glowingPart = smoothstep(0, _Smoothness, MovingPosOnModel);
				glowingPart = step(_Cutoff, glowingPart);
				// take out the normal texture part
				glowingPart *= (1 - maintexturePart);

#if INVERT

				// glowing bit that's a bit longer
				maintexturePart = 1 - smoothstep(0, _Smoothness, MovingPosOnModel + _DissolveColorWidth);
				maintexturePart = step(_Cutoff, maintexturePart);

				// normal texture
				glowingPart = 1 - smoothstep(0, _Smoothness, MovingPosOnModel);
				glowingPart = step(_Cutoff, glowingPart);
				// take out the normal texture part
				glowingPart *= (1 - maintexturePart);
#endif

				// Colorized Dissolve
				float4 glowingColored = glowingPart * _DisColor;

				// discard pixels beyond dissolving
				clip((maintexturePart + glowingPart) - 0.01);

				// main texture cutoff by dissolve
				float3 mainTexture = maintexturePart * c.rgb;

				// set main texture
				o.Albedo = mainTexture;

				// glowing dissolve

				//GLOW
				half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));

				o.Emission = (glowingColored * noisetexture) * _Brightness + _RimColor.rgb * pow(rim, _RimPower);

				// base settings
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
		ENDCG
		}
			FallBack "Diffuse"
}
