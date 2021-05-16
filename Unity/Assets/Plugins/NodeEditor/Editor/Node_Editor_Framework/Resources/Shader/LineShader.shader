Shader "Hidden/LineShader"
{
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalRenderPipeline"
            "RenderType" = "Opaque"
        }
        
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back
            ZWrite Off

            HLSLINCLUDE
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma vertex vert
            #pragma fragment frag

            uniform half4 _LineColor;

            CBUFFER_START(UnityPerMaterial)

            TEXTURE2D(_LineTexture);
            SAMPLER(sampler_LineTexture);

            CBUFFER_END

            struct attribute
            {
                float3 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            ENDHLSL
            HLSLPROGRAM
            v2f vert(attribute attribute)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(attribute.vertex);
                o.texcoord = attribute.texcoord;
                return o;
            }

            float4 frag(v2f i) : COLOR
            {
                return _LineColor * SAMPLE_TEXTURE2D(_LineTexture, sampler_LineTexture, i.texcoord);
            }
            ENDHLSL
        }
    }
}