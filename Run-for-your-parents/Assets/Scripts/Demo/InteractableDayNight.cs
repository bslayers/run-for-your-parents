using UnityEngine;

public class InteractableDayNight : InteractableObjectSO
{
    #region Variables

    public DayNightManager dayNightManager;

    [SerializeField]
    private float[] times;

    private int currentIndex = 0;

    

    #endregion

    #region Accessors


    #endregion


    #region Built-in
    protected override void Interact(GameObject actor, BodyMemberType member, Collider collider)
    {
        ChangeTime();
    }


    #endregion

    #region Methods

    private void ChangeTime()
    {
        if (times.Length == 0) { return; }
        ++currentIndex;
        dayNightManager.currentTime = times[currentIndex%times.Length];
    }

    #endregion

    #region Coroutine


    #endregion

    #region Events


    #endregion

    #region Editor


    #endregion

}