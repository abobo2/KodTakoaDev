using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameState
{
    public ReactiveProperty<int> energy;
    public ReactiveProperty<int> timer;

    public GameState()
    {
        energy = new ReactiveProperty<int>();

        energy.Subscribe(v => Debug.Log(v));
    }
}
