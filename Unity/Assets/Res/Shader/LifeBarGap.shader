//Code from https://blog.csdn.net/cyf649669121/article/details/82117638
Shader "NKGMoba/LifeBarGap"
{
    Properties
    {
        PerSplitWidth("分割块宽度：",float) = 10
        GapLineWidth("分割线宽度：",float) = 3
        [HideInInspector]
        BlackColor("BlackColor",Color) = (0,0,0,1)
        UVFactor("UV缩放系数", float) = 1
        UVStart("UV起始点", float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off
        ZWrite Off
        ZTest Off
        Blend SrcAlpha OneMinusSrcAlpha

        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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

            CBUFFER_START(UnityPerMaterial)

            float PerSplitWidth;
            float GapLineWidth;
            half4 BlackColor;
            float UVFactor;
            float UVStart;

            CBUFFER_END

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half virtualHealthBarWidth = (i.uv.x - UVStart)*100 * UVFactor;
                half result = step(PerSplitWidth - GapLineWidth, virtualHealthBarWidth % PerSplitWidth);

                half bigGapResult = step((virtualHealthBarWidth + PerSplitWidth) / PerSplitWidth % 10, 1);

                half secondResult = step(PerSplitWidth - GapLineWidth * 2,
                                         virtualHealthBarWidth % PerSplitWidth) * bigGapResult;
                //return half4(secondResult,0,0,1);
                // call discard
                // if ZWrite is Off, clip() is fast enough on mobile, because it won't write the DepthBuffer, so no GPU pipeline stall(confirmed by ARM staff).
                // 以每100生命值作为间隔
                clip((result + secondResult - 1) + (i.uv.y - 0.4 + bigGapResult * 0.4));
                return BlackColor;
            }
            ENDHLSL
        }
    }
}