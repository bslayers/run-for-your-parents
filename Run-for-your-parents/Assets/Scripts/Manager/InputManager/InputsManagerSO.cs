using System.Collections;
using UnityEngine;
public abstract class InputsManagerSO : MonoBehaviour
{
    #region Variables
    [Tooltip("Enable input at the start of the game")]
    [SerializeField] protected bool enabledInputAtInit = false;

    #endregion

    #region Built-in

    void Awake()
    {
        StartCoroutine(WaitForInputActionManager());
    }

    protected virtual void StartInputsManager()
    {
        if (enabledInputAtInit) { EnableInput(); }
        else { DisableInput(); }
    }

    public abstract void DisableInput();
    public abstract void EnableInput();

    #endregion

    #region Coroutine
    private IEnumerator WaitForInputActionManager()
    {
        yield return new WaitUntil(() => InputActionManager.Instance != null && InputActionManager.Instance.inputAction != null);
        StartInputsManager();
    }

    #endregion
}