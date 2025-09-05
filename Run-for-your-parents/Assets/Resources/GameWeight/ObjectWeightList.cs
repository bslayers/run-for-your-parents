using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWeightList<E> : ScriptableObject
{
    public List<ObjectWeight<E>> list = new();

    private int totalWeight;
    private int[] weights;

    public int TotalWeight { get => totalWeight; }
    public int[] Weights { get => weights; }

    public void CalculateWeights()
    {
        totalWeight = 0;
        weights = new int[list.Count];
        for (int i = 0; i < list.Count; ++i)
        {
            int weight = list[i].weight;
            totalWeight += weight;
            weights[i] = weight;
        }
    }
}

public class EnumWeightList<E> : ObjectWeightList<E> where E : Enum
{
    public EnumWeightList()
    {
        foreach (E val in Enum.GetValues(typeof(E)))
        { list.Add(new() { asset = val }); }
    }
}

[Serializable]
public class ObjectWeight<E>
{
    public E asset;

    [Range(1, 99)]
    public int weight = 1;


}