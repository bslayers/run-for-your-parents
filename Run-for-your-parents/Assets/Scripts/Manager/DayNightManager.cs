using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayNightManager : MonoBehaviour
{
    #region Variables

    [Header("Configuration")]

    [Tooltip("Seconds per in-game hour")]
    [Range(0.1f, 3600f)]
    public float secondsPerHour = 5f;

    [Tooltip("Total length of a full day in real seconds (calculated)")]
    public float DayLengthInSeconds => secondsPerHour * 24f;

    [Tooltip("Directional light representing the sun")]
    public Light sun;

    [Tooltip("Directional light representing the moon")]
    public Light moon;

    [Tooltip("Current in-game time (0-24 hours)")]
    [Range(0f, 24f)]
    public float currentTime;

    [Tooltip("Minimum thickness of the atmosphere (night time)")]
    [Range(0f, 1f)]
    public float minThickness = 0.1f;

    [Tooltip("Maximum thickness of the atmosphere (daytime, sunrise, sunset)")]
    [Range(1f, 4f)]
    [SerializeField]
    private float maxThickness = 2f;

    [Tooltip("Current thickness of the atmosphere")]
    [Range(0f, 10f)]
    public float currentThickness;

    [field: SerializeField]
    [Tooltip("Enable or disable the day-night cycle")]
    private bool hasCompletedDayCycle = false;

    [Tooltip("List of properties for different skybox events used for thing like weather")]
    public List<SkyBoxProperties> skyBoxProperties = new();

    public bool activateWeatherEvents = true;


    [Header("Day Schedule")]

    [Tooltip("Start of the day period (in hours, 0-24)")]
    [Range(0f, 24f)]
    public float sunrise;

    [Tooltip("Start of the sunset period (in hours, 0-24)")]
    [Range(0f, 24f)]
    public float startSunset;

    [Tooltip("Start of the day period (in hours, 0-24)")]
    [Range(0f, 24f)]
    public float startDay;

    [Tooltip("End of the sunset period (in hours, 0-24)")]
    [Range(0f, 24f)]
    public float endSunset;

    [Header("Moon Appearance")]
    [Tooltip("Duration in seconds for the moon to fade in/out")]
    [Range(0f, 5f)] public float moonFadeDuration = 1f;

    [Tooltip("Maximum intensity of the moon")]
    [Range(0f, 2f)] public float moonIntensityMax = 0.2f;

    /// <summary>
    /// Shader property ID for atmosphere thickness
    /// </summary>
    private int atmosID;

    /// <summary>
    /// Shader property ID for exposure
    /// </summary>
    private int exposureID;

    /// <summary>
    /// Target thickness value based on the current time
    /// </summary>
    private float targetThickness;

    /// <summary>
    /// The last hour that the day have been
    /// </summary>
    private int lastHour = -1;

    private float moonFadeProgress = 0f;


    /// <summary>
    /// Dictionary to track which skybox properties have been tried this cycle
    /// </summary>
    private readonly Dictionary<SkyBoxProperties, bool> eventAlreadyTriedDay = new();


    #endregion

    #region Accessors

    /// <summary>
    /// Check if the current time is within the day period
    /// </summary>
    public bool IsNight => currentTime >= endSunset || currentTime < sunrise;

    #endregion

    #region Built-in

    void Awake()
    {
        atmosID = Shader.PropertyToID("_AtmosphereThickness");
        exposureID = Shader.PropertyToID("_Exposure");
    }

    private void Start()
    {
        lastHour = Mathf.FloorToInt(currentTime);
        UpdateTime();
        UpdateSunAndMoon(true);
    }

    private void Update()
    {
        UpdateTime();
        UpdateSunAndMoon(false);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Update the current time based on the elapsed time since the last frame.
    /// </summary>
    private void UpdateTime()
    {
        if (hasCompletedDayCycle) return;

        currentTime += Time.deltaTime / secondsPerHour;

        if (currentTime >= 24f)
        {
            currentTime = 24f;
            hasCompletedDayCycle = true;
            eventAlreadyTriedDay.Clear();
        }
    }

    /// <summary>
    /// Update the sun's rotation and light settings based on the current time. It also manage weather events based on the time of day.
    /// </summary>
    private void UpdateSunAndMoon(bool start)
    {
        float sunAngle = SunMove();
        sun.transform.localRotation = Quaternion.Euler(sunAngle, 170f, 0f);
        moon.transform.localRotation = Quaternion.Euler(45f, 170f, 0f);

        float sunHeight = Vector3.Dot(sun.transform.forward, Vector3.down);
        float lightFactor = Mathf.Clamp01(sunHeight);

         UpdateRenderSettings(lightFactor);

        int currentHour = Mathf.FloorToInt(currentTime);
        if (currentHour != lastHour)
        {
            lastHour = currentHour;
            WeatherEvent();
        }
    }

    /// <summary>
    /// Calculate the sun's angle based on the current time and the defined sunrise, sunset, and day periods.
    /// </summary>
    /// <returns></returns>
    private float SunMove()
    {
        float sunAngle;

        if (currentTime >= sunrise && currentTime < startDay)
        {
            sunAngle = ComputeSunAngle(sunrise, startDay, -90f, 0f);
        }
        else if (currentTime >= startDay && currentTime < startSunset)
        {
            sunAngle = ComputeSunAngle(startDay, startSunset, 0f, 170f);
        }
        else if (currentTime >= startSunset && currentTime < endSunset)
        {
            sunAngle = ComputeSunAngle(startSunset, endSunset, 170f, 180f);
        }
        else
        {
            sunAngle = ComputeSunAngle(0f, sunrise, 180f, 200f);
        }
        return sunAngle;
    }

    /// <summary>
    /// Compute the sun angle based on the current time and the specified start and end angles.
    /// </summary>
    /// <param name="startHour"></param>
    /// <param name="endHour"></param>
    /// <param name="startAngle"></param>
    /// <param name="endAngle"></param>
    /// <returns></returns>
    private float ComputeSunAngle(float startHour, float endHour, float startAngle, float endAngle)
    {
        float t = Mathf.InverseLerp(startHour, endHour, currentTime);
        return Mathf.Lerp(startAngle, endAngle, t);
    }

    /// <summary>
    /// Update the render settings based on the current light factor.
    /// </summary>
    /// <param name="lightFactor"></param>
     private void UpdateRenderSettings(float lightFactor)
    {
        sun.enabled = true;
        sun.intensity = Mathf.Lerp(minThickness, maxThickness, lightFactor);
        sun.color = GetSunColorByHour(lightFactor);

        UpdateMoonAppearance();
        
        RenderSettings.sun = IsNight ? moon : sun;

        UpdateTickness(lightFactor);

        RenderSettings.ambientIntensity = Mathf.Lerp(0.1f, 1f, lightFactor);
        RenderSettings.ambientLight = Color.Lerp(new Color(0.02f, 0.02f, 0.05f), Color.white, lightFactor);
        RenderSettings.reflectionIntensity = Mathf.Lerp(0.2f, 1f, lightFactor);
        RenderSettings.skybox.SetColor("_SkyTint", Color.Lerp(new Color(0.05f, 0.05f, 0.1f), Color.gray, lightFactor));

        DynamicGI.UpdateEnvironment();
    }

    /// <summary>
    /// Update the atmosphere thickness based on the current light factor.
    /// </summary>
    /// <param name="lightFactor"></param>
    private void UpdateTickness(float lightFactor)
    {
        targetThickness = Mathf.Lerp(minThickness, maxThickness, lightFactor);

        float tickSpeed = Time.deltaTime / secondsPerHour;
        currentThickness = Mathf.MoveTowards(currentThickness, targetThickness, tickSpeed);

        RenderSettings.skybox.SetFloat(atmosID, currentThickness);
        RenderSettings.skybox.SetFloat(exposureID, currentThickness);
    }

    /// <summary>
    /// Update the moon's appearance, including fading and atmospheric effects, based on the current time.
    /// </summary>
    private void UpdateMoonAppearance()
    {
        float fadeSpeed = Time.deltaTime / moonFadeDuration;
        moonFadeProgress = IsNight ? Mathf.Min(1f, moonFadeProgress + fadeSpeed) : Mathf.Max(0f, moonFadeProgress - fadeSpeed);

        float moonIntensity = Mathf.Lerp(0f, moonIntensityMax, moonFadeProgress);
        moon.enabled = moonIntensity > 0.01f;
        moon.intensity = moonIntensity;
        moon.color = new Color(0.4f, 0.4f, 0.8f);
    }


    /// <summary>
    /// Get the sun color based on the current light factor.
    /// </summary>
    /// <param name="lightFactor"></param>
    /// <returns></returns>
    private Color GetSunColorByHour(float lightFactor)
    {
        if (IsNight)
        {
            return new Color(0.05f, 0.05f, 0.1f);
        }
        else
        {
            return Color.Lerp(new Color(0.8f, 0.4f, 0.1f), Color.white, lightFactor);
        }
    }

    /// <summary>
    /// Trigger weather events based on the current time and defined event properties.
    /// </summary>
    private void WeatherEvent()
    {
        if (!activateWeatherEvents) { return; }
        foreach (var prop in skyBoxProperties)
        {
            bool inWindow = currentTime >= prop.startHour && currentTime < prop.endHour;
            if (!inWindow)
            {
                prop.weatherEvent.StopWeather();
                continue;
            }

            if (!eventAlreadyTriedDay.TryGetValue(prop, out bool tried) || !tried)
            {
                eventAlreadyTriedDay[prop] = true;
                if (Random.value > prop.chance)
                {
                    prop.weatherEvent.StopWeather();
                    continue;
                }
            }

            prop.weatherEvent.StartWeather();
        }
    }


    #endregion

    #region Events

    [HideInInspector]
    public UnityEvent OnSunset;
    [HideInInspector]
    public UnityEvent OnSunrise;

    #endregion

    #region Editor

    #endregion
}
