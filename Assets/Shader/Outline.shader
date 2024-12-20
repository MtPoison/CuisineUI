Shader "Custom/HighlightShader"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1,1,1,1)
        _OutlineColor("Outline Color", Color) = (1,1,0,1)
        _OutlineWidth("Outline Width", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Include URP's core shader library
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Shader properties
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
            };

            // Uniforms
            float4 _BaseColor;
            float4 _OutlineColor;
            float _OutlineWidth;

            Varyings vert(Attributes v)
            {
                Varyings o;
                float3 worldPos = TransformObjectToWorld(v.positionOS.xyz);
                o.positionCS = TransformWorldToHClip(worldPos);
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                o.viewDirWS = GetCameraPositionWS() - worldPos;
                return o;
            }

            float4 frag(Varyings i) : SV_Target
            {
                // Normalize inputs
                float3 normalWS = normalize(i.normalWS);
                float3 viewDirWS = normalize(i.viewDirWS);

                // Fresnel effect for highlight
                float fresnel = pow(1.0 - dot(viewDirWS, normalWS), _OutlineWidth);

                // Combine base color and outline
                float3 baseColor = _BaseColor.rgb;
                float3 outlineColor = _OutlineColor.rgb * fresnel;

                return float4(baseColor + outlineColor, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack "Standard"
}
