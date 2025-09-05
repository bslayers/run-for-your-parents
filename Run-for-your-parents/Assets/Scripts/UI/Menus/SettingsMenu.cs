using UnityEngine;

public class SettingsMenu : MenuSO
{
#region Variables


#endregion

#region Accessors


#endregion


#region Built-in

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void StartMenu()
    {
        base.StartMenu();
    }

#endregion

#region Methods
    public void OpenSettings()
    {
        OpenMenu();
        menusManager.ChangeMenu(this);
    }

#endregion


#region Events


#endregion

#region Editor


#endregion
}
