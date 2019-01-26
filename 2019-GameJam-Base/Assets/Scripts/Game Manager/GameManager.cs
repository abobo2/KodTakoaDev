using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour, IInitiatable
{
    public Level[] levels;

    private int currentLevelIndex;
    private Level currentLevel;

    private int currentTaskIndex;
    private LevelTask currentTask;

    private Coroutine timerCoroutine;

    private GameState gameState;
    private GameEventsManager gameEventsManager;

    private Interactable expectedInteractable;

    private bool isGameRunning;

    public void Initiate()
    {
        gameEventsManager = ServiceLocator.instance.GetInstanceOfType<GameEventsManager>();
        gameState = ServiceLocator.instance.GetInstanceOfType<GameState>();

        gameEventsManager.ObserveInteractions().Subscribe(interaction => 
        {
            if (!isGameRunning)
            {
                return;
            }

            if (currentTask.interactionToBeDone == interaction)
            {
                CurrentTaskCompleted();
            }
        });

        gameState.energy.Subscribe(en =>
        {
            if (en <= 0)
            {
                LevelFailed();
            }
        });
    }

    public void Start()
    {
        PrepareStartOfGame();
        StarGame();
    }

    private void PrepareStartOfGame()
    {
        gameState.energy.Value = 100;
    }

    private IEnumerator LooseEnergyOverTime()
    {
        while (gameState.energy.Value > 0)
        {
            yield return new WaitForSeconds(1f);

            gameState.energy.Value -= 1;
        }
    }

    public void StarGame()
    {
        isGameRunning = true;
        currentLevelIndex = -1;

        StartCoroutine(LooseEnergyOverTime());
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        currentLevelIndex++;
        currentLevel = levels[currentLevelIndex];

        gameEventsManager.InvokeLevelStarted(currentLevel);
        StartLevel(currentLevel);
    }

    private void StartLevel(Level lvl)
    {
        Debug.Log("Starting level: " + currentLevel.level);

        currentTaskIndex = -1;

        timerCoroutine = StartCoroutine(StartTimer());
        LoadNextTask();
    }

    private void LoadNextTask()
    {
        if (!PrepareNextTask())
        {
            LevelComplete();
            return;
        }

        Debug.Log("Started task: " + currentTask.conditionMessage);
        Debug.Log("Interaction Required: " + currentTask.interactionToBeDone);
        gameEventsManager.InvokeLevelTaskStarted(currentTask);
        // wait for completion
    }

    private void CurrentTaskCompleted()
    {
        gameEventsManager.InvokeLevelTaskCompleted(currentTask);

        Debug.Log("Completed task: " + currentTask.conditionMessage);
        LoadNextTask();
    }

    public IEnumerator StartTimer()
    {
        gameState.timer.Value = currentLevel.time;

        while (gameState.timer.Value >= 0)
        {
           // Debug.Log(gameState.timer);

            yield return new WaitForSeconds(1f);
            gameState.timer.Value--;
        }

        LevelFailed();
    }

    private void LevelFailed()
    {
        isGameRunning = false;
        gameEventsManager.InvokeLevelFailed(currentLevel);
        Debug.Log("Level failed !");
    }

    private void LevelComplete()
    {
        gameEventsManager.InvokeLevelCompleted(currentLevel);

        StopCoroutine(timerCoroutine);

        Debug.Log("Level completed !");

        LoadNextLevel();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public bool PrepareNextTask()
    {
        currentTaskIndex++;
        if (currentTaskIndex < currentLevel.tasks.Length)
        {
            currentTask = currentLevel.tasks[currentTaskIndex];
            return true;
        }
        else
        {
            return false;
        }
    }
}
