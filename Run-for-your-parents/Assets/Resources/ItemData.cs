using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Resources/ItemData")]
public class ItemData : ScriptableObject
{
    public enum USING_TYPE { ThrowingItem, UsingItem };
    public enum ITEM_TYPE { UNKNOWN, STONE, TINCAN, COMPASS, FLASHLIGHT, LANTERN };

    public GameObject prefab;


    [Header("Information for Inventory")]
    public string objectName = "";
    [Tooltip("Type of the item")]
    public ITEM_TYPE type = ITEM_TYPE.UNKNOWN;
    [Tooltip("Type of use of the item")]
    public USING_TYPE usingType = USING_TYPE.UsingItem;
    public bool objectRotationFollowingCamera = false;

    [Tooltip("Is the item stackable?")]
    public bool stackable = false;

    [Range(1, 20)]
    [Tooltip("Max stack of item in one slot")]
    public int maxStack = 1;

    public int MaxStack
    {
        get
        {
            if (stackable) return maxStack;
            else return 1;
        }
    }

    [Header("Information for GameObject")]

    [Range(0.001f, 5f)]
    [Tooltip("Kilograms")]
    public float mass = 1f;


    [Header("Sound Emission")]
    public SoundData contactSoundData;
    public SoundData staySoundData;


    [Header("Icones in inventory")]
    [Tooltip("Icon of the item")]
    public Sprite image;
    [Tooltip("Icon of the item when there is more than one item")]
    public Sprite imageForMultipleItem;

}
