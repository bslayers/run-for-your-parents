

using System;
using UnityEngine;

public class Walker
{
    #region Variables

    private MapGenerator generator;
    public WalkerConstraint walkerConstraint;
    private Direction directionToLeft;
    private Direction directionToRight;
    public Cell currentCell;

    private bool canPlaceRoadAtRight;
    private bool canPlaceRoadAtLeft;
    private bool canPlaceWalkwayAtRight;
    private bool canPlaceWalkwayAtLeft;

    private int steps;



    #endregion

    #region Accessors



    #endregion

    #region Constructor

    public Walker(MapGenerator generator, WalkerConstraint constraint, Cell start)
    {
        this.generator = generator;
        walkerConstraint = constraint;
        currentCell = start;
        steps = 0;

        UpdateDirection();
        UpdateCanBooleans();
    }

    #endregion

    #region Built-in


    #endregion

    #region Methods

    /// <summary>
    /// Update variables canPlace* for generation
    /// </summary>
    private void UpdateCanBooleans()
    {
        canPlaceRoadAtLeft = false;
        canPlaceRoadAtRight = false;
        canPlaceWalkwayAtLeft = currentCell.neighbours[directionToLeft.direction] != null && walkerConstraint.createWalkwayAtLeft;
        canPlaceWalkwayAtRight = currentCell.neighbours[directionToRight.direction] != null && walkerConstraint.createWalkwayAtRight;

        if (walkerConstraint.roadType == MapAttribute.RoadType.WideRoad)
        {
            if (walkerConstraint.roadPartAtLeft)
            {
                canPlaceRoadAtLeft = canPlaceWalkwayAtLeft;
                canPlaceWalkwayAtLeft = currentCell.neighbours[directionToLeft.direction]?.neighbours[directionToLeft.direction] != null;
            }

            if (walkerConstraint.roadPartAtRight)
            {
                canPlaceRoadAtRight = canPlaceWalkwayAtRight;
                canPlaceWalkwayAtRight = currentCell.neighbours[directionToRight.direction]?.neighbours[directionToRight.direction] != null;
            }
        }
    }

    private void UpdateDirection()
    {
        switch (walkerConstraint.directionToMove.direction)
        {
            case EDirection.Down:
                directionToLeft = Direction.RIGHT;
                directionToRight = Direction.LEFT;
                break;
            case EDirection.Right:
                directionToLeft = Direction.UP;
                directionToRight = Direction.DOWN;
                break;
            case EDirection.Left:
                directionToLeft = Direction.DOWN;
                directionToRight = Direction.UP;
                break;
            case EDirection.Up:
                directionToLeft = Direction.LEFT;
                directionToRight = Direction.RIGHT;
                break;
            default:
                Debug.LogWarning($"Unknown direction '{walkerConstraint.directionToMove.direction}' in Walker");
                directionToLeft = Direction.LEFT;
                directionToRight = Direction.RIGHT;
                break;
        }
    }

    /// <summary>
    /// Perform next move of the walker
    /// </summary>
    /// <returns>
    /// true: the walker stopped <br/>
    /// false: the walker didn't stop
    /// </returns>
    public bool NextStep()
    {
        ++steps;

        Cell cell;
        Move(out cell);

        if (!CanDetermineCell(cell)) { return true; }

        currentCell = cell;

        if (!DetermineCurrentCells()) { return true; }

        return CheckFinished();

    }

    private void Move(out Cell cell)
    {
        cell = currentCell.neighbours[walkerConstraint.directionToMove.direction];
    }

    /// <summary>
    /// Check if the <paramref name="cell"/> is accurate cell for determination. 
    /// Stop the walker if the <paramref name="cell"/> can't be determined. 
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    private bool CanDetermineCell(Cell cell)
    {
        if (cell == null) { StopWalking(StoppingCause.OutOfMap); return false; }

        //TODO: maybe

        return true;
    }

    private bool DetermineCurrentCells()
    {
        if (IsRoadCell(currentCell)) { StopWalking(StoppingCause.EncounterRoad); return false; }

        PlaceRoad();

        PlaceWalkways();

        return true;
    }

    private bool IsRoadCell(Cell cell)
    {
        return (cell.info.mask & (CellTypeMask.Road)) != 0;
    }

    private void PlaceRoad()
    {
        currentCell.info.mask = CellTypeMask.Road;

        generator.CellTypeChanged(currentCell);

        if (walkerConstraint.roadType == MapAttribute.RoadType.SingleRoad) { return; }

        if (canPlaceRoadAtLeft)
        {
            Cell leftCell = currentCell.neighbours[directionToLeft.direction];
            leftCell.info.mask = CellTypeMask.Road;
            generator.CellTypeChanged(leftCell);
        }
        if (canPlaceRoadAtRight)
        {
            Cell rightCell = currentCell.neighbours[directionToRight.direction];
            rightCell.info.mask = CellTypeMask.Road;
            generator.CellTypeChanged(rightCell);
        }

    }

    private void PlaceWalkways()
    {
        if (canPlaceWalkwayAtLeft)
        {
            Cell cell = currentCell.neighbours[directionToLeft.direction];
            PlaceWalkway(canPlaceRoadAtLeft ? cell.neighbours[directionToLeft.direction] : cell);
        }

        if (canPlaceWalkwayAtRight)
        {
            Cell cell = currentCell.neighbours[directionToRight.direction];
            PlaceWalkway(canPlaceRoadAtRight ? cell.neighbours[directionToRight.direction] : cell);
        }
    }

    private void PlaceWalkway(Cell atCell)
    {
        if (IsRoadCell(atCell)) { return; }

        atCell.info.mask = CellTypeMask.Walkway;
        generator.CellTypeChanged(atCell);
    }

    private bool CheckFinished()
    {
        if (steps >= walkerConstraint.maxStepsAtLeft && steps >= walkerConstraint.maxStepsAtRight) { StopWalking(StoppingCause.NoMoreStep); return true; }
        if (steps >= walkerConstraint.maxStepsAtLeft) { StopWalking(StoppingCause.IntersectionAtLeft); return true; }
        if (steps >= walkerConstraint.maxStepsAtRight) { StopWalking(StoppingCause.IntersectionAtRight); return true; }
        return false;
    }


    private void StopWalking(StoppingCause cause)
    {
        generator.OnWalkerStoppedWalking(this, cause);
    }

    #endregion
}

[Serializable]
public struct WalkerConstraint
{
    public Direction directionToMove;

    public MapAttribute.RoadType roadType;
    public bool roadPartAtLeft;
    public bool roadPartAtRight;

    public bool createWalkwayAtRight;
    public bool createWalkwayAtLeft;
    public bool createWalkwayAtEndLeft;
    public bool createWalkwayAtEndRight;

    public int maxStepsAtRight;
    public int maxStepsAtLeft;
}

public enum StoppingCause
{
    NoMoreStep,
    OutOfMap,
    EncounterRoad,
    IntersectionAtLeft,
    IntersectionAtRight,
}