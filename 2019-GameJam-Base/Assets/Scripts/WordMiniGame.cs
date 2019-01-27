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

    public float ErrorDelay = .40f;

    private bool hasErrored;
    

    private char currentTargetCharacter => currentPhrase.Substring(currentIndex).ToLower()[0];
    private int currentIndex;


    private List<string> phraseList = new List<string>()
    {
        "Flip over the steak",
        "Fry some eggs",
        "Add a touch of love",
        "Add a lot of butter",
        "Take their bread away",
        "Add white to the lyutenica",
        "Bake some cake",
        "Make some kozunak",
        "This is a piece of cake",
        "Easy as pie",
        "Turn the oven on",
        "Add LOTS of spices",
        "Cut potatoes",
        "Drink Rakia",
        "Dang they ruined the country",
        "Make kompot",
        "Add 2 tbsp pig fat",
        "I hope they are not vegan",
        "I have no time",
        "I have to hurry up!",
        "Where is Courage?",
        "Where is my husband?",
        "Home sweet home",
        "Those cookies were lit",
        "Towels can't tell jokes",
        "Make coffee",
        "Sausage puns are the wurst",
        "Want to hear a pizza joke?",
        "Praise the sun",
        "Do she me who you",
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
            if (string.IsNullOrEmpty(Input.inputString))
            {
                return;
            }
            
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