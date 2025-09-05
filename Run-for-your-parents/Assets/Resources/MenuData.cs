using UnityEngine;

[CreateAssetMenu(fileName = "MenuData", menuName = "Resources/MenuData")]
public class MenuData : ScriptableObject
{
    public enum MenuType{Main=0,Pause=1, GameOver=2, Victory=3, Chat=4, Expedition=5, SignWheel=6}

    public string menuName = "";

    [Tooltip("Type of the menu")]
    public MenuType type;

    [Tooltip("Color of the button in the Navigable")]
    public NavigableColorData colorDataOfNavigables;
}
