using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameWeightData), menuName = "Resources/" + nameof(ObjectWeight<int>) + "/" + nameof(GameWeightData))]
public class GameWeightData : ScriptableObject
{
    [Header("Outside")]
    public DecorCellWeightListData decorCellWeightList;
    public StreetlampLightTypeWeightListData streetlampLightWeightList;

    [Header("Structures")]
    public BoolWeightListData boolPossibility;


    [Header("Detectors")]
    public BoolWeightListData isDetectorForDoor;
    public BoolWeightListData isDetectorForWeightDetector;

    //[Header("Entities")]
    //public 


}
