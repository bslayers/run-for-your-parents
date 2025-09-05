using UnityEngine;

public class EventOnlyInteractable : InteractableObjectSO
{
    protected override void Interact(GameObject actor, BodyMemberType member, Collider collider) { }
}
