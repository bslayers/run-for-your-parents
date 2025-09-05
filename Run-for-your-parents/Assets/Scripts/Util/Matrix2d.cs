using UnityEngine;

public class Matrix2D<T>
{
    #region Variables
    private T[,] data;

    private int length;
    private int width;

    public int Length { get => length; }
    public int Width { get => width; }


    #endregion

    #region Accessors

    public T this[int x, int y]
    {
        get => data[x, y];
        set => data[x, y] = value;
    }

    public T this[Vector2Int pos]
    {
        get { return data[pos.x, pos.y]; }
        set => data[pos.x, pos.y] = value;
    }


    #endregion

    #region Constructor

    public Matrix2D(int length, int width)
    {
        InitMatrix(length, width);
    }

    public Matrix2D(int length, int width, T initValue)
    {
        InitMatrix(length, width);
        InitValues(initValue);
    }

    /// <summary>
    /// Initialize 2D Matrix with size
    /// </summary>
    /// <param name="size">a Vector2Int representing length and width</param>
    public Matrix2D(Vector2Int size)
    {
        InitMatrix(size.x, size.y);
    }

    public Matrix2D(Vector2Int size, T initValue)
    {
        InitMatrix(size.x, size.y);
        InitValues(initValue);
    }

    private void InitMatrix(int length, int width)
    {
        data = new T[length, width];
        this.length = length;
        this.width = width;
    }

    private void InitValues(T with)
    {
        for (int x = 0; x < Length; ++x)
        {
            for (int y = 0; y < Width; ++y)
            {
                data[x, y] = with;
            }
        }
    }

    #endregion

    #region Methods

    public bool CanAccess(Vector2Int pos)
    {
        return CanAccess(pos.x, pos.y);
    }

    public bool CanAccess(int x, int y)
    {
        return x >= 0 && x < Length && y >= 0 && y < Width;
    }

    public override string ToString()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int y = 0; y < Width; ++y)
        {
            for (int x = 0; x < Length; ++x)
            {
                sb.Append($"[{x},{y}]={data[x, y]}|");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }


    #endregion
}