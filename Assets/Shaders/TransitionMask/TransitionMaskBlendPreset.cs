namespace TransitionMaskBlendPreset
{
    public enum ImageBlend
    {
        Normal = 0,
        Replace = 10,
        Premultiplied = 20,
        Add1 = 30,
        Add2 = 31,
        Sub = 40,
        Multiply = 50,
        Min = 60,
        Max = 70,
        SoftAdd1 = 80,
        SoftAdd2 = 81,
        MaskNormal = 500,
        MaskReverse1 = 510,
        MaskReverse2 = 511,
        MaskReverseEx1 = 520,
        MaskReverseEx2 = 521,
        MaskAdd = 530,
        MaskReverseAdd1 = 540,
        MaskReverseAdd2 = 541,
        MaskSub = 550,
        MaskMul = 560,
        MaskReverseMul = 570,
        Customize = 999,
    }
    public enum MaskBlend
    {
        Normal = 0,
        Replace = 10,
        Add = 30,
        AddEx = 31,
        Sub = 40,
        Sub2 = 41,
        Multiply = 50,
        MultiplyEx = 51,
        Min = 60,
        Max = 70,
        DstNormal = 80,
        Customize = 999,
    }
}
