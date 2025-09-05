using System;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(PlayerData), menuName = "Resources/" + nameof(ActorData) + "/" + nameof(PlayerData))]
public class PlayerData : ActorData
{

    [Header("For Interacting")]
    public float rayCastLength;

    [Header("For Camera")]
    public Vector2 sensibility;
    public MinMaxFloat maxRotationXAxis;
    public Vector3 positionOffset;

    [Header("For Motor")]

    public float gravity;
    public float jumpHight;

    public PlayerSpeeds speeds;

    public PlayerSounds sounds;



}

[Serializable]
public class PlayerSpeeds
{
    public float walkingSpeed;
    public float runningSpeed;
    public float sneekingSpeed;
    public float crawlingSpeed;
    public float slitheringSpeed;
}

[Serializable]
public class PlayerSounds
{
    public SoundData footSound;
    public SoundData crawlSound;
    public SoundData slitherSound;

    [Header("Radius multiplicator for movement (based on " + nameof(footSound) + ")")]
    public float whenRunning = 1f;
    public float whenSneeking = 1f;
    public float whenLanding = 1f;
}
