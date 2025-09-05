using UnityEngine;

[CreateAssetMenu(fileName = nameof(SoundData), menuName = "Resources/" + nameof(SoundData))]
public class SoundData : ScriptableObject
{
    public enum NoiseStrengthType { Weak, Normal, Loud }

    [Range(0.0f, 100.0f)]
    [Tooltip("Volume percentage relative to the global soundFXVolume")]
    public float volumePercentage = 100f;

    [Tooltip("The audio clip to play when the sound is emitted")]
    public AudioClip audioClip;

    [Tooltip("The type of noise strength")]
    public NoiseStrengthType noiseStrength;
    [Tooltip("The radius of the sound emission area")]
    public float radius;
}
