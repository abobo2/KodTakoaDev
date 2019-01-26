using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameState
{
    public ReactiveProperty<int> energy;
    public ReactiveProperty<int> timer;
    public int maxEnergy;

    public GameState()
    {
        energy = new ReactiveProperty<int>();
        timer = new ReactiveProperty<int>();
        maxEnergy = 100;

        energy.Subscribe(v => Debug.Log(v));
    }
}
