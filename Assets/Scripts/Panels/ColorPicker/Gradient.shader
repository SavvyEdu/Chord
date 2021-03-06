Shader "Unlit/Gradient"
{
    Properties
    {
		[PerRendererData] _MainTex ("Texture", 2D) = "white" {}
		_Color0("_Color0", Color) = (0,0,0,1)
		_Color1("_Color1", Color) = (1,1,1,1)

			// required for UI.Mask
			 _StencilComp("Stencil Comparison", Float) = 8
			 _Stencil("Stencil ID", Float) = 0
			 _StencilOp("Stencil Operation", Float) = 0
			 _StencilWriteMask("Stencil Write Mask", Float) = 255
			 _StencilReadMask("Stencil Read Mask", Float) = 255
			 _ColorMask("Color Mask", Float) = 15
	}
		SubShader
		{
			Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
			LOD 100
			ZWrite Off
			Blend One OneMinusSrcAlpha

			// required for UI.Mask
		 Stencil
		 {
			 Ref[_Stencil]
			 Comp[_StencilComp]
			 Pass[_StencilOp]
			 ReadMask[_StencilReadMask]
			 WriteMask[_StencilWriteMask]
		 }
		  ColorMask[_ColorMask]

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				fixed4 _Color0;
				fixed4 _Color1;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture for alpha
                fixed4 col = tex2D(_MainTex, i.uv);
				return lerp(_Color0, _Color1, i.uv.x) * col.a;
            }
            ENDCG
        }
    }
}
