using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GuiManager : MonoBehaviour, IInitiatable
{
    public Slider energiBar;

    private GameState data;

    public void Initiate()
    {
        data = ServiceLocator.instance.GetInstanceOfType<GameState>();

        data.energy.Subscribe(newVal => OnEnergyVhange(newVal));
    }

    private void OnEnergyVhange(int newEnergyVal)
    {

    }
}
