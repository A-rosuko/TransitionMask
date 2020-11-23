using UnityEngine;
using UnityEngine.Rendering;
using TransitionMaskBlendPreset;

namespace UnityEditor
{
    internal class TransitionMask_GUI : ShaderGUI
    {
        //GUI Text
        private static class Styles
        {
            public static GUIContent imageTypeText = new GUIContent("Image Type", "Image Type");
            public static GUIContent maskBlendModeText = new GUIContent("Blend Mode", "Blend Mode");
            public static GUIContent imageBlendModeText = new GUIContent("BlendMode", "BlendMode");
            public static GUIContent showMaskImageText = new GUIContent("Show Mask Image", "Show Mask Image");
            public static GUIContent multiplyAlphaText = new GUIContent("Multiply Alpha", "Multiply Alpha");

            public static GUIContent blendSrcText = new GUIContent("SrcFactor", "SrcFactor");
            public static GUIContent blendDstText = new GUIContent("DstFactor", "DstFactor");
            public static GUIContent blendAlphaSrcText = new GUIContent("SrcFactorA", "SrcFactorA");
            public static GUIContent blendAlphaDstText = new GUIContent("DstFactorA", "DstFactorA");
            public static GUIContent blendColorOpText = new GUIContent("ColerOp", "ColerOp");
            public static GUIContent blendAlphaOpText = new GUIContent("AlphaOp", "AlphaOp");

            public static GUIContent useAlphaMaskMapText = new GUIContent("Use Alpha Mask Map", "Use Alpha Mask Map");
            public static GUIContent alphaMaskMapText = new GUIContent("Alpha Mask Map", "Alpha Mask Map");

            public static GUIContent useMaskChannelText = new GUIContent("Use Mask Channel", "Use Mask Channel");
            public static GUIContent alphaMaskTransitonText = new GUIContent("Transition", "Transition");
            public static GUIContent alphaMaskEdgeText = new GUIContent("Mask Edge", "Mask Edge");
            public static GUIContent alphaMaskInverseText = new GUIContent("Alpha Inverse", "Alpha Inverse");
            public static GUIContent imageIntensityText = new GUIContent("Image Intensity", "Image Intensity");

            public static GUIContent mainSpeedUText = new GUIContent("Speed U", "Speed U");
            public static GUIContent mainSpeedVText = new GUIContent("Speed V", "Speed V");
            public static GUIContent scaleText = new GUIContent("Scale", "Scale");
            public static GUIContent pixelSnapText = new GUIContent("PixelSnap", "PixelSnap");

            public static GUIContent useUIAlphaClipText = new GUIContent("Use Alpha Clip", "Use Alpha Clip");
            public static GUIContent cutoffText = new GUIContent("Cut off", "Cut off");
            public static GUIContent clipInverseText = new GUIContent("Clip Inverse", "Clip Inverse");

            public static GUIContent alphaToMaskText = new GUIContent("alphaToMask", "alphaToMask");

            public static GUIContent cullModeText = new GUIContent("Cull Mode", "Cull Mode");

            public static GUIContent colorMaskText = new GUIContent("ColorMask", "ColorMask");

            public static GUIContent stencilCompText = new GUIContent("Comp", "Comp");
            public static GUIContent stencilRefText = new GUIContent("Ref", "Ref");
            public static GUIContent stencilPassText = new GUIContent("Pass", "Pass");

            public static GUIContent zWriteParamText = new GUIContent("ZWrite Param", "ZWrite Param");
            public static GUIContent zTestModeText = new GUIContent("ZTest Mode", "ZTest Mode");
        }
        //Properties
        MaterialProperty imageType;
        MaterialProperty maskBlendMode;
        MaterialProperty imageBlendMode;
        MaterialProperty showMaskImage;
        MaterialProperty multiplyAlpha;

        MaterialProperty blendSrc;
        MaterialProperty blendDst;
        MaterialProperty blendAlphaSrc;
        MaterialProperty blendAlphaDst;
        MaterialProperty blendColorOp;
        MaterialProperty blendAlphaOp;

        MaterialProperty useAlphaMaskMap;
        MaterialProperty alphaMaskMap;

        MaterialProperty useMaskChannel;
        MaterialProperty alphaMaskEdge;
        MaterialProperty alphaMaskTransiton;
        MaterialProperty alphaMaskInverse;
        MaterialProperty imageIntensity;

        MaterialProperty mainSpeedU;
        MaterialProperty mainSpeedV;
        MaterialProperty scale;
        MaterialProperty pixelSnap;

        MaterialProperty useUIAlphaClip;
        MaterialProperty cutoff;
        MaterialProperty clipInverse;
        MaterialProperty alphaToMask;

        MaterialProperty cullMode;

        MaterialProperty colorMask;

        MaterialProperty stencilComp;
        MaterialProperty stencilRef;
        MaterialProperty stencilPass;

        MaterialProperty zWriteParam;
        MaterialProperty zTestMode;

        MaterialEditor m_MaterialEditor;

        private bool _isBlendOpen = false;

        //Properties Update
        public void FindProperties(MaterialProperty[] props)
        {
            imageType = FindProperty("_ImageType", props);
            maskBlendMode = FindProperty("_MaskBlendMode", props);
            imageBlendMode = FindProperty("_ImageBlendMode", props);
            showMaskImage = FindProperty("_ShowMaskImage", props);
            multiplyAlpha = FindProperty("_MultiplyAlpha", props);

            blendSrc = FindProperty("_BlendSrc", props);
            blendDst = FindProperty("_BlendDst", props);
            blendAlphaSrc = FindProperty("_BlendAlphaSrc", props);
            blendAlphaDst = FindProperty("_BlendAlphaDst", props);
            blendColorOp = FindProperty("_BlendColorOp", props);
            blendAlphaOp = FindProperty("_BlendAlphaOp", props);

            useAlphaMaskMap = FindProperty("_UseAlphaMaskMap", props);
            alphaMaskMap = FindProperty("_AlphaMaskMap", props);

            useMaskChannel = FindProperty("_useMaskChannel", props);
            alphaMaskEdge = FindProperty("_AlphaMaskEdge", props);
            alphaMaskTransiton = FindProperty("_AlphaMaskTransition", props);
            alphaMaskInverse = FindProperty("_AlphaMaskInverse", props);
            imageIntensity = FindProperty("_ImageIntensity", props);

            mainSpeedU = FindProperty("_MainSpeedU", props);
            mainSpeedV = FindProperty("_MainSpeedV", props);
            scale = FindProperty("_Scale", props);
            pixelSnap = FindProperty("_PixelSnap", props);

            useUIAlphaClip = FindProperty("_UseUIAlphaClip", props);
            cutoff = FindProperty("_Cutoff", props);
            clipInverse = FindProperty("_ClipInverse", props);
            alphaToMask = FindProperty("_AlphaToMask", props);

            cullMode = FindProperty("_CullMode", props);

            colorMask = FindProperty("_ColorMask", props);

            stencilComp = FindProperty("_StencilComp", props);
            stencilRef = FindProperty("_StencilRef", props);
            stencilPass = FindProperty("_StencilPass", props);

            zWriteParam = FindProperty("_ZWriteParam", props);
            zTestMode = FindProperty("_ZTestMode", props);
        }

        //GUIFunction
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            FindProperties(props); //Properties update
            m_MaterialEditor = materialEditor;
            Material material = materialEditor.target as Material;

            ShaderPropertiesGUI(material);//GUI update

            //Show default GUI(Debug)
            //EditorGUILayout.Space();
            //base.OnGUI(materialEditor, props);
        }

        //GUI update
        public void ShaderPropertiesGUI(Material material)
        {
            EditorGUIUtility.labelWidth = 0f;

            int indentation = 0;
            m_MaterialEditor.RenderQueueField();
            m_MaterialEditor.EnableInstancingField();

            indentation = 1;
            if (imageType.floatValue == 0)
            {
                //Mask
                EditorGUI.BeginChangeCheck();
                m_MaterialEditor.ShaderProperty(imageType, Styles.imageTypeText);
                m_MaterialEditor.ShaderProperty(maskBlendMode, Styles.maskBlendModeText, indentation);
                m_MaterialEditor.ShaderProperty(showMaskImage, Styles.showMaskImageText, indentation);
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateAlphaBlendMode(material);
                    if (maskBlendMode.floatValue == (int)MaskBlend.Customize)
                    {
                        _isBlendOpen = true;
                    }
                }
            }
            else
            {
                //Image
                EditorGUI.BeginChangeCheck();
                m_MaterialEditor.ShaderProperty(imageType, Styles.imageTypeText);
                m_MaterialEditor.ShaderProperty(imageBlendMode, Styles.imageBlendModeText, indentation);
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateColorBlendMode(material);
                    if (imageBlendMode.floatValue == (int)ImageBlend.Customize)
                    {
                        _isBlendOpen = true;
                    }
                }
            }
            m_MaterialEditor.ShaderProperty(multiplyAlpha, Styles.multiplyAlphaText, indentation);

            //BlendMode
            bool isBlendFoldoutOpne = EditorGUILayout.Foldout(_isBlendOpen, "BlendMode");
            if (_isBlendOpen != isBlendFoldoutOpne)
            {
                _isBlendOpen = isBlendFoldoutOpne;
            }
            if (isBlendFoldoutOpne)
            {
                EditorGUI.BeginChangeCheck();
                m_MaterialEditor.ShaderProperty(blendSrc, Styles.blendSrcText, indentation);
                m_MaterialEditor.ShaderProperty(blendDst, Styles.blendDstText, indentation);
                m_MaterialEditor.ShaderProperty(blendAlphaSrc, Styles.blendAlphaSrcText, indentation);
                m_MaterialEditor.ShaderProperty(blendAlphaDst, Styles.blendAlphaDstText, indentation);
                m_MaterialEditor.ShaderProperty(blendColorOp, Styles.blendColorOpText, indentation);
                m_MaterialEditor.ShaderProperty(blendAlphaOp, Styles.blendAlphaOpText, indentation);
                if (EditorGUI.EndChangeCheck())
                {
                    material.SetFloat("_ImageBlendMode", (float)ImageBlend.Customize);
                    material.SetFloat("_MaskBlendMode", (float)MaskBlend.Customize);
                }
            }

            indentation = 0;

            m_MaterialEditor.ShaderProperty(useAlphaMaskMap, Styles.useAlphaMaskMapText, indentation);
            if (useAlphaMaskMap.floatValue != 0)
            {
                m_MaterialEditor.TexturePropertySingleLine(Styles.alphaMaskMapText, alphaMaskMap);
                m_MaterialEditor.TextureScaleOffsetProperty(alphaMaskMap);
            }

            //MaskSetting
            m_MaterialEditor.ShaderProperty(useMaskChannel, Styles.useMaskChannelText, indentation);
            m_MaterialEditor.ShaderProperty(alphaMaskTransiton, Styles.alphaMaskTransitonText, indentation);
            m_MaterialEditor.ShaderProperty(alphaMaskEdge, Styles.alphaMaskEdgeText, indentation);//エッジ
            m_MaterialEditor.ShaderProperty(alphaMaskInverse, Styles.alphaMaskInverseText, indentation);//Alpha反転

            m_MaterialEditor.ShaderProperty(imageIntensity, Styles.imageIntensityText, indentation);//Alpha反転
            m_MaterialEditor.ShaderProperty(mainSpeedU, Styles.mainSpeedUText, indentation);
            m_MaterialEditor.ShaderProperty(mainSpeedV, Styles.mainSpeedVText, indentation);
            m_MaterialEditor.ShaderProperty(scale, Styles.scaleText, indentation);
            m_MaterialEditor.ShaderProperty(pixelSnap, Styles.pixelSnapText, indentation);

            //AlphaClip
            m_MaterialEditor.ShaderProperty(useUIAlphaClip, Styles.useUIAlphaClipText, indentation);
            m_MaterialEditor.ShaderProperty(cutoff, Styles.cutoffText, indentation);
            m_MaterialEditor.ShaderProperty(clipInverse, Styles.clipInverseText, indentation);
            m_MaterialEditor.ShaderProperty(alphaToMask, Styles.alphaToMaskText, indentation);

            //CullOff
            m_MaterialEditor.ShaderProperty(cullMode, Styles.cullModeText, indentation);

            //ColorMask
            m_MaterialEditor.ShaderProperty(colorMask, Styles.colorMaskText, indentation);

            //Stencil
            m_MaterialEditor.ShaderProperty(stencilComp, Styles.stencilCompText, indentation);
            m_MaterialEditor.ShaderProperty(stencilRef, Styles.stencilRefText, indentation);
            m_MaterialEditor.ShaderProperty(stencilPass, Styles.stencilPassText, indentation);
            
            //depth
            m_MaterialEditor.ShaderProperty(zWriteParam, Styles.zWriteParamText, indentation);
            m_MaterialEditor.ShaderProperty(zTestMode, Styles.zTestModeText, indentation);

        }

        //ImagePreset
        private void UpdateColorBlendMode(Material material)
        {
            switch (imageBlendMode.floatValue)
            {
                case (float)ImageBlend.Normal:
                    SetColorBlend(material,
                    BlendMode.SrcAlpha,
                    BlendMode.OneMinusSrcAlpha,
                    BlendOp.Add);
                    break;
                case (float)ImageBlend.Replace:
                    SetColorBlend(material,
                        BlendMode.One,
                        BlendMode.Zero,
                        BlendOp.Add);
                    break;
                case (float)ImageBlend.Premultiplied:
                    SetColorBlend(material,
                    BlendMode.One,
                    BlendMode.OneMinusSrcAlpha,
                    BlendOp.Add);
                    break;
                case (float)ImageBlend.Add1:
                    SetColorBlend(material,
                        BlendMode.One,
                        BlendMode.One,
                        BlendOp.Add);
                    break;
                case (float)ImageBlend.Add2:
                    SetColorBlend(material,
                        BlendMode.SrcAlpha,
                        BlendMode.One,
                        BlendOp.Add);
                    break;
                case (float)ImageBlend.Sub:
                    SetColorBlend(material,
                        BlendMode.SrcAlpha,
                        BlendMode.One,
                        BlendOp.ReverseSubtract);
                    break;
                case (float)ImageBlend.Multiply:
                    SetColorBlend(material,
                        BlendMode.Zero,
                        BlendMode.SrcColor,
                        BlendOp.Add);
                    break;
                case (float)ImageBlend.Min:
                    SetColorBlend(material,
                        BlendMode.One,
                        BlendMode.One,
                        BlendOp.Min);
                    break;
                case (float)ImageBlend.Max:
                    SetColorBlend(material,
                        BlendMode.One,
                        BlendMode.One,
                        BlendOp.Max);
                    break;
                case (float)ImageBlend.SoftAdd1:
                    SetColorBlend(material,
                        BlendMode.OneMinusDstColor,
                        BlendMode.One,
                        BlendOp.Add);
                    break;
                case (float)ImageBlend.SoftAdd2:
                    SetColorBlend(material,
                        BlendMode.DstColor,
                        BlendMode.One,
                        BlendOp.Add);
                    break;
                case (float)ImageBlend.MaskNormal:
                    SetColorBlend(material,
                        BlendMode.DstAlpha,
                        BlendMode.OneMinusDstAlpha,
                        BlendOp.Add);
                    break;
                case (float)ImageBlend.MaskReverse1:
                    SetColorBlend(material,
                        BlendMode.OneMinusDstAlpha,
                        BlendMode.DstAlpha,
                        BlendOp.Add);
                    break;
                case (float)ImageBlend.MaskReverse2:
                    SetColorBlend(material,
                        BlendMode.SrcAlphaSaturate,
                        BlendMode.DstAlpha,
                        BlendOp.Add);
                    break;
                case (float)ImageBlend.MaskReverseEx1:
                    SetColorBlend(material,
                        BlendMode.SrcAlphaSaturate,
                        BlendMode.DstAlpha,
                        BlendOp.Add);
                    SetAlphaBlend(material,
                        BlendMode.Zero,
                        BlendMode.DstAlpha,
                        BlendOp.Add);
                    return;
                case (float)ImageBlend.MaskReverseEx2:
                    SetColorBlend(material,
                        BlendMode.SrcAlphaSaturate,
                        BlendMode.DstAlpha,
                        BlendOp.Add);
                    SetAlphaBlend(material,
                        BlendMode.OneMinusDstAlpha,
                        BlendMode.DstAlpha,
                        BlendOp.Add);
                    return;
                case (float)ImageBlend.MaskAdd:
                    SetColorBlend(material,
                        BlendMode.DstAlpha,
                        BlendMode.One,
                        BlendOp.Add);
                    break;
                case (float)ImageBlend.MaskReverseAdd1:
                    SetColorBlend(material,
                        BlendMode.One,
                        BlendMode.DstAlpha,
                        BlendOp.Add);
                    break;
                case (float)ImageBlend.MaskReverseAdd2:
                    SetColorBlend(material,
                        BlendMode.SrcAlphaSaturate,
                        BlendMode.One,
                        BlendOp.Add);
                    break;
                case (float)ImageBlend.MaskSub:
                    SetColorBlend(material,
                        BlendMode.DstAlpha,
                        BlendMode.One,
                        BlendOp.ReverseSubtract);
                    break;
                case (float)ImageBlend.MaskMul:
                    SetColorBlend(material,
                        BlendMode.DstAlpha,
                        BlendMode.DstAlpha,
                        BlendOp.Add);
                    break;
                case (float)ImageBlend.MaskReverseMul:
                    SetColorBlend(material,
                        BlendMode.OneMinusDstAlpha,
                        BlendMode.OneMinusDstAlpha,
                        BlendOp.Add);
                    break;
            }

            SetAlphaBlend(material,
                BlendMode.Zero,
                BlendMode.One,
                BlendOp.Add);
        }

        //MaskPreset
        private void UpdateAlphaBlendMode(Material material)
        {
            switch (maskBlendMode.floatValue)
            {
                case (float)MaskBlend.Normal:
                    SetAlphaBlend(material,
                    BlendMode.One,
                    BlendMode.OneMinusSrcAlpha,
                    BlendOp.Add);
                    break;
                case (float)MaskBlend.Replace:
                    SetAlphaBlend(material,
                    BlendMode.One,
                    BlendMode.Zero,
                    BlendOp.Add);
                    break;
                case (float)MaskBlend.Add:
                    SetAlphaBlend(material,
                    BlendMode.One,
                    BlendMode.One,
                    BlendOp.Add);
                    break;
                case (float)MaskBlend.AddEx:
                    SetAlphaBlend(material,
                    BlendMode.DstAlpha,
                    BlendMode.SrcAlphaSaturate,
                    BlendOp.Add);
                    break;
                case (float)MaskBlend.Sub:
                    SetAlphaBlend(material,
                    BlendMode.One,
                    BlendMode.One,
                    BlendOp.ReverseSubtract);
                    break;
                case (float)MaskBlend.Sub2:
                    SetAlphaBlend(material,
                    BlendMode.SrcAlpha,
                    BlendMode.OneMinusDstAlpha,
                    BlendOp.Subtract);
                    break;
                case (float)MaskBlend.Multiply:
                    SetAlphaBlend(material,
                    BlendMode.Zero,
                    BlendMode.SrcAlpha,
                    BlendOp.Add);
                    break;
                case (float)MaskBlend.MultiplyEx:
                    SetAlphaBlend(material,
                    BlendMode.SrcAlphaSaturate,
                    BlendMode.SrcAlpha,
                    BlendOp.Add);
                    break;
                case (float)MaskBlend.Min:
                    SetAlphaBlend(material,
                    BlendMode.Zero,
                    BlendMode.SrcAlpha,
                    BlendOp.Min);
                    break;
                case (float)MaskBlend.Max:
                    SetAlphaBlend(material,
                    BlendMode.Zero,
                    BlendMode.SrcAlpha,
                    BlendOp.Max);
                    break;
                case (float)MaskBlend.DstNormal:
                    SetAlphaBlend(material,
                    BlendMode.OneMinusDstAlpha,
                    BlendMode.One,
                    BlendOp.Add);
                    break;
            }

            if (showMaskImage.floatValue == 1)
            {
                SetColorBlend(material,
                    BlendMode.SrcAlpha,
                    BlendMode.OneMinusSrcAlpha,
                    BlendOp.Add);
            }
            else
            {
                SetColorBlend(material,
                    BlendMode.Zero,
                    BlendMode.One,
                    BlendOp.Add);
            }
        }


        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            base.AssignNewShaderToMaterial(material, oldShader, newShader);
        }

        static void SetKeyword(Material m, string keyword, bool state)
        {
            if (state)
                m.EnableKeyword(keyword);
            else
                m.DisableKeyword(keyword);
        }

        public void SetColorBlend(
            Material m,
            BlendMode Src,
            BlendMode Dst,
            BlendOp ColorOp
            )
        {
            m.SetFloat("_BlendSrc", (float)Src);
            m.SetFloat("_BlendDst", (float)Dst);
            m.SetFloat("_BlendColorOp", (float)ColorOp);
        }

        public void SetAlphaBlend(
            Material m,
            BlendMode AlphaSrc,
            BlendMode AlphaDst,
            BlendOp AlphaOp
            )
        {
            m.SetFloat("_BlendAlphaSrc", (float)AlphaSrc);
            m.SetFloat("_BlendAlphaDst", (float)AlphaDst);
            m.SetFloat("_BlendAlphaOp", (float)AlphaOp);
        }
    }
}