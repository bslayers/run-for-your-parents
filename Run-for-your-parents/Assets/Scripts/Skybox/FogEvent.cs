using UnityEngine;

[CreateAssetMenu(fileName = "Fog Event", menuName = "Weather/FogEvent")]
public class FogEvent : WeatherSO
{
    #region Variables
    [SerializeField, Tooltip("The fog density of the fog event")]
    private float fogDensity = 0.1f;

    [SerializeField, Tooltip("Fog color for when there is fog during the day, used to tint the fog")]
    private Color fogDayColor = new(0.8f, 0.8f, 0.8f);

#endregion

    #region Accessors


    #endregion


    #region Built-in

    public override void StartWeather()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = fogDayColor;
        RenderSettings.fogDensity = fogDensity;
    }

    public override void StopWeather()
    {
        RenderSettings.fog = false;
    }

#endregion

#region Methods


#endregion


#region Events


#endregion

#region Editor


#endregion
    
}