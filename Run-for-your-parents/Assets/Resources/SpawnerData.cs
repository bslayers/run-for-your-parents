using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SpawnerData), menuName = "Resources/" + nameof(SpawnerData))]
public class SpawnerData : ScriptableObject
{
    public DecorCellWeightListData decorCells;


#if UNITY_EDITOR
    [ContextMenu("Recover all object")]
    protected void RecoverAll()
    {
        decorCells.list.Clear();
        decorCells.list.Add(new());

        string[] guids = AssetDatabase.FindAssets($"t:{nameof(DecorCellData)}");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            DecorCellData cell = AssetDatabase.LoadAssetAtPath<DecorCellData>(path);
            if (cell.decor == DecorType.StreetLamp) { continue; }

            decorCells.list.Add(new() { asset = cell });
        }
    }
#endif
}



