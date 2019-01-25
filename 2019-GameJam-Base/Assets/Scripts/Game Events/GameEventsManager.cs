using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class GameEventsManager
{
    private Subject<Interactable> interactionsSubject;

    public GameEventsManager()
    {
        interactionsSubject = new Subject<Interactable>();
    }

    public IObservable<Interactable> ObserveInteractions()
    {
        return interactionsSubject;
    }

    public void InvokeInteraction(Interactable interactable)
    {
        interactionsSubject.OnNext(interactable);
    }
}

public enum Interactable
{
    Oven,
    Door,
    Tv,
    Phone,
    Window
}