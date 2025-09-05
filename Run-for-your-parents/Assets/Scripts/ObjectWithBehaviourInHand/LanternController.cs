using UnityEngine;

public class LanternController : MonoBehaviour, IUsableItem
{

    #region Variables

    [SerializeField]
    private Light[] lightSources;

    private bool isOn = false;
    #endregion

    #region Accessors


    #endregion


    #region Built-in

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnOFF(false);
    }

    public void UseItem(PlayerAnimatorManager animatorManager)
    {
        isOn = !isOn;

        if (lightSources != null)
        {
            OnOFF(isOn);
        }
    }

    public void OnOFF(bool isOn)
    {
        foreach (Light light in lightSources)
        {
            light.enabled = isOn;
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