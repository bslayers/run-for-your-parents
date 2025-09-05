using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(CellData), menuName = "Resources/" + nameof(CellData) + "/" + nameof(CellData))]
public class CellData : ScriptableObject
{

    public GameObject prefab;

    [Range(-180, 180)]
    public int rotation = 0;

    public CellConnection connection;


#if UNITY_EDITOR

    private void OnValidate()
    {
        rotation = Mathf.RoundToInt(rotation / 90f) * 90;
    }

#endif
}

[Serializable]
public class ConstraintCell
{
    public Legality legality;
    public List<CellData> list = new();
    public MapAttribute attribute;
}

public enum Legality : byte
{
    Unknown,
    Legal,
    Illegal,
}

[Serializable]
public class CellConnection
{
    public CellTypeMask upLeft, up, upRight;
    public CellTypeMask left, center, right;
    public CellTypeMask downLeft, down, downRight;

    public Dictionary<EDirection, CellTypeMask> ConnectionsMap
    {
        get => new()
        {
            [EDirection.UpLeft] = upLeft,
            [EDirection.Up] = up,
            [EDirection.UpRight] = upRight,
            [EDirection.Left] = left,
            [EDirection.Center] = center,
            [EDirection.Right] = right,
            [EDirection.DownLeft] = downLeft,
            [EDirection.Down] = down,
            [EDirection.DownRight] = downRight,
        };
    }

    public double MatchScore(CellConnection connection)
    {
        var patternMap = ConnectionsMap;
        var requesterMap = connection.ConnectionsMap;

        double finalScore = 0;

        foreach (EDirection dir in patternMap.Keys)
        {
            double score = ScoreDirection(patternMap[dir], requesterMap[dir]);
            if (score == 0) { return 0; }
            finalScore += score;
        }

        return finalScore;
    }

    private int ScoreDirection(CellTypeMask patternMask, CellTypeMask actual)
    {
        if (actual == CellTypeMask.Void) { return patternMask == CellTypeMask.Any ? 2 : 1; }

        if (patternMask == CellTypeMask.Any) { return 1; }

        if (patternMask == actual) { return 3; }

        if ((patternMask & actual) != 0)
        {
            int commonFlags = CountSetBits(patternMask & actual);
            int totalPatternFlags = CountSetBits(patternMask);
            return (int)((double)commonFlags / totalPatternFlags * 2);
        }

        return 0;
    }

    private int CountSetBits(CellTypeMask mask)
    {
        int count = 0;
        int value = (int)mask;
        while (value != 0)
        {
            count += value & 1;
            value >>= 1;
        }
        return count;
    }

    public override string ToString()
    {
        return $"|{upLeft}|{up}|{upRight}|\n{left}|{center}|{right}|\n|{downLeft}|{down}|{downRight}|";
    }

}

[Flags]
public enum CellTypeMask
{
    Void = 0,
    Road = 1 << 1,
    Walkway = 1 << 2,
    Structure = 1 << 3,

    Any = ~0,
}
