Shader "Custom/GlitchEffectURP"
{
    Properties
    {
        _BaseMap ("Texture", 2D) = "white" {}
        _GlitchStrength ("Glitch Strength", Range(0, 1)) = 0.1
        _Speed ("Glitch Speed", Range(0, 10)) = 1.0
        _GlitchAmount ("Glitch Amount", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalRenderPipeline" }
        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionHCS : SV_POSITION;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            float _GlitchStrength;
            float _Speed;
            float _GlitchAmount;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float glitchOffset = sin(_Time.y * _Speed + IN.positionHCS.x * 10.0) * _GlitchStrength * _GlitchAmount;
                float2 uv = IN.uv;
                uv.x += glitchOffset;

                half4 col = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
                if (col.a < 0.1) discard;
                return col;
            }
            ENDHLSL
        }
    }
}
