Shader "Custom/SpriteBlurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Float) = 0.005
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _BlurSize;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                fixed4 color = tex2D(_MainTex, uv) * 0.227;

                color += tex2D(_MainTex, uv + float2(_BlurSize, 0)) * 0.316;
                color += tex2D(_MainTex, uv - float2(_BlurSize, 0)) * 0.316;
                color += tex2D(_MainTex, uv + float2(0, _BlurSize)) * 0.316;
                color += tex2D(_MainTex, uv - float2(0, _BlurSize)) * 0.316;

                return color;
            }
            ENDCG
        }
    }
}
