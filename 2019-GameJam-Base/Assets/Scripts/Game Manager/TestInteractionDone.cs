using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractionDone : MonoBehaviour
{
    public Interactable interactable;

    private GameEventsManager gameEventsManager;

    public void Start()
    {
        gameEventsManager = ServiceLocator.instance.GetInstanceOfType<GameEventsManager>();
    }

    public void Click()
    {
        Debug.Log("Task clicked: " + interactable.ToString());
        gameEventsManager.InvokeInteraction(interactable);
    }
}
