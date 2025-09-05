using UnityEngine;

[CreateAssetMenu(fileName = nameof(EnnemyData), menuName = "Resources/" + nameof(ActorData) + "/" + nameof(EnnemyData))]
public class EnnemyData : ScriptableObject
{
    public float stealDistance = 10f;
}