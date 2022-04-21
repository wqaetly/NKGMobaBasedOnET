// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Werewolf/Indicators/FillLinear"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _FillColor ("Fill Color", Color) = (1,1,1,1)
        _ProjectTex ("Shape", 2D) = "" {}
        _Fill ("Fill", Range (0,1)) = 0
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

            fixed _Fill;
            fixed4 _MainColor;
            fixed4 _FillColor;
            fixed4 _ProjectTex_ST;
            sampler2D _ProjectTex;

            struct vInput
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct vOutput
            {
                float4 pos : SV_POSITION;
                float2 uvMain : TEXCOORD0;
            };

            vOutput vert(vInput v)
            {
                vOutput o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uvMain = TRANSFORM_TEX(v.texcoord, _ProjectTex);
                return o;
            }

            fixed4 frag(vOutput i) : SV_Target
            {
                fixed4 main = tex2D(_ProjectTex, i.uvMain);
                fixed4 fill = tex2D(_ProjectTex, i.uvMain);

                main *= _MainColor;
                fill *= _FillColor;

                fixed mainBlit = max(0, sign(i.uvMain.y - _Fill));
                fixed fillBlit = max(0, sign(_Fill - i.uvMain.y));

                fixed4 res = fixed4(0, 0, 0, 0);
                res += main * fixed4(mainBlit, mainBlit, mainBlit, mainBlit);
                res += fill * fixed4(fillBlit, fillBlit, fillBlit, fillBlit);

                return res;
            }
            ENDCG
        }
    }
}