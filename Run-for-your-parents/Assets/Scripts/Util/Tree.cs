

public interface ITree<E>
{

    #region Accessors

    public abstract bool IsLeave { get; }

    #endregion


    #region Built-in

    public E GetElement();
    public ITree<E>[] GetBranches();



    #endregion


}