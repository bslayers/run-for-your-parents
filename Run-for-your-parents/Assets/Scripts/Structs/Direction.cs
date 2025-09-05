using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Direction
{
    public static readonly Direction UP = new Direction { direction = EDirection.Up, vector = Vector2Int.up };
    public static readonly Direction UP_RIGHT = new Direction { direction = EDirection.UpRight, vector = Vector2Int.up + Vector2Int.right };
    public static readonly Direction RIGHT = new Direction { direction = EDirection.Right, vector = Vector2Int.right };
    public static readonly Direction DOWN_RIGHT = new Direction { direction = EDirection.DownRight, vector = Vector2Int.down + Vector2Int.right };
    public static readonly Direction DOWN = new Direction { direction = EDirection.Down, vector = Vector2Int.down };
    public static readonly Direction DOWN_LEFT = new Direction { direction = EDirection.DownLeft, vector = Vector2Int.down + Vector2Int.left };
    public static readonly Direction LEFT = new Direction { direction = EDirection.Left, vector = Vector2Int.left };
    public static readonly Direction UP_LEFT = new Direction { direction = EDirection.UpLeft, vector = Vector2Int.up + Vector2Int.left };


    public static readonly Direction[] directions4List =
    {
        UP,
        RIGHT,
        DOWN,
        LEFT,
    };

    public static readonly Direction[] directions8List =
    {
        UP,
        UP_RIGHT,
        RIGHT,
        DOWN_RIGHT,
        DOWN,
        DOWN_LEFT,
        LEFT,
        UP_LEFT,
    };

    public static readonly Dictionary<EDirection, Direction> directionsMap = new Dictionary<EDirection, Direction>()
    {
        [EDirection.Up] = UP,
        [EDirection.UpRight] = UP_RIGHT,
        [EDirection.Right] = RIGHT,
        [EDirection.DownRight] = DOWN_RIGHT,
        [EDirection.Down] = DOWN,
        [EDirection.DownLeft] = DOWN_LEFT,
        [EDirection.Left] = LEFT,
        [EDirection.UpLeft] = UP_LEFT,
    };


    public EDirection direction;
    public Vector2Int vector;

    public EDirection OppositeDirection
    {
        get
        {
            switch (direction)
            {
                case EDirection.Up:
                    return EDirection.Down;
                case EDirection.UpRight:
                    return EDirection.DownLeft;
                case EDirection.Right:
                    return EDirection.Left;
                case EDirection.DownRight:
                    return EDirection.UpLeft;
                case EDirection.Down:
                    return EDirection.Up;
                case EDirection.DownLeft:
                    return EDirection.UpRight;
                case EDirection.Left:
                    return EDirection.Right;
                case EDirection.UpLeft:
                    return EDirection.DownRight;
                default:
                    return direction;
            }
        }
    }

    public EDirection RotateDirection90(bool left)
    {
        switch (direction)
        {
            case EDirection.Up:
                return left ? EDirection.Left : EDirection.Right;
            case EDirection.UpRight:
                return left ? EDirection.UpLeft : EDirection.DownRight;
            case EDirection.Right:
                return left ? EDirection.Up : EDirection.Down;
            case EDirection.DownRight:
                return left ? EDirection.UpRight : EDirection.DownLeft;
            case EDirection.Down:
                return left ? EDirection.Right : EDirection.Left;
            case EDirection.DownLeft:
                return left ? EDirection.DownRight : EDirection.UpLeft;
            case EDirection.Left:
                return left ? EDirection.Down : EDirection.Up;
            case EDirection.UpLeft:
                return left ? EDirection.DownLeft : EDirection.UpRight;
            default:
                return direction;
        }
    }

    public EDirection RotateDirection45(bool left)
    {
        switch (direction)
        {
            case EDirection.Up:
                return left ? EDirection.UpLeft : EDirection.UpRight;
            case EDirection.UpRight:
                return left ? EDirection.Up : EDirection.Right;
            case EDirection.Right:
                return left ? EDirection.UpRight : EDirection.DownLeft;
            case EDirection.DownRight:
                return left ? EDirection.Right : EDirection.Down;
            case EDirection.Down:
                return left ? EDirection.DownRight : EDirection.DownLeft;
            case EDirection.DownLeft:
                return left ? EDirection.Down : EDirection.Left;
            case EDirection.Left:
                return left ? EDirection.DownLeft : EDirection.UpLeft;
            case EDirection.UpLeft:
                return left ? EDirection.Left : EDirection.Up;
            default:
                return direction;
        }
    }

}

public enum EDirection { Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft, Center }

[Flags]
public enum DirectionMask
{
    None = 0,
    Center = 1,
    Up = 1 << 1,
    UpRight = 1 << 2,
    Right = 1 << 3,
    DownRight = 1 << 4,
    Down = 1 << 5,
    DownLeft = 1 << 6,
    Left = 1 << 7,
    UpLeft = 1 << 8,
}