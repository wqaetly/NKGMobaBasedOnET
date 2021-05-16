Shader "NKGMoba/Darius_R_RimLightOffset"
{
    Properties
    {
        _MainTex("MainTex",2D)="white"{}
        _OffsetDir("OffsetDir", Vector) = (0,0,0,0)
        _OffsetIntensity("OffsetIntensity", Float) = 1
        _TimeScale("TimeScale", Float) = 1
    }

    SubShader
    {
        Tags {"RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent" "RenderType" = "Transparent"}
        
        Cull Off
        
        Pass
        {
            Tags {"LightMode" = "UniversalForward"}
            //关闭深度写入
            ZWrite Off
            //将该片元着色器产生的颜色混合因子设置为SrcAlpha
            //将已经存在与颜色缓冲中的颜色混合因子设置为OneMinusSrcAlpha
            Blend SrcAlpha OneMinusSrcAlpha
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)

            float4 _MainTex_TexelSize;
            float3 _OffsetDir;
            float _OffsetIntensity;
            float _TimeScale;

            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct Varyings
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal: TEXCOORD1;
                float3 worldViewDir: TEXCOORD2;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                input.positionOS.xyz += _OffsetDir * _OffsetIntensity * (_Time.y % _TimeScale);
                VertexPositionInputs vertexPositionInput = GetVertexPositionInputs(input.positionOS);

                output.vertex = vertexPositionInput.positionCS;
                output.uv = input.uv;
                //使用顶点变换矩阵的逆转置矩阵对法线进行相同的变换，
                //因此我们要先得到模型空间到世界空间的变换矩阵的逆矩阵unity_WorldToObject
                //然后通过调换他在mul函数中的位置，得到和转置矩阵相同的矩阵乘法。
                //由于法线是一个三维矢量，我们只需要截取unity_WorldToObject前三行的前3列即可
                output.worldNormal = mul(input.normal, (float3x3)unity_WorldToObject);
                output.worldViewDir = _WorldSpaceCameraPos.xyz - vertexPositionInput.positionWS;
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                //将法线归一化
				float3 worldNormal = normalize(input.worldNormal);
				//把视线方向归一化
				float3 worldViewDir = normalize(input.worldViewDir);
				//计算视线方向与法线方向的夹角，夹角越大，dot值越接近0，说明视线方向越偏离该点，也就是平视，该点越接近边缘
				float rim = max(0, dot(worldViewDir, worldNormal));
				//计算rimLight
				half4 rimColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(clamp(rim, 0.1f, 0.3f), 0.5f));
                rimColor.a -= (_Time.y % _TimeScale);
				return rimColor;
            }
            ENDHLSL
        }
    }
}