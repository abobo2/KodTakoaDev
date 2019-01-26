using UnityEngine;

public abstract class BaseInteractable : IInteractable
{
    public abstract void BeginInteract(GameObject other);
    public abstract void StopInteract(GameObject other);
    public abstract void InvokeInteraction();

    public bool CanBeInteractedWith;
}
