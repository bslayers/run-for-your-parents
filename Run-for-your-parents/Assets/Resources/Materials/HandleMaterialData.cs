using UnityEngine;

[CreateAssetMenu(fileName = "HandleMaterial", menuName = "Resources/MaterialData/HandleMaterial")]
public class HandleMaterialData : MaterialData
{
    public enum HandleType { Old, Modern, Metal };

    public Material material;
    public HandleType type;
    public Surface.Orientation orientation;

}
