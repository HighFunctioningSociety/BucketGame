Shader "Custom/MultiPassHeatDistort" {
    Properties{
        _Speed("Speed", Range(0, 10)) = 1.0
        _Distortion("Distortion", Range(0, 1)) = 0.1
        _RenderTexture("Render Texture", 2D) = "white" {}
    }

        SubShader{
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
            LOD 100

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 3.0
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                sampler2D _RenderTexture;
                float _Speed;
                float _Distortion;

                float4 frag(v2f i) : SV_Target {
                    // Sample render texture
                    float4 grab = tex2D(_RenderTexture, i.uv);

                    // Apply sin wave distortion to render texture
                    float2 distortedUV = i.uv + sin(i.uv.y * _Speed + grab.r * 10) * _Distortion;

                    // Sample and return the distorted render texture
                    return tex2D(_RenderTexture, distortedUV);
                }
                ENDCG
            }
    }
        FallBack "Diffuse"
}








