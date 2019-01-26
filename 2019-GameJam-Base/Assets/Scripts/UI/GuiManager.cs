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
    public Slider energiBar;

    public TasksListManager tasksManager;

    private GameState data;

    public void Initiate()
    {
        data = ServiceLocator.instance.GetInstanceOfType<GameState>();

        data.energy.Subscribe(newVal => OnEnergyVhange(newVal));
        data.timer.Subscribe(newTime => OnTimerChanged(newTime));
    }

    private void OnEnergyVhange(int newEnergyVal)
    {

    }

    private void OnTimerChanged(int seconds)
    {

    }
}
