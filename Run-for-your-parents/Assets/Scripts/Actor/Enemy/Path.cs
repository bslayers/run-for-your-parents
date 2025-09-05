using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Path : MonoBehaviour
{
#region Variables
    [Tooltip("The list of waypoints to follow")]
    public List<Transform> waypoints = new List<Transform>();

    [Tooltip("If true, the path will be drawn in the editor")]
    [SerializeField] private bool alwaysDrawPath;
    [Tooltip("If true, the path will be drawn as a loop")]
    [SerializeField] private bool drawAsLoop;
    [Tooltip("If true, the path will be drawn with numbers")]
    [SerializeField] private bool drawNumbers;
    [Tooltip("The color of the path when drawn")]
    public Color debugColour = Color.white;

#endregion

#region Accessors


#endregion


#region Built-in

#endregion

#region Methods


#endregion


#region Events


#endregion

#region Editor
#if UNITY_EDITOR

    public void OnDrawGizmos()
    {
        if(alwaysDrawPath)
        {
            DrawPath();
        }
    }

    public void DrawPath()
    {
        for(int i=0; i<waypoints.Count; i++)
        {
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = debugColour;
            if(drawNumbers) Handles.Label(waypoints[i].position, i.ToString(), labelStyle);

            if(i >= 1)
            {
                Gizmos.color = debugColour;
                Gizmos.DrawLine(waypoints[i-1].position, waypoints[i].position);

                if(drawAsLoop) Gizmos.DrawLine(waypoints[waypoints.Count -1].position, waypoints[0].position);
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        if(alwaysDrawPath) return;
        else DrawPath();
        
    }
#endif
#endregion

}
