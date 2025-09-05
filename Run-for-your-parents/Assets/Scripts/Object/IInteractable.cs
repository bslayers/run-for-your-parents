using System;
using UnityEngine;

public interface IInteractable
{
    #region Built-in

    string GetMessageToPrompt();

    void DestroyInteractable();

    /// <summary>
    /// This function will be called from our <paramref name="actor"/>
    /// </summary>
    /// <param name="actor">a GameObject representin the actor who interact with the interactable</param>
    /// <param name="member">the member of a player actor which interact with the interactable</param>
    /// <param name="collider">which collider of the interactable the actor had interacted</param>
    void BaseInteract(GameObject actor, BodyMemberType member, Collider collider);

    #endregion
}