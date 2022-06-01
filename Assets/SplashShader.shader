Shader "Custom/SplashShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_PainterPosition("PainterPosition", Float)
		_Radius("Radius", Range(0,1)) = 0.0
		_Hardness("Hardness", Range(0,1)) = 0.0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input
			{
				float2 uv_MainTex;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}

			v2f vert(appdata v)
			{
				v2f o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.uv = uv;
				float4 uv = float4(0, 0, 0, 1);
				uv.xy = (v.uv.xy * 2 - 1) * float2(1, _ProjectionParams.x);
				o.vertex = uv;
				return o;
			}

			float mask(float3 position, float3 center, float radius, float hardness)
			{
				float m = distance(center, position);
				return 1 - smoothstep(radius * hardness, radius, m);
			}

			float frag(v2f i) : SV_Target
			{
				float m = mask(i.worldPos, _PainterPosition, _Radius, _Hardness);
				float endge = m * _Strength;
				return lerp(float4(0, 0, 0, 0), float4(1, 0, 0, 1), edge);
			}
			ENDCG
		}
			FallBack "Diffuse"
}
