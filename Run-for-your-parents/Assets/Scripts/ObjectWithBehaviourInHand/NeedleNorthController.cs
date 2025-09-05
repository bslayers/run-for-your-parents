using UnityEngine;
public class NeedleNorthController : MonoBehaviour
{
    [Tooltip("Rotation speed of the needle")]
    [field: SerializeField]
    public float RotationSpeed { get; private set; } = 10f;

    [Tooltip("Use the camera's orientation")]
    [field: SerializeField]
    private bool useCameraOrientation = false;

    [Tooltip("Name of the camera in the scene")]
    [field: SerializeField]
    private string cameraName = "Main Camera";

    [field: SerializeField]
    private bool showGizmos = false;

    private Transform targetNorth;
    private Transform cameraTransform;

    [SerializeField]
    private bool findNorthByTag = false;

    private void Start()
    {
        
        targetNorth = findNorthByTag ? GameObject.FindGameObjectWithTag("NorthGoal").transform : Game.Instance.North;

        if (useCameraOrientation)
        {
            GameObject camObj = GameObject.Find(cameraName);
            if (camObj != null) cameraTransform = camObj.transform;
        }
    }

    private void Update()
    {
        RotateNeedle();
    }

    private void RotateNeedle()
    {

        if (targetNorth == null) { targetNorth = Game.Instance.North; if (targetNorth == null) { return; } }

        Vector3 localDirection, worldDirection;
        worldDirection = transform.position - targetNorth.position;
        worldDirection.y = 0f;

        if (useCameraOrientation && cameraTransform != null)
        {
            localDirection = transform.parent.InverseTransformDirection(worldDirection).normalized;
            localDirection.y = 0f;

            Quaternion targetLocalRotation = Quaternion.FromToRotation(Vector3.forward, localDirection);

            Quaternion offsetRotation = Quaternion.Euler(0f, 90f, 0f); // else it's not showing the goal
            targetLocalRotation *= offsetRotation;

            float rotation = RotationSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetLocalRotation, rotation);
        }

        else
        {
            localDirection = transform.parent.InverseTransformDirection(worldDirection).normalized;

            Quaternion desiredLocalRotation = Quaternion.LookRotation(localDirection, Vector3.up);
            Quaternion axisCorrection = Quaternion.FromToRotation(Vector3.forward, Vector3.right);
            Quaternion targetLocalRotation = desiredLocalRotation * axisCorrection;

            float rotation = RotationSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetLocalRotation, rotation);
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        if (targetNorth != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetNorth.position);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 0.5f);
    }
}