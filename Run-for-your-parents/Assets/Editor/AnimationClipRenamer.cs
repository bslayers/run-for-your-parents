using UnityEditor;
using UnityEngine;

public class AnimationClipRenamer : AssetPostprocessor
{
    private const string PrefixToRemove = "Armature|";

    void OnPostprocessModel(GameObject g)
    {
        ModelImporter importer = assetImporter as ModelImporter;
        if (importer == null) return;

        
        ModelImporterClipAnimation[] clips = importer.clipAnimations;

        bool changed = false;

        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i].name.StartsWith(PrefixToRemove))
            {
                string newName = clips[i].name.Substring(PrefixToRemove.Length);
                clips[i].name = newName;//;$"{g.name}|{newName}";
                changed = true;
            }
        }

        if (changed)
        {
            importer.clipAnimations = clips;
        }
    }
}
