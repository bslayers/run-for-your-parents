using UnityEngine;

public class FootCollider : MemberCollider
{
    #region Variables

    private PlayerSounds playerSounds;
    private SoundData sound;


    #endregion

    #region Accessors


    #endregion


    #region Built-in


    protected override void AwakeMemberCollider()
    {
        playerSounds = player.playerData.sounds;
        sound = playerSounds.footSound;
        base.AwakeMemberCollider();
    }


    #endregion

    #region Methods





    protected override void MakeSound()
    {
        float radius = sound.radius;
        if (motor.IsRunning)
        {
            radius *= playerSounds.whenRunning;
        }
        else if (motor.IsSneeking)
        {
            radius *= playerSounds.whenSneeking;
        }

        SoundEmitor.EmiteSound(player.gameObject, sound, radius);
    }


    #endregion

    #region Coroutine


    #endregion

    #region Events




    #endregion

    #region Editor


    #endregion

}