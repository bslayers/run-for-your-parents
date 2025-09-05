using System.Linq;
using System.Reflection;
using UnityEngine;

public class VariationItemChoice : MonoBehaviour, IGenerator
{

#region Variables

    [Tooltip("List of possible data to apply on the object (put the right type that corresponds to your object)")]
    public ScriptableObject[] variationData;

    #endregion

    #region Accessors


    #endregion


    #region Built-in
    public void Generate()
    {
        if (!TryGetComponent<Structure>(out var structure)) return;

        var fields = structure.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);


        foreach (var field in fields)
        {
            if (!field.Name.EndsWith("aterialData")) { continue; }

            var compatible = variationData.Where(data => data != null && field.FieldType.IsAssignableFrom(data.GetType())).ToArray();
            if (compatible.Length == 0) { continue; }
            
            var randomData = compatible[Random.Range(0, compatible.Length)];
            field.SetValue(structure, randomData);
        }
        structure.UpdateAllMaterial();
    }

    #endregion

    #region Methods

#endregion

#region Coroutine


#endregion

#region Events


#endregion

#region Editor


#endregion

}