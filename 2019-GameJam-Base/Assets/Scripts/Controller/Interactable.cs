using UnityEngine;

interface IInteractable
{
    void BeginInteract(GameObject other);

    void StopInteract(GameObject other);

    void InvokeInteraction();
}