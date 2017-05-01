Shader "Custom/HoneyCombShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}

		_OverlayColour("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};

		fixed4 _Color;
		fixed4 _OverlayColour;

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb + ((_Color + _OverlayColour) / 1.4);

			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap)) / 5;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
