Shader "Custom/ChromaticAberration"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Intensity ("Intensity", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
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
            float _Intensity;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 获取主纹理颜色
                fixed4 color = tex2D(_MainTex, i.uv);
                
                // 应用色差
                fixed4 colorR = tex2D(_MainTex, i.uv + float2(_Intensity * 0.01, 0));
                fixed4 colorB = tex2D(_MainTex, i.uv - float2(_Intensity * 0.01, 0));
                
                // 返回带色差的颜色
                return fixed4(colorR.r, color.g, colorB.b, color.a);
            }
            ENDCG
        }
    }
}
