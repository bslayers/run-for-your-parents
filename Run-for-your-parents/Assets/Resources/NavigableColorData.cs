using UnityEngine;

[CreateAssetMenu(fileName = "MenuColorData", menuName = "Resources/MenuColorData")]
public class NavigableColorData : ScriptableObject
{
    [Header("Colors for butons")]
    public Color normalColor = Color.white;
    public Color normalFontColor = Color.black;
    public Color highlightedColor = Color.white;
    public Color highlightedFontColor = Color.black;
    public Color SelectedColor = Color.white;
    public Color SelectedFontColor = Color.black;
    public Color pressedColor = Color.white;
    public Color pressedFontColor = Color.black;
}
