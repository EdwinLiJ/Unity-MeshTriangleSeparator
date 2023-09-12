Shader "Unlit/GlassesDemo"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Distance ("Distance", Range(0, 10)) = 1
        _LifeTime ("LifeTime", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
      
        Cull off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct appdata
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : TEXCOORD1;
            };

            float _Distance;
            float _LifeTime;
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float GetRandom(float2 uv, float salt)
            {
                uv += float2(salt, salt);
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            float2 Rotate(half2 position, half2 pivot, half angleRange)
            {
                // 用于计算轨迹的固定参数，越低旋转越顺畅
                float angle = angleRange;
                float cosAngle = cos(angle);
                float sinAngle = sin(angle);
                float2x2 rot = half2x2(cosAngle, -sinAngle, sinAngle, cosAngle);
                position -= pivot;
                float2 output = mul(rot, position);
                output += pivot;
                return output;
            }

            v2f vert (appdata v)
            {
                v2f o;
                float index = v.color.r;
                float3 endPosition = v.positionOS + _Distance * v.normalOS * GetRandom(index, index);
                endPosition.xz = Rotate(endPosition.xz, 0, (1 - _LifeTime) * 20 * GetRandom(index + 1, index + 1));
      
                float3 positionOS = lerp(v.positionOS, endPosition, _LifeTime);
                o.positionCS = TransformObjectToHClip(positionOS);
                o.uv = v.uv;
                o.color = index;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 result = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                return result;
            }
            ENDHLSL
        }
    }
}
