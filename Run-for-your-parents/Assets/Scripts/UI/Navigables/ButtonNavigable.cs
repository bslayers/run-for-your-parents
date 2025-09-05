using UnityEngine.UI;

public class ButtonNavigable : NavigableSO
{
    #region Variables
    protected Button button;


    #endregion

    #region Accessors


    #endregion


    #region Built-in
    protected override void Start()
    {
        base.Start();
        button = (Button)selectable;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #endregion

    #region Methods
    

    public override void Select() 
    {
        button.onClick.Invoke();
    }


    #endregion


    #region Events



    #endregion

    #region Editor



    #endregion

}