Shader "BlackBird/Outline"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _LineWidth("Outline Width", Float) = 0
        _LineColor("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineEmission("Outline Emission", Float) = 0
        _OutlineTexture("Outline Texture", 2D) = "white" { }
        _Speed("Speed", Float) = 0.05
        [Toggle(_LIT)] _Lit("Flat Lit?", Float) = 1
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4
        _Clip("Clipping", Range(0, 1.001)) = 0.5
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }
        SubShader
        {
            Tags { "RenderType" = "TransparentCutout" }
            
        
        //    Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM

            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf Standard fullforwardshadows

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0
           // #pragma vertex vert
            //#pragma fragment frag
            sampler2D _MainTex;

            struct Input
            {
                float2 uv_MainTex;
            };

            half _Glossiness;
            half _Metallic;
            fixed4 _Color;

            // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
            // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
            // #pragma instancing_options assumeuniformscaling
            UNITY_INSTANCING_BUFFER_START(Props)
                // put more per-instance properties here
            UNITY_INSTANCING_BUFFER_END(Props)

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                // Albedo comes from a texture tinted by color
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb;
                // Metallic and smoothness come from slider variables
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Alpha = c.a;
            }
            ENDCG
        
            Pass
        {
            Name "MyShader"
            Tags { "LightMode" = "ForwardBase" }
            Stencil
            {
                Ref 901 Comp Equal Pass keep
            }
            ZWrite On ZTest Always Cull Off
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"
            struct appdata
            {
                float4 vertex: POSITION; float2 uv: TEXCOORD0;
            };
            struct v2f
            {
                UNITY_FOG_COORDS(1) float4 vertex: SV_POSITION;
            };
            v2f vert(appdata v)
            {
                v2f o; o.vertex = UnityObjectToClipPos(v.vertex);	return o;
            }
            float4 frag(v2f i) : COLOR
            {
                return float4(0.0 / 255.0, 191.0 / 255.0, 255.0 / 255.0, 1);
            }
            ENDCG

        }

            Pass
            {
               
                Tags {  }
                Cull Front
                CGPROGRAM

                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
                #pragma fragmentoption ARB_precision_hint_fastest
                #pragma multi_compile_shadowcaster
                #pragma only_renderers d3d9 d3d11 glcore gles
                #pragma target 3.0
                uniform float _LineWidth;
                uniform float _OutlineEmission;
                uniform float4 _LineColor;
                uniform sampler2D _OutlineTexture; uniform float4 _OutlineTexture_ST;
                uniform float _Speed;
                struct VertexInput
                {
                    float4 vertex: POSITION;
                    float3 normal: NORMAL;
                    float2 texcoord0: TEXCOORD0;
                };
                struct VertexOutput
                {
                    float4 pos: SV_POSITION;
                    float2 uv0: TEXCOORD0;
                };
                VertexOutput vert(VertexInput v)
                {
                    VertexOutput o = (VertexOutput)0;
                    o.uv0 = v.texcoord0;
                    o.pos = UnityObjectToClipPos(float4(v.vertex.xyz + v.normal * _LineWidth / 10000, 1));
                    return o;
                }
                float4 frag(VertexOutput i, float facing : VFACE) : COLOR
                {

                    clip(_LineWidth - 0.001);
                    float isFrontFace = (facing >= 0 ? 1 : 0);
                    float faceSign = (facing >= 0 ? 1 : -1);
                    fixed4 col = fixed4(tex2D(_OutlineTexture, TRANSFORM_TEX((i.uv0 + (_Speed * _Time.g)), _OutlineTexture)).rgb, 0) * _LineColor;
                    return col + col * _OutlineEmission;
                }
                ENDCG
            }
            Pass
        {
            Name "Shadow"
            Tags { "LightMode" = "ShadowCaster" }
            Offset 1, 1
            Cull Off

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
           // #define UNITY_PASS_SHADOWCASTER
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "AutoLight.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            struct VertexInput
            {
                float4 vertex: POSITION;
                float2 texcoord0: TEXCOORD0;
            };
            struct VertexOutput
            {
                V2F_SHADOW_CASTER;
                float2 uv0: TEXCOORD1;
            };
            VertexOutput vert(VertexInput v)
            {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR
            {
                float isFrontFace = (facing >= 0 ? 1 : 0);
                float faceSign = (facing >= 0 ? 1 : -1);
                float4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(i.uv0, _MainTex));
                float SurfaceAlpha = _MainTex_var.a;
                clip(SurfaceAlpha - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG

        }

        }
}