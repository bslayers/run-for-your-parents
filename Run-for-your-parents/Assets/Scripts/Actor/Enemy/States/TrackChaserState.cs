using UnityEngine;

public class TrackChaserState : TrackState
{
    #region Variables


    #endregion

    #region Accessors


    #endregion


    #region Built-in


#endregion

#region Methods
    public override void StealBehaviour()
    {
        if(enemy.Target.transform.CompareTag("Player")){ return; }
        enemy.Target = null;
    }


#endregion

#region Coroutine


#endregion

#region Events


#endregion

#region Editor


#endregion

}