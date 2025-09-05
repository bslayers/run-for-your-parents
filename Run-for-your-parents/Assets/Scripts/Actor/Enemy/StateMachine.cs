using UnityEngine;

public class StateMachine : MonoBehaviour
{
    #region Variables
    [Tooltip("The current state of the ennemy")]
    public EnemyStateSO currentState;
    [Tooltip("The previous state of the ennemy")]
    public EnemyStateSO previousState;

    #endregion

    #region Accessors


    #endregion


    #region Built-in
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.Perform();
        }
    }

    #endregion

    #region Methods
    /// <summary>
    /// Initialize the StateMachine
    /// </summary>
    public void Initialize()
    {
        ChangeState(new PatrolState());
    }

    /// <summary>
    /// Change the current state. Exit the current state and enter the newState
    /// </summary>
    /// <param name="newState">the new state to place in current state</param>
    public void ChangeState(EnemyStateSO newState)
    {
        if (currentState != null)
        {
            //run cleanup on activeState
            currentState.Exit();
            previousState = currentState;
        }

        //change to a new state
        currentState = newState;

        if (currentState != null)
        {
            //setup new state
            currentState.stateMachine = this;
            currentState.enemy = GetComponent<Enemy>();
            currentState.Enter();
        }
    }


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}
