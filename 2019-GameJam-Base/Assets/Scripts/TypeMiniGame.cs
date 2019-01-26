using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TypeMiniGame : MonoBehaviour
{
    public Text txt;

    public string letters;
    public int len;

    private char[] characters;
    private List<char> charactersToMatch;

    private bool isGameActive;
    private int curIndex;

    private Action onComplete;
    private Action onFailed;

    public void StartGame(Action onComplete, Action onFailed)
    {
        if (characters == null)
        {
            characters = letters.ToUpper().ToCharArray();
        }

        charactersToMatch = new List<char>();
        for (int i = 0; i < len; i++)
        {
            charactersToMatch.Add(characters[UnityEngine.Random.Range(0, characters.Length)]);
        }

        this.onComplete = onComplete;
        this.onFailed = onFailed;

        isGameActive = true;
        curIndex = 0;
        ShowNextExpectedButton(charactersToMatch[curIndex]);

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!isGameActive)
        {
            return;
        }

        if (Input.anyKeyDown)
        {
            char keyDown = Input.inputString.ToUpper()[0];

            if (keyDown == charactersToMatch[curIndex])
            {
                curIndex++;

                if (curIndex >= charactersToMatch.Count)
                {
                    Completed();
                    return;
                }

                ShowNextExpectedButton(charactersToMatch[curIndex]);
            }
            else
            {
                Failed();
            }
        }
    }

    public void ShowNextExpectedButton(char letter)
    {
        txt.text = letter.ToString();
    }

    private void Completed()
    {
        isGameActive = false;

        if (onComplete != null)
        {
            onComplete();
        }

        txt.text = "Completed!";
        StartCoroutine(HideWithDelay());
    }

    private void Failed()
    {
        isGameActive = false;

        if (onFailed != null)
        {
            onFailed();
        }

        txt.text = "Failed!";
        StartCoroutine(HideWithDelay());
    }

    private IEnumerator HideWithDelay()
    {
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
}
