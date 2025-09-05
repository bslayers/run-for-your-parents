using UnityEngine;

public class Player : ActorSO
{
    #region Variables

    [RequiredReference]
    public PlayerData playerData;

    [Header("Components")]

    [RequiredReference]
    [Tooltip("The UIInputsManager component of the Player")]
    public UIInputsManager uIInputs;
    [RequiredReference]
    [Tooltip("The MenusManager component of the Player")]
    public MenusManager menusManager;
    public PlayerAnimatorManager animatorManager;

    [HideInInspector]
    public PlayerInputsManager playerInputs;


    [Header("Shared properties")]
    public IndexedHandManagerList handManagers;

    #endregion

    #region Accessors


    #endregion


    #region Built-in

    #endregion

    #region Functions

    protected override void ActorInit()
    {
        base.ActorInit();
        playerInputs = GetComponent<PlayerInputsManager>();
    }

    protected override void DeathFeedback()
    {
        throw new System.NotImplementedException();
    }

    protected override void HealFeedback()
    {
        throw new System.NotImplementedException();
    }

    protected override void HurtFeedback()
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #region Events



    #endregion

    #region Editor



    #endregion
}
