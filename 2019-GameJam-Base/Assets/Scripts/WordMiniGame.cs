using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts;
using TMPro;
using Random = UnityEngine.Random;
using UniRx;
public class WordMiniGame : MonoBehaviour
{
    public Color ErrorColor;

    public Color DefaultUnfinishedColor;

    public Color FinishedColor;

    public GameObject MiniGameContainer;
    public TextMeshProUGUI finishedText;

    public TextMeshProUGUI unfinishedText;

    private string currentPhrase;

    public static bool GameIsRunning;
    private Action onComplete;
    private Action onWrongInput;

    public float ErrorDelay = .75f;

    private bool hasErrored;
    

    private char currentTargetCharacter => currentPhrase.Substring(currentIndex)[0];
    private int currentIndex;


    public List<string> phraseList = new List<string>()
    {
        "banica",
        "lutenica",
        "djangur",
        "pravene",
        "mandja",
        "musaka",
        "patka",
        "govedo",
        "6ibek",
        "maniak",
        "lice",
        "pls edit",
        "susipaha q taq durjava",
        "pi6i kur i begai",
    };

    public void StartMinigame(Action onComplete, Action onWrongInput){
        GameIsRunning = true;
        MiniGameContainer.SetActive(true);

        this.onComplete = onComplete;
        this.onWrongInput = onWrongInput;

        currentIndex = 0;
        PickPhrase();
        SetTexts();
    }

    public void PickPhrase(){

        currentPhrase = phraseList[Random.Range(0, phraseList.Count)];
        MiniGameContainer.gameObject.SetActive(true);
    }


    private void Update()
    {
        if (!GameIsRunning || hasErrored)
        {
            return;
        }

        if (Input.anyKeyDown)
        {
            char keyDown = Input.inputString.ToLower()[0];


            Debug.Log("Current Target Character is " + currentTargetCharacter);
            Debug.Log("KeyDown is " + keyDown);
            if (keyDown == currentTargetCharacter)
            {
                currentIndex++;
                Screenshake.RequestScreenshake(0.5f, 2);

                if (currentIndex >= currentPhrase.Length)
                {
                    SetTexts();
                    Completed();
                    return;
                }
                SetTexts();
            }
            else
            {
                Screenshake.RequestScreenshake(1.5f, 3);
                WrongInput();
            }
        }
    }

    private void SetTexts()
    {
        finishedText.text = currentPhrase.Substring(0, currentIndex);
        unfinishedText.text = currentPhrase.Substring(currentIndex);
    }

    private void WrongInput(){
        onWrongInput();
        hasErrored = true;
        unfinishedText.color = ErrorColor;
        Observable.Timer(TimeSpan.FromSeconds(ErrorDelay)).Subscribe(_ => 
        {
            unfinishedText.color = DefaultUnfinishedColor;
            hasErrored = false;
        });
    }
    
    private void Completed()
    {
        GameIsRunning = false;

        if (onComplete != null)
        {
            onComplete();
        }

        Observable.Timer(TimeSpan.FromSeconds(0.75f)).Subscribe(_ => Hide());
    }
    private void Failed()
    {
        GameIsRunning = false;

        if (onWrongInput != null)
        {
            onWrongInput();
        }

        Observable.Timer(TimeSpan.FromSeconds(0.75f)).Subscribe(_ => Hide());
    }

    public void Hide(){
        MiniGameContainer.gameObject.SetActive(false);
    }

}