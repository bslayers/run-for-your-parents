using System;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(StreetlampLightTypeWeightListData), menuName = "Resources/" + nameof(ObjectWeight<bool>) + "/" + nameof(ObjectWeightList<bool>) + "/" + nameof(StreetlampLightTypeWeightListData))]
public class StreetlampLightTypeWeightListData : EnumWeightList<StreetLampManager.LightType>
{

}