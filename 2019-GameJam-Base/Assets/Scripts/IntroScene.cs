using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScene : MonoBehaviour
{
    public Image introImg;
    public Image screen1;
    public Image screen2;
    public Image screen3;

    public GameObject goBtnNext;
    public GameObject goBtnStart;

    private int currentScreenNum = 1;

    private void Start()
    {
        introImg.gameObject.SetActive(false);
        screen1.gameObject.SetActive(false);
        screen2.gameObject.SetActive(false);
        screen3.gameObject.SetActive(false);
        goBtnNext.SetActive(false);

        StartCoroutine(ShowIntroImage());
    }

    private IEnumerator ShowIntroImage()
    {
        introImg.gameObject.SetActive(true);

        Color32 startColor = new Color32(255, 255, 255, 0);
        Color32 endColor = new Color32(255, 255, 255, 255);

        Vector3 startScale = new Vector3(0.7f, 0.7f, 1f);
        Vector3 endScale = Vector3.one;

        float currentTime = 0f;
        float time = 1.5f;

        while (currentTime < time)
        {
            introImg.color = Color32.Lerp(startColor, endColor, currentTime / time);
            introImg.rectTransform.localScale = Vector3.Lerp(startScale, endScale, currentTime / time);

            currentTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(3f);

        currentTime = 0;
        time = 0.75f;

        while (currentTime < time)
        {
            introImg.color = Color32.Lerp(endColor, startColor, currentTime / time);
            currentTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(ShowScreen(currentScreenNum));
    }

    private IEnumerator ShowScreen(int screenNum)
    {
        Image screenImg = null;
        if (screenNum == 1)
        {
            screenImg = screen1;
        }
        else if (screenNum == 2)
        {
            screenImg = screen2;
        }
        else
        {
            screenImg = screen3;
        }

        screenImg.gameObject.SetActive(true);

        Color32 startColor = new Color32(255, 255, 255, 0);
        Color32 endColor = new Color32(255, 255, 255, 255);

        screenImg.color = startColor;

        float currentTime = 0f;
        float time = 0.75f;

        while (currentTime < time)
        {
            screenImg.color = Color32.Lerp(startColor, endColor, currentTime / time);
            currentTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(3f);

        if (screenNum < 3)
        {
            goBtnNext.SetActive(true);
        }
        else
        {
            goBtnStart.SetActive(true);
        }
    }

    public void OnNextClick()
    {
        currentScreenNum++;

        goBtnNext.SetActive(false);
        StartCoroutine(ShowScreen(currentScreenNum));
    }

    public void OnStartClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainGameScene");
    }
}
