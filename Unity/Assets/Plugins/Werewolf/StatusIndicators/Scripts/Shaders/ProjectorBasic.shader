// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Werewolf/Indicators/Basic"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Shape", 2D) = "" {}
    }

    Subshader
    {
        Tags
        {
            "Queue"="Transparent"
        }
        Pass
        {
            ZWrite Off
            AlphaTest Greater 0
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha
            Offset -1, -1

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _MainColor;
            sampler2D _MainTex;
            fixed4 _MainTex_ST;

            struct a2v
            {
                float2 uvMain : TEXCOORD0;
                float4 pos : POSITION;
            };

            struct v2f
            {
                float2 uvMain : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert(a2v v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uvMain = TRANSFORM_TEX(v.uvMain, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 texS = tex2D(_MainTex, i.uvMain);
                texS.rgba *= _MainColor.rgba;
                fixed4 res = texS;
                return res;
            }
            ENDCG
        }
    }
}