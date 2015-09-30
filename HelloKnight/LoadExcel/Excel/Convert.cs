using System;

static class CConvert
{
    static public Int32 ToInt32(string p_str)
    {
        int iValue = 0;
        if (0 < p_str.Length)
        {
            iValue = Convert.ToInt32(p_str);
        }
        return iValue;
    }

    static public float ToFloat(string p_str)
    {
        float fValue = 0.0f;
        if (0 < p_str.Length)
        {
            fValue = Convert.ToSingle(p_str);
        }
        return fValue;
    }
}