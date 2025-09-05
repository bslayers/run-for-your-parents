using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
#region Variables
    public enum VolumeType{Master, SFX, Music};

    [Tooltip("The instance of the SoundMixerManager")]
    public static SoundMixerManager Instance;
    [Tooltip("AudioMixer to control the volume")]
    [SerializeField] private AudioMixer audioMixer;

    public const string MASTER_VOLUME = "masterVolume";
    public const string SFX_VOLUME = "soundFXVolume";
    public const string MUSIC_VOLUME = "musicVolume";
    private static readonly Dictionary<VolumeType, string> volumeName = new Dictionary<VolumeType, string>(){
        {VolumeType.Master, MASTER_VOLUME},
        {VolumeType.SFX, SFX_VOLUME},
        {VolumeType.Music, MUSIC_VOLUME}
    };

    #endregion

    #region Accessors


    #endregion


    #region Built-in

    void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    #endregion

    #region Methods
    public void UpdateMixer(GameData data)
    {
        SetVolume(MASTER_VOLUME, data.masterVolume);
        SetVolume(MUSIC_VOLUME, data.musicVolume);
        SetVolume(SFX_VOLUME, data.sfxVolume);
    }

    public void RecoverAllVolume(ref GameData data)
    {
        data.masterVolume = GetVolumePercentage(MASTER_VOLUME);
        data.musicVolume = GetVolumePercentage(MUSIC_VOLUME);
        data.sfxVolume = GetVolumePercentage(SFX_VOLUME);
    }

    public float GetVolume(VolumeType volume)
    {
        return GetVolume(volumeName.GetValueOrDefault(volume));
    }

    public float GetVolume(string volumeName)
    {
        float val;
        audioMixer.GetFloat(volumeName, out val);
        return val;
    }

    public float GetVolumePercentage(VolumeType volumeName)
    {
        return Mathf.Pow(10f, GetVolume(volumeName) / 20f);
    }

    public float GetVolumePercentage(string volumeName)
    {
        return Mathf.Pow(10f, GetVolume(volumeName) / 20f);
    }

    public void SetVolume(VolumeType volume, float level)
    {
        SetVolume(volumeName.GetValueOrDefault(volume), level);
    }

    public void SetVolume(string volumeName, float level)
    {
        audioMixer.SetFloat(volumeName, Mathf.Log10(level) * 20);
    }

  

#endregion


#region Events


#endregion

#region Editor


#endregion
}
