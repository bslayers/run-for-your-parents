
using System.Collections.Generic;

public static class Surface
{
    #region Variables
    public enum Scale { S025, S05, S1, S2, S3, S4 };
    public enum Orientation { Horizontal, Vertical };

    public static float SizeForS1 = 6f;

    public static readonly Dictionary<Scale, float> scaleMap = new Dictionary<Scale, float>(){
        {Scale.S025, 0.25f},
        {Scale.S05, 0.5f},
        {Scale.S1, 1},
        {Scale.S2, 2f},
        {Scale.S3, 3f},
        {Scale.S4, 4f}
    };

    public static float GetRealSize(Scale scale)
    {
        return scaleMap[scale] * SizeForS1;
    }

    #endregion

}