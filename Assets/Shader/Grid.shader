// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Grid"
{
    Properties
    {
        _Color ("Color (RGBA)", Color) = (1, 1, 1, 1) 
        _Size ("Size", Range(0.0, 1.0)) = 0.1
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float3 worldPos : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Size;

            float mod(float x, float y)
            {
                return x - y * floor(x/y);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //basically just a mod function with offset + it's flipped version
                //will be set to 0 with spikes of 1 at whole numbers. spikes are _Size units wide
                float aX = mod(i.worldPos.x + ( _Size / 2 ), 1);          
                float bX = mod(( _Size / 2 ) - i.worldPos.x, 1);
                float x = -(aX + bX) + _Size + 1;
                
                float aY = mod(i.worldPos.y + ( _Size / 2 ), 1);          
                float bY = mod(( _Size / 2 ) - i.worldPos.y, 1);
                float y = -(aY + bY) + _Size + 1;

                fixed4 col = _Color;
                col.a = max(x,y);
                return col;
            }
            ENDCG
        }
    }


}
