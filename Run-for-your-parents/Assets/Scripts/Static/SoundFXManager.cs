using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    #region Variables
    public static SoundFXManager Instance;

    [Tooltip("The audio source prefab to use for sound effects.")]
    [SerializeField]
    private AudioSource soundFXObject;

    private Dictionary<int, AudioSource> audioClipsToCheck = new Dictionary<int, AudioSource>();

    #endregion

    #region Accessors


    #endregion


    #region Built-in

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    #endregion

    #region Methods
    /// <summary>
    /// Start an audio clip at <paramref name="actorOfSound"/> position
    /// </summary>
    /// <param name="actorOfSound">GameObject where playing the audio clip</param>
    /// <param name="audioClip">audio clip to play</param>
    /// <param name="volume"></param>
    public int PlaySoundFXClip(GameObject actorOfSound, AudioClip audioClip, float volume, float maxDistance)
    {
        return PlaySoundFXClip(actorOfSound, audioClip, volume, false, maxDistance);
    }

    /// <summary>
    /// Start an audio clip at <paramref name="actorOfSound"/> position and check if <paramref name="audioClip"/> is in playing when <paramref name="withoutRepetition"/> is true
    /// </summary>
    /// <param name="actorOfSound">GameObject where playing the audio clip</param>
    /// <param name="audioClip">audio clip to play</param>
    /// <param name="volume"></param>
    /// <param name="withoutRepetition">determine if it's possible for actorOfSound to play the same audioClip</param>
    /// <returns>
    /// 0: audioClip played</br>
    /// 1: audioClip is null
    /// 2: audioClip already played
    /// </returns>
    public int PlaySoundFXClip(GameObject actorOfSound, AudioClip audioClip, float volume, bool withoutRepetition, float maxDistance)
    {
        if (audioClip == null) return 1;

        float finalVolume = volume / 100f;

        int status = -1;
        if (withoutRepetition)
        {
            status = CheckIsSoundPlaying(actorOfSound.GetInstanceID());
            if (status == 2) return 2;
        }

        //spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, actorOfSound.transform.position, Quaternion.identity);

        //add in check
        if (withoutRepetition)
        {
            if (status == 0) audioClipsToCheck.Add(actorOfSound.GetInstanceID(), audioSource);
            else if (status == 1) audioClipsToCheck[actorOfSound.GetInstanceID()] = audioSource;
        }

        //assign the audioClip
        audioSource.clip = audioClip;

        //assign volume
        audioSource.volume = finalVolume;

        audioSource.maxDistance = maxDistance;

        //get length of sound FX clip
        float clipLength = audioSource.clip.length;

        audioSource.Play();

        //remove the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);

        return 0;
    }

    /// <summary>
    /// Check if non-repetive audio source is playing for an actorOfSound
    /// </summary>
    /// <param name="key">the instance id of actorOfsound</param>
    /// <returns>0: instance id isn't a key of audioClipsToCheck;<br/>
    /// 1: the audio clip has been destroyed;<br/>
    /// 2: the audio clip is always playing.
    /// </returns>
    private int CheckIsSoundPlaying(int key)
    {
        try
        {
            if (audioClipsToCheck[key] == null) return 1;
            else return 2;
        }
        catch (KeyNotFoundException)
        {
            return 0;
        }
    }

    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}
