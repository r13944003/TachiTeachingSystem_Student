Shader "__Bison/NewSkin36/LODFadeout"
{
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" "Queue" = "Transparent"}
		LOD 200

		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
#pragma surface surf Lambert alpha

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

#pragma multi_compile _ LOD_FADE_CROSSFADE

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) *_Color;// *(1.0 - unity_LODFade.x);

			o.Albedo = c.rgb;
			o.Alpha = unity_LODFade.x;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
