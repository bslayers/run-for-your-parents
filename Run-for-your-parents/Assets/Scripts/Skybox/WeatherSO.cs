using UnityEngine;

public abstract class WeatherSO : ScriptableObject, IWeather
{
    public abstract void StartWeather();
    public abstract void StopWeather();
}
