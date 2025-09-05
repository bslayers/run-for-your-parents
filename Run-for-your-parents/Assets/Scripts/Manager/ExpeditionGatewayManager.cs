using UnityEngine;

public class ExpeditionGatewayManager : MonoBehaviour
{
#region Variables


#endregion

#region Accessors


#endregion


#region Built-in
    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {
        
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

#endregion

#region Methods


#endregion


#region Events
    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger) return;

        if(other.CompareTag("Player"))
        {
            other.GetComponent<MenusManager>().OpenMenu(MenuData.MenuType.Expedition);
        }
    }

#endregion

#region Editor


#endregion
}