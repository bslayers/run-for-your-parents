using UnityEngine;

public static class SoundEmitor
{


    #region Methods

    public static void EmiteSound(GameObject sender, SoundData soundData)
    {
        EmiteSound(sender, soundData, false);
    }

    public static void EmiteSound(GameObject sender, SoundData soundData, float specialRadius)
    {
        EmiteSound(sender, soundData, specialRadius, false);
    }

    public static void EmiteSound(GameObject sender, SoundData soundData, bool withoutDuplication)
    {
        if (soundData == null) { return; }
        EmiteSound(sender, soundData, soundData.radius, withoutDuplication);
    }

    public static void EmiteSound(GameObject sender, SoundData soundData, float radius, bool withoutDuplication)
    {
        if (soundData == null) { return; }
        if (SoundFXManager.Instance.PlaySoundFXClip(sender, soundData.audioClip, soundData.volumePercentage, withoutDuplication, radius) == 2) { return; }

        if (radius == 0) { return; }

        float sphereRadius = radius;

        Collider[] detectedColliders = Physics.OverlapSphere(sender.transform.position, sphereRadius, LayerMask.GetMask("Sound Detector"));

        foreach (Collider collider in detectedColliders)
        {
            //Debug.Log(collider);
            if (!collider.TryGetComponent<Enemy>(out var ennemy)) { continue; }

            float distance = Vector3.Distance(sender.transform.position, ennemy.transform.position);
            float ratio = distance / radius;

            int effectiveStrength = CalculateSoundStrength(ratio, soundData);

            ennemy.ChangeTargetIfSoundHigher(sender, effectiveStrength, radius);
        }
    }

    private static int CalculateSoundStrength(float ratio, SoundData soundData)
    {
        int baseStrength = (int)soundData.noiseStrength;
        int effectiveStrength = baseStrength;
        if (ratio > 0.66f) effectiveStrength = baseStrength - 2;
        else if (ratio > 0.33f) effectiveStrength = baseStrength - 1;

        return effectiveStrength;
    }


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}
