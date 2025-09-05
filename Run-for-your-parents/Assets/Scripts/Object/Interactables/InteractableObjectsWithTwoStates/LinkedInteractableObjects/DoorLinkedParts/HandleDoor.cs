using UnityEngine;

public class HandleDoor : DoorLinkedPart, IGenerator
{
    #region Variables

    [SerializeField]
    [RequiredReference]
    private GameObject detectorHandle;

    #endregion

    #region Accessors


    #endregion


    #region Built-in
    protected override void StartObject()
    {
        base.StartObject();
        IsDetectorHasBeenModified();
    }

    protected override void PerformFirstStateAction(Collider collider)
    {
        doorManager.OpenDoor();
    }

    protected override void PerformSecondStateAction(Collider collider)
    {
        doorManager.CloseDoor();
    }

    public void Generate()
    {
        IsDetector = Random.Range(0, 2) == 0;
    }

    #endregion

    #region Methods

    protected override void IsDetectorHasBeenModified()
    {
        GetComponent<MeshRenderer>().enabled = !isDetector;
        detectorHandle.SetActive(isDetector);
    }


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}
