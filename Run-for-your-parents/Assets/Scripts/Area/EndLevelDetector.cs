using UnityEditor;
using UnityEngine;

public class EndLevelDetector : MonoBehaviour
{
    

#region Variables
    private BoxCollider boxCollider;
    private MenusManager menusManager;

    #endregion

    #region Accessors


    #endregion


    #region Built-in

    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null) return;
        menusManager = FindFirstObjectByType<MenusManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            menusManager.OpenMenu(MenuData.MenuType.Victory);
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