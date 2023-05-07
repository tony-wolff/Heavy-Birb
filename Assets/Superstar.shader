Shader "Unlit/Superstar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Toggle ("Toggle", Range(0,1)) = 0
        _ColorA ("ColorA", Color) = (1, 1, 1, 1)
        _ColorB ("ColorB", Color) = (1, 1, 1, 1)
        _ColorC ("ColorC", Color) = (1, 1, 1, 1)
        _ColorD ("ColorD", Color) = (1, 1, 1, 1)
        _ColorE ("ColorE", Color) = (1, 1, 1, 1)
        _ColorStart ("Color Start", Range(0, 1)) = 0
        _ColorEnd ("Color End", Range(0, 1)) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Transparent"
                "Queue"="Transparent"
        }
        Pass
        {
           //Blend One One //Additive blending
           // Blend SrcColor DstColor // Multiplicative blending
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #define TAU 6.28318530718

            float4 _ColorA;
            float4 _ColorB;
            float4 _ColorC;
            float4 _ColorD;
            float4 _ColorE;
            float _ColorStart;
            float _ColorEnd;
            float _Toggle;
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct interpolators
            {
                float3 normal : TEXCOORD0;
                float2 uv : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            interpolators vert (appdata v)
            {
                interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;
                return o;
            }

            float inverseLerp(float start, float end, float input){
                return (input - start)/(end - start);
            }

            void chooseBlend(out float4 blendA, out float4 blendB, float x){
                if(x >= 0 && x < 0.2){
                    blendA=_ColorA;
                    blendB=_ColorB;
                }
                else if (x>= 0.2 && x < 0.4){
                    blendA = _ColorB;
                    blendB = _ColorC;
                }
                else if (x>= 0.4 && x < 0.6){
                    blendA = _ColorC;
                    blendB = _ColorD;
                }
                else if (x>= 0.6 && x < 0.8){
                    blendA = _ColorD;
                    blendB = _ColorE;
                }
                else if (x>= 0.8 && x < 1){
                    blendA = _ColorE;
                    blendB = _ColorA;
                }
            }

            float4 frag (interpolators i) : SV_Target
            {
                //Blend between 2 colors based on the x uv coordinates
                // float4 outputColor = lerp(_ColorA, _ColorB, i.uv.x);
                // return outputColor;
                //frac = repeats pattern if go outside the range (0, 1)
                //saturate : clamp values between 0 and 1
                //float t = saturate ( inverseLerp(_ColorStart, _ColorEnd, i.uv.x));
                float xOffset = cos(i.uv.y*TAU*8)*0.01; //little offset along the y axis
                //Gives a wavy pattern that moves
                //TAU helps the repeats pattern
                //_Time.y is time in seconds
                //Cos repeats between 0 and 1
                float t = cos((i.uv.x + xOffset +_Time.y)*TAU*4)*0.5+0.5; //*0.5 + 0.5 get the domain between 0 and 1;

                float e = i.uv.x * 5.0;
                float4 blendA;
                float4 blendB;
                chooseBlend(blendA, blendB, i.uv.x);
                float4 rainbowColor = lerp(blendA, blendB, e - floor(e));
                rainbowColor += t*0.1;
                float4 tex = tex2D(_MainTex, i.uv);
                float4 outColor = tex;
                if(_Toggle)
                    outColor = lerp(tex, rainbowColor, 0.5);
                return outColor;
            }
            ENDCG
        }
    }
}
