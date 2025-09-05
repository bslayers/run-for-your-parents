using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerInputsManager))]
public class PlayerLook : PlayerComponentSO
{
    #region Variables
    private PlayerAnimatorManager animatorManager;

    [Tooltip("Component of the camera")]
    public Camera cam;
    [SerializeField]
    private Transform BoneToFollow;

    private IndexedHandManagerList handManagers;

    private float xRotation = 0f;

    [Header("Parameters")]

    [SerializeField]
    [Tooltip("Sensibility of the camera")]
    private Vector2 Sensibility = new(10f, 10f);

    [SerializeField]
    private MinMaxFloat maxRotationX = new(-80f, 80f);

    [SerializeField]
    private Vector3 positionOffset;




    #endregion

    #region Accessors


    #endregion


    #region Built-in

    protected override void InitPlayer()
    {
        base.InitPlayer();
        handManagers = player.handManagers;
        animatorManager = player.animatorManager;
        StartCoroutine(DelayAudioListinerActivation());
    }

    void LateUpdate()
    {
        UpdateCameraTransform();
    }


    #endregion

    #region Methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        //calculate camera rotation for looking up and down
        xRotation -= mouseY * Time.deltaTime * Sensibility.y;
        xRotation = Mathf.Clamp(xRotation, maxRotationX.min, maxRotationX.max);
        //apply this of our camera
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //if (animatorManager.isShowingItem) { RotateObject(); }

        BoneToFollow.transform.rotation = cam.transform.rotation;

        //rotate for looking left and right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * Sensibility.x);

    }

    private void UpdateCameraTransform()
    {
        cam.transform.position = BoneToFollow.position;
        cam.transform.localPosition += positionOffset;
    }

    private void RotateObject()
    {
        Vector3 camRotation = cam.transform.rotation.eulerAngles;
        if (animatorManager.ObjectRotationInHandFollowCamera[Hand.Left])
        {
            handManagers[Hand.Left].gameObject.transform.rotation = Quaternion.Euler(camRotation);
        }
        if (animatorManager.ObjectRotationInHandFollowCamera[Hand.Right])
        {
            handManagers[Hand.Right].gameObject.transform.rotation = Quaternion.Euler(camRotation);
        }
    }


    #endregion

    #region Coroutine

    IEnumerator DelayAudioListinerActivation()
    {
        AudioListener listener = cam.GetComponent<AudioListener>();
        listener.enabled = false;
        yield return new WaitForSeconds(1.5f);

        listener.enabled = true;
    }

    #endregion

    #region Events


    #endregion

    #region Editor
#if UNITY_EDITOR

    void OnValidate()
    {
        UpdateCameraTransform();
    }

#endif
    #endregion
}
