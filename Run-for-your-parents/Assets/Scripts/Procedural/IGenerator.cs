
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IGenerator
{
    #region Built-in

    public void Generate();

    #endregion

    /// <summary>
    /// Choose randomly an index in a list of weight
    /// </summary>
    /// <param name="weightArray">a list of int representing the weight of each spawnable item</param>
    /// <returns>the index of the weight list</returns>
    public static int ChooseItem(int[] weightArray)
    {
        int totalWeight = weightArray.Sum();

        return ChooseItem(weightArray, totalWeight);
    }

    /// <summary>
    /// Choose randomly an index in a list of weight
    /// </summary>
    /// <param name="weightArray">a list of int representing the weight of each spawnable item</param>
    /// <param name="totalWeight">the sum of weight in <paramref name="weightArray"/></param>
    /// <returns>the index of the weight list</returns>
    public static int ChooseItem(int[] weightArray, int totalWeight)
    {
        int rdmInt = Random.Range(0, totalWeight);

        for (int i = 0; i < weightArray.Length; ++i)
        {
            int weight = weightArray[i];
            if (rdmInt < weight) { return i; }
            rdmInt -= weight;
        }
        return 0;
    }
}