using UnityEngine;

[CreateAssetMenu(fileName = "SkyBox Properties", menuName = "Weather/SkyBoxProperties")]
public class SkyBoxProperties : ScriptableObject
{
    [Tooltip("The display name of the event")]
    public string propertiesName;

    [Tooltip("Hour of the day when the event can start (0 - 24).")]
    [Range(0f, 24f)]
    public float startHour;

    [Tooltip("Hour of the day when the event ends (0 - 24).")]
    [Range(0f, 24f)]
    public float endHour;

    [Tooltip("Chance for the event to occur within the valid time window (0 = never, 1 = always).")]
    [Range(0f, 1f)]
    public float chance;

    [Tooltip("The weather event associated with this skybox property.")]
    public WeatherSO weatherEvent;
    
    [SerializeField, Tooltip("Is the event active or not (so can it be triggered or not)")]
    private bool isEnabled = false;

    public bool IsEnabled
    {
        get => isEnabled;
        set => isEnabled = value;
    }

}
