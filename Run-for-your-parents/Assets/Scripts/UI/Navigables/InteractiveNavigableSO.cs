
public abstract class InteractiveNavigableSO : NavigableSO
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

#endregion

#region Methods
    /// <summary>
    /// Perform an up action of the InteractiveNavigable
    /// </summary>
    public abstract void ActionUp();
    /// <summary>
    /// Perform a right action of the InteractiveNavigable
    /// </summary>
    public abstract void ActionRight();
    /// <summary>
    /// Perform a down action of the InteractiveNavigable
    /// </summary>
    public abstract void ActionDown();
    /// <summary>
    /// Perform a left action of the InteractiveNavigable
    /// </summary>
    public abstract void ActionLeft();

    /// <summary>
    /// Perform a confirm action of the InteractiveNavigable
    /// </summary>
    public virtual void Confirm()
    {
        Deselect();
        Hover();
    }

    /// <summary>
    /// Perform a back action of the InteractiveNavigable
    /// </summary>
    public virtual void Back()
    {
        Deselect();
        Hover();
    }

#endregion


#region Events



#endregion

#region Editor



#endregion

}