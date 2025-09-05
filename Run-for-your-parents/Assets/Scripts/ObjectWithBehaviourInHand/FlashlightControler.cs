using UnityEngine;

public class FlashlightControler : MonoBehaviour, IUsableItem
{
    #region Variables

    [SerializeField]
    private Light lightSources;

    private bool isOn = false;

    private AudioSource audioSource;

    [Tooltip("Sound to play when the flashlight is turned on or off")]
    public AudioClip soundOnOff;

    #endregion

    #region Accessors


    #endregion


    #region Built-in

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lightSources.enabled = false;
    }

    public void UseItem(PlayerAnimatorManager animatorManager)
    {
        isOn = !isOn;

        if (lightSources != null)
        {
            lightSources.enabled = isOn;
            if (soundOnOff != null)
            {
                // It don't work  
                //audioSource.PlayOneShot(soundOnOff);
            }
        }
        
    }

    #endregion

    #region Methods


    #endregion

    #region Coroutine


    #endregion

    #region Events


    #endregion

    #region Editor


    #endregion

}