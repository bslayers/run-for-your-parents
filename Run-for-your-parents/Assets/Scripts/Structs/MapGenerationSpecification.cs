using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapGenerationSpecification
{
    public const int MAX_WEIGHT = 10;

    [Range(2, 10)] public int minStepsAllowed;
    [Range(2, 10)] public int mindistanceBetweenIntersection;

    [Header("Specification for Structure generation")]
    [Range(1, MAX_WEIGHT)] public int minNbStructure;
    [Range(2, MAX_WEIGHT)] public int maxNbStructure;

    [Header("Structures weight")]
    [Range(0, MAX_WEIGHT)] public int size1Weight;
    [Range(0, MAX_WEIGHT)] public int size2Weight;
    [Range(0, MAX_WEIGHT)] public int size3Weight;
    [Range(0, MAX_WEIGHT)] public int size4Weight;

    [Header("Road types weight")]
    [Range(0, MAX_WEIGHT)] public int wideRoadWeight;
    [Range(0, MAX_WEIGHT)] public int singleRoadWeight;

    [Header("road intersection at direction weight")]
    [Range(0, MAX_WEIGHT)] public int straightIntersectionWeight;
    [Range(0, MAX_WEIGHT)] public int rightIntersectionWeight;
    [Range(0, MAX_WEIGHT)] public int leftIntersectionWeight;

    [Header("Walkways weight")]
    [Range(0, MAX_WEIGHT)] public int walkwayWeight;



    public void GetStructureSizeWeightMap(ref Dictionary<Surface.Scale, int> map)
    {
        map.Clear();
        map.Add(Surface.Scale.S1, size1Weight);
        map.Add(Surface.Scale.S2, size2Weight);
        map.Add(Surface.Scale.S3, size3Weight);
        map.Add(Surface.Scale.S4, size4Weight);
    }

    public void GetIntersectionWeightMap(ref Dictionary<MapAttribute.IntersectionType, int> map)
    {
        map.Clear();
        map.Add(MapAttribute.IntersectionType.StraightIntersection, straightIntersectionWeight);
        map.Add(MapAttribute.IntersectionType.LeftIntersection, leftIntersectionWeight);
        map.Add(MapAttribute.IntersectionType.RightIntersection, rightIntersectionWeight);
    }

    public void GetRoadTypeWeightMap(ref Dictionary<MapAttribute.RoadType, int> map)
    {
        map.Clear();
        map.Add(MapAttribute.RoadType.WideRoad, wideRoadWeight);
        map.Add(MapAttribute.RoadType.SingleRoad, singleRoadWeight);
    }

}

public class MapAttribute
{
    public enum RoadType { WideRoad, SingleRoad, }
    public enum IntersectionType { StraightIntersection, LeftIntersection, RightIntersection, }
    public enum WalkwayType { WalkwayInSingleRoad, WalkwayInWideRoad, }

    //public enum DecorType { NoDecor, Detector, }
};