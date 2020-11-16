Shader "Custom/TransitionMask"
{
    Properties
    {
        [PerRendererData]_MainTex("Image Map", 2D) = "white" {}

        [Header(Main)]
        [KeywordEnum(AlphaMask,Image)]_ImageType("Image Type", float) = 0

        //[Header(AlphaMask)]
        [Enum(TransitionMaskShader.MaskBlend)]_MaskBlendMode("Mask Blend Mode", float) = 0
        [Toggle]_ShowMaskImage("Show Mask Image", float) = 0
        [KeywordEnum(R,G,B,A)]_useMaskChannel("_useMaskChannel",float)=1
        _AlphaMaskTransition("Transition", Range(0.0, 1.0)) = 1.0
        _AlphaMaskEdge("Edge", Range(0.0, 1.0)) = 1.0
        [Toggle]_AlphaMaskInverse("Inverse", float) = 0

        //[Header(Image)]
        [Enum(TransitionMaskShader.ImageBlend)]_ImageBlendMode("Image Blend Mode", float) = 0

        _ImageIntensity("ImageIntensity", float) = 1.0

        [Header(Stencil)]
        [Enum(UnityEngine.Rendering.CompareFunction)]_StencilComp("Comparison", float) = 8
        _StencilRef("ReferenceValue", Range(0, 255)) = 1
        [Enum(UnityEngine.Rendering.StencilOp)]_StencilPass("Pass", float) = 2
        
        [Header(Depth)]
        [Toggle]_ZWriteParam("ZWrite", Float) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)]_ZTestMode("ZTest Mode", float) = 4
        
        //[Header(Blend)]
        [Enum(UnityEngine.Rendering.BlendMode)]_BlendSrc("Coler = SrcColor *", float) = 0
        [Enum(UnityEngine.Rendering.BlendMode)]_BlendDst("+ DstColor *", float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]_BlendAlphaSrc("Alpha = SrcAlpha *", float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]_BlendAlphaDst("+ DstAlpha *", float) = 0
        [Enum(UnityEngine.Rendering.BlendOp)]_BlendColorOp("BlendOp Color", float) = 0
        [Enum(UnityEngine.Rendering.BlendOp)]_BlendAlphaOp("BlendOp Alpha", float) = 0
    }

    SubShader
    {
        Tags{
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Pass
        {
            Blend[_BlendSrc][_BlendDst],[_BlendAlphaSrc][_BlendAlphaDst]
            BlendOp[_BlendColorOp],[_BlendAlphaOp]
            
            ZWrite[_ZWriteParam]
            ZTest[_ZTestMode]

            Stencil
            {
                Ref[_StencilRef]
                Comp[_StencilComp]
                Pass[_StencilPass]
            }

            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma shader_feature _IMAGETYPE_ALPHAMASK
            #pragma shader_feature _USEMASKCHANNEL_R _USEMASKCHANNEL_G _USEMASKCHANNEL_B _USEMASKCHANNEL_A

            sampler2D _MainTex;
            half _ImageIntensity;
            half _AlphaMaskTransition;
            half _AlphaMaskEdge;
            half _AlphaMaskInverse;
            half _useMaskChannel;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                half2 uv0 : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos: SV_POSITION;
                float4 color : COLOR0;
                half2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv0;
                o.color = v.color;

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                half4 color = tex2D(_MainTex, i.uv);
                color *= i.color;
                color = saturate(color * _ImageIntensity);

#if _IMAGETYPE_ALPHAMASK
                //mask
    #if _USEMASKCHANNEL_R
                color.a = smoothstep(_AlphaMaskTransition, _AlphaMaskTransition + _AlphaMaskEdge, 1 - color.r * _AlphaMaskTransition);
    #elif _USEMASKCHANNEL_G
                color.a = smoothstep(_AlphaMaskTransition, _AlphaMaskTransition + _AlphaMaskEdge, 1 - color.g * _AlphaMaskTransition);
    #elif _USEMASKCHANNEL_B
                color.a = smoothstep(_AlphaMaskTransition, _AlphaMaskTransition + _AlphaMaskEdge, 1 - color.b * _AlphaMaskTransition);
    #elif _USEMASKCHANNEL_A
                color.a = smoothstep(_AlphaMaskTransition, _AlphaMaskTransition + _AlphaMaskEdge, 1 - color.a * _AlphaMaskTransition);
    #endif
                color.a = _AlphaMaskInverse ? color.a : 1 - color.a;
                color.a *= i.color.a;
#endif

                return color;
            }
            ENDCG
        }
    }
    CustomEditor "TransitionMask_GUI"
}
