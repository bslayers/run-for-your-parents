using System;

[Serializable]
public abstract class MinMax<E>
{
    #region Variables

    public E min;
    public E max;

    #endregion

    #region Accessors



    #endregion

    #region Constructor

    public MinMax() { }

    public MinMax(E min, E max)
    {
        this.min = min;
        this.max = max;
    }

    #endregion

    #region Methods

    public bool AtLimit(E value)
    {
        return min.Equals(value) || max.Equals(value);
    }

    public abstract bool In(E value);

    #endregion
}

[Serializable]
public class MinMaxFloat : MinMax<float>
{
    public MinMaxFloat(float min, float max) : base(min, max) { }

    public override bool In(float value)
    {
        return min < value && value < max;
    }
}
[Serializable]
public class MinMaxDouble : MinMax<double>
{
    public MinMaxDouble(double min, double max) : base(min, max) { }

    public override bool In(double value)
    {
        return min < value && value < max;
    }
}
[Serializable]
public class MinMaxInt : MinMax<int>
{
    public MinMaxInt(int min, int max) : base(min, max) { }

    public override bool In(int value)
    {
        return min < value && value < max;
    }
}
