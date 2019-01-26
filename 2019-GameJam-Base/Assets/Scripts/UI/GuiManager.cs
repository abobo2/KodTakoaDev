using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class GuiManager : MonoBehaviour, IInitiatable
{
    public TextMeshProUGUI txtTimer;
    public Slider energyBar;
    public Image energyBarImage;

    public TasksListManager tasksManager;

    private GameState data;
    private GameEventsManager eventsManager;

    public void Initiate()
    {
        data = ServiceLocator.instance.GetInstanceOfType<GameState>();
        data.energy.Subscribe(newVal => OnEnergyChange(newVal));
        data.timer.Subscribe(newTime => OnTimerChanged(newTime));

        eventsManager = ServiceLocator.instance.GetInstanceOfType<GameEventsManager>();
        eventsManager.ObserveLevelTaskStarted().Subscribe(task => tasksManager.OnNewTaskStarted(task));
        eventsManager.ObserveLevelTaskCompleted().Subscribe(task => tasksManager.OnTaskCompleted(task));
        eventsManager.ObserveLevelStarted().Subscribe(lvl => OnNewLevelStarted(lvl));
        eventsManager.ObserveLevelFailed().Subscribe(lvl => OnLevelFailed(lvl));
        eventsManager.ObserveLevelCompleted().Subscribe(lvl => OnLevelCompleted(lvl));
    }

    private void OnEnergyChange(int newEnergyVal)
    {
        energyBar.value = (float)data.energy.Value / data.maxEnergy;
        energyBarImage.color = Color.Lerp(Color.red, Color.green, energyBar.value);
    }

    private void OnTimerChanged(int seconds)
    {
        if (seconds >= 0)
        {
            txtTimer.gameObject.SetActive(true);

            TimeSpan span = TimeSpan.FromSeconds(seconds);
            txtTimer.text = span.Minutes.ToString().PadLeft(2, '0') + ":" + span.Seconds.ToString().PadLeft(2, '0');
        }
        else
        {
            txtTimer.gameObject.SetActive(false);
        }
    }

    private void OnNewLevelStarted(Level lvl)
    {

    }

    private void OnLevelFailed(Level lvl)
    {

    }

    private void OnLevelCompleted(Level lvl)
    {

    }
}
