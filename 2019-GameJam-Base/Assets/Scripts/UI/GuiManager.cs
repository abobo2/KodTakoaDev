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

    public TasksListManager tasksManager;

    private GameState data;
    
    public void Initiate()
    {
        data = ServiceLocator.instance.GetInstanceOfType<GameState>();

        data.energy.Subscribe(newVal => OnEnergyChange(newVal));
        data.timer.Subscribe(newTime => OnTimerChanged(newTime));
    }

    private void OnEnergyChange(int newEnergyVal)
    {
        energyBar.value = (float)data.energy.Value / data.maxEnergy;
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
}
