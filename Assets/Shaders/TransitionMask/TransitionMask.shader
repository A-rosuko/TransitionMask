Shader "Custom/TransitionMask"
{
    Properties
    {
        //[Header(ImageType)]
        [KeywordEnum(AlphaMask,Image)] _ImageType("Image Type", float) = 0
        [Enum(TransitionMaskBlendPreset.MaskBlend)]_MaskBlendMode("Mask Blend Mode", float) = 0
        [Enum(TransitionMaskBlendPreset.ImageBlend)]_ImageBlendMode("Image Blend Mode", float) = 0
        [Toggle]_ShowMaskImage("Show Mask Image", float) = 0
        [Toggle(MULTIPLY_ALPHA)]_MultiplyAlpha("Multiply Alpha Blend", float) = 0

        //[Header(Blend)]
        [Enum(UnityEngine.Rendering.BlendMode)]_BlendSrc("Coler = SrcColor *", float) = 0
        [Enum(UnityEngine.Rendering.BlendMode)]_BlendDst("+ DstColor *", float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]_BlendAlphaSrc("Alpha = SrcAlpha *", float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]_BlendAlphaDst("+ DstAlpha *", float) = 0
        [Enum(UnityEngine.Rendering.BlendOp)]_BlendColorOp("BlendOp Color", float) = 0
        [Enum(UnityEngine.Rendering.BlendOp)]_BlendAlphaOp("BlendOp Alpha", float) = 0

        //[Header(Main)]
        [PerRendererData]_MainTex("Image Map", 2D) = "white" {}
        [PerRendererData]_Color("Color", Color) = (1,1,1,1)

        [Header(MaskSetting)]
        [Toggle(USE_ALPHA_MASK_MAP)]_UseAlphaMaskMap("Use Alpha Mask Map", float) = 0
        _AlphaMaskMap("Alpha Mask Map", 2D) = "white" {}

        [KeywordEnum(R,G,B,A)]_useMaskChannel("_useMaskChannel",float) = 0
        _AlphaMaskTransition("Transition", Range(0.0, 1.0)) = 1.0
        _AlphaMaskEdge("Edge", Range(0.0, 1.0)) = 1.0
        [Toggle]_AlphaMaskInverse("Inverse", float) = 0
        _ImageIntensity("ImageIntensity", float) = 1.0

        _MainSpeedU("SpeedU", float) = 0.0
        _MainSpeedV("SpeedV", float) = 0.0
        _Scale("Scale", float) = 1.0
        [Toggle(PIXELSNAP_ON)]_PixelSnap("Pixel Snap", Float) = 0

        [Header(Clip)]
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", float) = 0
        _Cutoff("Alpha Cutoff", float) = 0.0
        [Toggle]_ClipInverse("Inverse", float) = 0

        [Toggle]_AlphaToMask("_AlphaToMask", Float) = 0

        [Header(Other)]
        [Enum(UnityEngine.Rendering.CullMode)]_CullMode("Cull Mode", Float) = 2
        _ColorMask("Color Mask", Float) = 15

        [Header(Stencil)]
        [Enum(UnityEngine.Rendering.CompareFunction)]_StencilComp("Comparison", float) = 8
        _StencilRef("ReferenceValue", Range(0, 255)) = 1
        [Enum(UnityEngine.Rendering.StencilOp)]_StencilPass("Pass", float) = 2

        [Header(Depth)]
        [Toggle]_ZWriteParam("ZWrite", Float) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)]_ZTestMode("ZTest Mode", float) = 4
    }

    SubShader
    {
        Tags{
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        Pass
        {
            Blend[_BlendSrc][_BlendDst],[_BlendAlphaSrc][_BlendAlphaDst]
            BlendOp[_BlendColorOp],[_BlendAlphaOp]
            AlphaToMask[_AlphaToMask]

            Cull[_CullMode]

            Stencil
            {
                Ref[_StencilRef]
                Comp[_StencilComp]
                Pass[_StencilPass]
            }
            ColorMask[_ColorMask]

            ZWrite[_ZWriteParam]
            ZTest[_ZTestMode]

            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _IMAGETYPE_ALPHAMASK
            #pragma shader_feature _USEMASKCHANNEL_R _USEMASKCHANNEL_G _USEMASKCHANNEL_B _USEMASKCHANNEL_A
            #pragma shader_feature UNITY_UI_ALPHACLIP
            #pragma shader_feature MULTIPLY_ALPHA
            #pragma shader_feature USE_ALPHA_MASK_MAP
            #pragma shader_feature PIXELSNAP_ON

            #pragma multi_compile_instancing
            #pragma instancing_options procedural:vertInstancingSetup

            #include "UnityCG.cginc"
            #include "UnityStandardParticleInstancing.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            half4 _MainTex_TexelSize;

            sampler2D  _AlphaMaskMap;
            half4  _AlphaMaskMap_ST;

            half _AlphaMaskTransition;
            half _AlphaMaskEdge;
            half _AlphaMaskInverse;
            half _ImageIntensity;

            half _MainSpeedU;
            half _MainSpeedV;
            half _Scale;

            half _Cutoff;
            half _ClipInverse;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
            UNITY_INSTANCING_BUFFER_END(Props)
            
            fixed4 _TextureSampleAdd;

            struct appdata
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv0 : TEXCOORD0;
            };

            struct v2f
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 pos: SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.pos = UnityObjectToClipPos(v.vertex);

#ifdef PIXELSNAP_ON
                o.pos = UnityPixelSnap(o.pos);
#endif
                o.uv = TRANSFORM_TEX(v.uv0, _AlphaMaskMap);

                //scroll
                o.uv += half2(_MainSpeedU, _MainSpeedV) * _Time.r;

                //Scale
                o.uv = _Scale == 0.0 ? 0.0 : (o.uv - 0.5) / _Scale + 0.5;

                o.color = v.color * UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
                vertInstancingColor(o.color);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
                half4 color = (UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv) + _TextureSampleAdd) * i.color;
                color = saturate(color * _ImageIntensity);

#if !USE_ALPHA_MASK_MAP
                //Image
    #if _USEMASKCHANNEL_R
                color.a = smoothstep(_AlphaMaskTransition, _AlphaMaskTransition + _AlphaMaskEdge, 1 - color.r * _AlphaMaskTransition * 2);
    #elif _USEMASKCHANNEL_G
                color.a = smoothstep(_AlphaMaskTransition, _AlphaMaskTransition + _AlphaMaskEdge, 1 - color.g * _AlphaMaskTransition * 2);
    #elif _USEMASKCHANNEL_B
                color.a = smoothstep(_AlphaMaskTransition, _AlphaMaskTransition + _AlphaMaskEdge, 1 - color.b * _AlphaMaskTransition * 2);
    #elif _USEMASKCHANNEL_A
                color.a = smoothstep(_AlphaMaskTransition, _AlphaMaskTransition + _AlphaMaskEdge, 1 - color.a * _AlphaMaskTransition * 2);
    #endif
                color.a = _AlphaMaskInverse ? color.a : 1 - color.a;
#else
                //Mask
                half4 mask = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_AlphaMaskMap, i.uv);
    #if _USEMASKCHANNEL_R
                mask.a = smoothstep(_AlphaMaskTransition, _AlphaMaskTransition + _AlphaMaskEdge, 1 - mask.r * _AlphaMaskTransition * 2);
    #elif _USEMASKCHANNEL_G
                mask.a = smoothstep(_AlphaMaskTransition, _AlphaMaskTransition + _AlphaMaskEdge, 1 - mask.g * _AlphaMaskTransition * 2);
    #elif _USEMASKCHANNEL_B
                mask.a = smoothstep(_AlphaMaskTransition, _AlphaMaskTransition + _AlphaMaskEdge, 1 - mask.b * _AlphaMaskTransition * 2);
    #elif _USEMASKCHANNEL_A
                mask.a = smoothstep(_AlphaMaskTransition, _AlphaMaskTransition + _AlphaMaskEdge, 1 - mask.a * _AlphaMaskTransition * 2);
    #endif
                color.a *= _AlphaMaskInverse ? mask.a : 1 - mask.a;
#endif

#ifdef UNITY_UI_ALPHACLIP
                clip((_ClipInverse ? -color.a + _Cutoff : color.a - _Cutoff) - 1e-6);
#endif

#ifdef MULTIPLY_ALPHA
                color.rgb *= color.a;
#endif

                return color;
            }
            ENDCG
        }
    }
    CustomEditor "TransitionMask_GUI"
}
