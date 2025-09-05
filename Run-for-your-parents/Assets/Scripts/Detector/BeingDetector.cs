using UnityEngine;

public class BeingDetector : DetectorSO
{
    #region Variables



    #endregion

    #region Accessors



    #endregion


    #region Built-in



    #endregion

    #region Methods


    protected override void DigitalizePlayer(PlayerBodyManager player, Collider other)
    {
        digitalizeSound.Play();
        player.Digitalize();
    }

    #endregion


    #region Events



    #endregion

    #region Editor



    #endregion

}