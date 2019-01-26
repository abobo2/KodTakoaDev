using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class GameEventsManager
{
    private Subject<Interactable> interactionsSubject;

    private Subject<Level> levelStarted;
    private Subject<LevelTask> levelTaskStarted;

    private Subject<Level> levelCompleted;
    private Subject<LevelTask> levelTaskCompleted;

    private Subject<Level> levelFailed;

    public GameEventsManager()
    {
        interactionsSubject = new Subject<Interactable>();
        levelStarted = new Subject<Level>();
        levelTaskStarted = new Subject<LevelTask>();

        levelCompleted = new Subject<Level>();
        levelTaskCompleted = new Subject<LevelTask>();

        levelFailed = new Subject<Level>();
    }

    public IObservable<Interactable> ObserveInteractions()
    {
        return interactionsSubject;
    }

    public IObservable<Level> ObserveLevelStarted()
    {
        return levelStarted;
    }

    public IObservable<LevelTask> ObserveLevelTaskStarted()
    {
        return levelTaskStarted;
    }

    public IObservable<Level> ObserveLevelCompleted()
    {
        return levelCompleted;
    }

    public IObservable<LevelTask> ObserveLevelTaskCompleted()
    {
        return levelTaskCompleted;
    }

    public IObservable<Level> ObserveLevelFailed()
    {
        return levelFailed;
    }

    public void InvokeInteraction(Interactable interactable)
    {
        interactionsSubject.OnNext(interactable);
    }

    public void InvokeLevelStarted(Level newLevel)
    {
        levelStarted.OnNext(newLevel);
    }

    public void InvokeLevelTaskStarted(LevelTask newLevelTask)
    {
        levelTaskStarted.OnNext(newLevelTask);
    }

    public void InvokeLevelCompleted(Level newLevel)
    {
        levelCompleted.OnNext(newLevel);
    }

    public void InvokeLevelTaskCompleted(LevelTask newLevelTask)
    {
        levelTaskCompleted.OnNext(newLevelTask);
    }

    public void InvokeLevelFailed(Level newLevel)
    {
        levelFailed.OnNext(newLevel);
    }
}

public enum Interactable
{
    None,
    Oven,
    TV,
    Baniza,
    Sarmi,
    Lyuteniza
}