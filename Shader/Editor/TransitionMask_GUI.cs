using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;
using TransitionMaskShader;

namespace UnityEditor
{
    internal class TransitionMask_GUI : ShaderGUI
    {
        //GUIContent
        private static class Styles
        {
            public static GUIContent imageTypeText = new GUIContent("Image Type", "Image Type");
            public static GUIContent useMaskChannelText = new GUIContent("Use Mask Channel", "Use Mask Channel");
            public static GUIContent showMaskImageText = new GUIContent("Show Mask Image", "Show Mask Image");

            public static GUIContent maskBlendModeText = new GUIContent("BlendMode", "BlendMode");
            public static GUIContent alphaMaskEdgeText = new GUIContent("Mask Edge", "Mask Edge");
            public static GUIContent alphaMaskTransitonText = new GUIContent("Transition", "Transition");
            public static GUIContent alphaMaskInverseText = new GUIContent("Alpha Inverse", "Alpha Inverse");

            public static GUIContent imageBlendModeText = new GUIContent("BlendMode", "BlendMode");
            public static GUIContent imageIntensityText = new GUIContent("Image Intensity", "Image Intensity");

            public static GUIContent stencilCompText = new GUIContent("Comp", "Comp");
            public static GUIContent stencilRefText = new GUIContent("Ref", "Ref");
            public static GUIContent stencilPassText = new GUIContent("Pass", "Pass");

            public static GUIContent zWriteParamText = new GUIContent("ZWrite Param", "ZWrite Param");
            public static GUIContent zTestModeText = new GUIContent("ZTest Mode", "ZTest Mode");

            public static GUIContent blendSrcText = new GUIContent("SrcFactor", "SrcFactor");
            public static GUIContent blendDstText = new GUIContent("DstFactor", "DstFactor");
            public static GUIContent blendAlphaSrcText = new GUIContent("SrcFactorA", "SrcFactorA");
            public static GUIContent blendAlphaDstText = new GUIContent("DstFactorA", "DstFactorA");
            public static GUIContent blendColorOpText = new GUIContent("ColerOp", "ColerOp");
            public static GUIContent blendAlphaOpText = new GUIContent("AlphaOp", "AlphaOp");
        }

        //Properties
        MaterialProperty imageType;

        MaterialProperty maskBlendMode;
        MaterialProperty useMaskChannel;
        MaterialProperty showMaskImage;
        MaterialProperty alphaMaskEdge;
        MaterialProperty alphaMaskTransiton;
        MaterialProperty alphaMaskInverse;

        MaterialProperty imageBlendMode;
        MaterialProperty imageIntensity;

        MaterialProperty blendSrc;
        MaterialProperty blendDst;
        MaterialProperty blendAlphaSrc;
        MaterialProperty blendAlphaDst;
        MaterialProperty blendColorOp;
        MaterialProperty blendAlphaOp;

        MaterialProperty zWriteParam;
        MaterialProperty zTestMode;

        MaterialProperty stencilComp;
        MaterialProperty stencilRef;
        MaterialProperty stencilPass;

        MaterialEditor m_MaterialEditor;

        private bool _isBlendOpen = false;

        public void FindProperties(MaterialProperty[] props)
        {
            imageType = FindProperty("_ImageType", props);

            maskBlendMode = FindProperty("_MaskBlendMode", props);
            useMaskChannel = FindProperty("_useMaskChannel", props);
            showMaskImage = FindProperty("_ShowMaskImage", props);
            alphaMaskEdge = FindProperty("_AlphaMaskEdge", props);
            alphaMaskTransiton = FindProperty("_AlphaMaskTransition", props);
            alphaMaskInverse = FindProperty("_AlphaMaskInverse", props);

            imageBlendMode = FindProperty("_ImageBlendMode", props);
            imageIntensity = FindProperty("_ImageIntensity", props);

            stencilComp = FindProperty("_StencilComp", props);
            stencilRef = FindProperty("_StencilRef", props);
            stencilPass = FindProperty("_StencilPass", props);

            blendSrc = FindProperty("_BlendSrc", props);
            blendDst = FindProperty("_BlendDst", props);
            blendAlphaSrc = FindProperty("_BlendAlphaSrc", props);
            blendAlphaDst = FindProperty("_BlendAlphaDst", props);
            blendColorOp = FindProperty("_BlendColorOp", props);
            blendAlphaOp = FindProperty("_BlendAlphaOp", props);

            zWriteParam = FindProperty("_ZWriteParam", props);
            zTestMode = FindProperty("_ZTestMode", props);
        }

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            FindProperties(props);
            m_MaterialEditor = materialEditor;
            Material material = materialEditor.target as Material;

            ShaderPropertiesGUI(material);

            //Show default GUI for debug
            //EditorGUILayout.Space();
            //base.OnGUI(materialEditor, props);
        }

        public void ShaderPropertiesGUI(Material material)
        {
            EditorGUIUtility.labelWidth = 0f;

            int indentation = 0;
            m_MaterialEditor.RenderQueueField();
            m_MaterialEditor.EnableInstancingField();

            //Stencil
            m_MaterialEditor.ShaderProperty(stencilComp, Styles.stencilCompText, indentation);
            m_MaterialEditor.ShaderProperty(stencilRef, Styles.stencilRefText, indentation);
            m_MaterialEditor.ShaderProperty(stencilPass, Styles.stencilPassText, indentation);

            //Other
            m_MaterialEditor.ShaderProperty(zWriteParam, Styles.zWriteParamText, indentation);
            m_MaterialEditor.ShaderProperty(zTestMode, Styles.zTestModeText, indentation);
            
            EditorGUI.BeginChangeCheck();
            
            //ModeType
            m_MaterialEditor.ShaderProperty(imageType, Styles.imageTypeText);
            if (EditorGUI.EndChangeCheck())
            {
                UpdateBlendMode(material);
            }

            indentation = 1;
            if (imageType.floatValue == 0)
            {
                //Mask
                EditorGUI.BeginChangeCheck();
                m_MaterialEditor.ShaderProperty(maskBlendMode, Styles.maskBlendModeText, indentation);//mode選択
                m_MaterialEditor.ShaderProperty(showMaskImage, Styles.showMaskImageText, indentation);
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateBlendMode(material);
                    if (showMaskImage.floatValue == 1)
                    {
                        material.SetFloat("_BlendSrc", (float)BlendMode.SrcAlpha);
                        material.SetFloat("_BlendDst", (float)BlendMode.OneMinusSrcAlpha);
                    }
                    else
                    {
                        material.SetFloat("_BlendSrc", (float)BlendMode.Zero);
                        material.SetFloat("_BlendDst", (float)BlendMode.One);
                    }
                    if (maskBlendMode.floatValue == (int)MaskBlend.Customize)
                    {
                        _isBlendOpen = true;
                    }
                }
                m_MaterialEditor.ShaderProperty(useMaskChannel, Styles.useMaskChannelText, indentation);
                m_MaterialEditor.ShaderProperty(alphaMaskTransiton, Styles.alphaMaskTransitonText, indentation);
                m_MaterialEditor.ShaderProperty(alphaMaskEdge, Styles.alphaMaskEdgeText, indentation);//エッジ
                m_MaterialEditor.ShaderProperty(alphaMaskInverse, Styles.alphaMaskInverseText, indentation);//Alpha反転
            }
            else
            {
                //Image
                EditorGUI.BeginChangeCheck();
                m_MaterialEditor.ShaderProperty(imageBlendMode, Styles.imageBlendModeText, indentation);//mode選択
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateBlendMode(material);
                    if (imageBlendMode.floatValue == (int)ImageBlend.Customize)
                    {
                        _isBlendOpen = true;
                    }
                }

            }
            indentation = 0;
                m_MaterialEditor.ShaderProperty(imageIntensity, Styles.imageIntensityText, indentation);//Alpha反転

            //Blend
            bool isBlendFoldoutOpne = EditorGUILayout.Foldout(_isBlendOpen, "BlendMode");
            if (_isBlendOpen != isBlendFoldoutOpne)
            {
                _isBlendOpen = isBlendFoldoutOpne;
            }
            if (isBlendFoldoutOpne)
            {
                EditorGUI.BeginChangeCheck();

                //Blend
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
        }

        private void UpdateBlendMode(Material material)
        {
            if (imageType.floatValue == 0)
            {
                //Mask
                switch (maskBlendMode.floatValue)
                {
                    case (float)MaskBlend.Override:
                        SetBlend(material,
                        BlendMode.Zero,
                        BlendMode.One,
                        BlendMode.One,
                        BlendMode.Zero,
                        BlendOp.Add,
                        BlendOp.Add);
                        break;
                    case (float)MaskBlend.Normal:
                        SetBlend(material,
                        BlendMode.Zero,
                        BlendMode.One,
                        BlendMode.One,
                        BlendMode.OneMinusSrcAlpha,
                        BlendOp.Add,
                        BlendOp.Add);
                        break;
                    case (float)MaskBlend.Add:
                        SetBlend(material,
                        BlendMode.Zero,
                        BlendMode.One,
                        BlendMode.One,
                        BlendMode.One,
                        BlendOp.Add,
                        BlendOp.Add);
                        break;
                    case (float)MaskBlend.Sub:
                        SetBlend(material,
                        BlendMode.Zero,
                        BlendMode.One,
                        BlendMode.One,
                        BlendMode.One,
                        BlendOp.Add,
                        BlendOp.ReverseSubtract);
                        break;
                    case (float)MaskBlend.Multiply:
                        SetBlend(material,
                        BlendMode.Zero,
                        BlendMode.One,
                        BlendMode.Zero,
                        BlendMode.SrcAlpha,
                        BlendOp.Add,
                        BlendOp.Add);
                        break;
                    case (float)MaskBlend.Min:
                        SetBlend(material,
                        BlendMode.Zero,
                        BlendMode.One,
                        BlendMode.Zero,
                        BlendMode.SrcAlpha,
                        BlendOp.Add,
                        BlendOp.Min);
                        break;
                    case (float)MaskBlend.Max:
                        SetBlend(material,
                        BlendMode.Zero,
                        BlendMode.One,
                        BlendMode.Zero,
                        BlendMode.SrcAlpha,
                        BlendOp.Add,
                        BlendOp.Max);
                        break;
                }
            }
            else
            {
                //Image
                switch (imageBlendMode.floatValue)
                {
                    case (float)ImageBlend.Normal:
                        SetBlend(material,
                            BlendMode.DstAlpha,
                            BlendMode.OneMinusDstAlpha,
                            BlendMode.Zero,
                            BlendMode.One,
                            BlendOp.Add,
                            BlendOp.Add);
                        break;
                    case (float)ImageBlend.Reverse:
                        SetBlend(material,
                            BlendMode.OneMinusDstAlpha,
                            BlendMode.DstAlpha,
                            BlendMode.Zero,
                            BlendMode.One,
                            BlendOp.Add,
                            BlendOp.Add);
                        break;
                    case (float)ImageBlend.Min:
                        SetBlend(material,
                            BlendMode.DstAlpha,
                            BlendMode.One,
                            BlendMode.Zero,
                            BlendMode.One,
                            BlendOp.Min,
                            BlendOp.Add);
                        break;
                    case (float)ImageBlend.Max:
                        SetBlend(material,
                            BlendMode.DstAlpha,
                            BlendMode.One,
                            BlendMode.Zero,
                            BlendMode.One,
                            BlendOp.Max,
                            BlendOp.Add);
                        break;
                    case (float)ImageBlend.Add:
                        SetBlend(material,
                            BlendMode.DstAlpha,
                            BlendMode.One,
                            BlendMode.Zero,
                            BlendMode.One,
                            BlendOp.Add,
                            BlendOp.Add);
                        break;
                    case (float)ImageBlend.ReverseAdd:
                        SetBlend(material,
                            BlendMode.OneMinusDstAlpha,
                            BlendMode.One,
                            BlendMode.Zero,
                            BlendMode.One,
                            BlendOp.Add,
                            BlendOp.Add);
                        break;
                    case (float)ImageBlend.Sub:
                        SetBlend(material,
                            BlendMode.DstAlpha,
                            BlendMode.One,
                            BlendMode.Zero,
                            BlendMode.One,
                            BlendOp.ReverseSubtract,
                            BlendOp.Add);
                        break;
                    case (float)ImageBlend.ReverseMul:
                        SetBlend(material,
                            BlendMode.OneMinusDstAlpha,
                            BlendMode.OneMinusDstAlpha,
                            BlendMode.Zero,
                            BlendMode.One,
                            BlendOp.Add,
                            BlendOp.Add);
                        break;
                    case (float)ImageBlend.BlackOut:
                        SetBlend(material,
                            BlendMode.OneMinusDstAlpha,
                            BlendMode.OneMinusSrcAlpha,
                            BlendMode.Zero,
                            BlendMode.One,
                            BlendOp.Add,
                            BlendOp.Add);
                        break;
                    case (float)ImageBlend.AlphaDark:
                        SetBlend(material,
                            BlendMode.OneMinusSrcAlpha,
                            BlendMode.OneMinusDstAlpha,
                            BlendMode.Zero,
                            BlendMode.One,
                            BlendOp.Add,
                            BlendOp.Add);
                        break;

                }
            }
        }
        public enum BlendOp
        {
            Add = 0,
            Subtract = 1,
            ReverseSubtract = 2,
            Min = 3,
            Max = 4,
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

        public void SetBlend(
            Material m,
            BlendMode Src,
            BlendMode Dst,
            BlendMode AlphaSrc,
            BlendMode AlphaDst,
            BlendOp ColorOp,
            BlendOp AlphaOp
            )
        {
            m.SetFloat("_BlendSrc", (float)Src);
            m.SetFloat("_BlendDst", (float)Dst);
            m.SetFloat("_BlendAlphaSrc", (float)AlphaSrc);
            m.SetFloat("_BlendAlphaDst", (float)AlphaDst);
            m.SetFloat("_BlendColorOp", (float)ColorOp);
            m.SetFloat("_BlendAlphaOp", (float)AlphaOp);
        }
        
    }
}
