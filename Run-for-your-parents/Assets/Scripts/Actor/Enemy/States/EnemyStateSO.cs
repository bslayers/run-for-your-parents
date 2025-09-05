using System.Collections;
using System.Numerics;

public abstract class EnemyStateSO
{
    #region Variables
    public Enemy enemy;
    public StateMachine stateMachine;

    #endregion

    #region Accessors


    #endregion


    #region Built-in
    public abstract void Enter();
    public abstract void Perform();
    public abstract void Exit();

    #endregion

    #region Methods


    #endregion

    #region Events


    #endregion

    #region Editor


    #endregion

}
