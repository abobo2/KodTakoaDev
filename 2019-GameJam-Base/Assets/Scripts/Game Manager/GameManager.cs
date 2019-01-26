using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class GameManager : MonoBehaviour, IInitiatable, ILateInitiatable
{
    private static int EnergyThatTvGives = 25;
    private static float PlayerPlayerStartingSpeed = 0.3f;

    public Level[] levels;

    private int currentLevelIndex;
    private int currentTaskClusterIndex;

    private List<LevelTask> currentTasksToDo;

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

            HandleInteraction(interaction);
        });

        gameState.energy.Subscribe(en =>
        {
            if (!isGameRunning)
            {
                return;
            }

            if (en <= 0)
            {
                LevelFailed();
            }
        });
    }

    public void Start()
    {
        PrepareStartOfGame();
    }

    public void LateInitiate()
    {
        StarGame();
    }

    private void PrepareStartOfGame()
    {
        gameState.energy.Value = 100;
        gameState.playerSpeed.Value = PlayerPlayerStartingSpeed;
    }

    private IEnumerator LooseEnergyOverTime()
    {
        while (gameState.energy.Value > 0)
        {
            yield return new WaitForSeconds(1f);

            if (isGameRunning)
            {
                gameState.energy.Value -= 1;
            }
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

        gameEventsManager.InvokeLevelStarted(levels[currentLevelIndex]);
        StartLevel(levels[currentLevelIndex]);
    }

    private void StartLevel(Level lvl)
    {
        currentTaskClusterIndex = -1;

        Debug.Log("Starting level: " + lvl.level);

        timerCoroutine = StartCoroutine(StartTimer());
        LoadNextTaskCluster();
    }

    private void LoadNextTaskCluster()
    {
        currentTaskClusterIndex++;

        if (currentTaskClusterIndex >= levels[currentLevelIndex].taskClusters.Length || levels[currentLevelIndex].taskClusters.Length == 0)
        {
            LevelComplete();
            return;
        }

        currentTasksToDo = new List<LevelTask>();
        foreach (var task in levels[currentLevelIndex].taskClusters[currentTaskClusterIndex].tasks)
        {
            Debug.Log("Started task: " + task.conditionMessage);
            Debug.Log("Interaction Required: " + task.interactionToBeDone);
            gameEventsManager.InvokeLevelTaskStarted(task);

            currentTasksToDo.Add(task);
        }
    }

    private void TaskCompleted(LevelTask task)
    {
        gameEventsManager.InvokeLevelTaskCompleted(task);
        currentTasksToDo.Remove(task);

        Debug.Log("Completed task: " + task.conditionMessage);
        if (currentTasksToDo.Count <= 0)
        {
            LoadNextTaskCluster();
        }
    }

    public IEnumerator StartTimer()
    {
        gameState.timer.Value = levels[currentLevelIndex].time;

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
        gameEventsManager.InvokeLevelFailed(levels[currentLevelIndex]);
        Debug.Log("Level failed !");
    }

    private void LevelComplete()
    {
        gameEventsManager.InvokeLevelCompleted(levels[currentLevelIndex]);

        StopCoroutine(timerCoroutine);

        Debug.Log("Level completed !");

        LoadNextLevel();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    /*
    public bool PrepareNextTask()
    {
        currentTaskIndex++;
        if (currentTaskIndex < currentLevel.taskClusters.Length)
        {
          //  currentTask = currentLevel.taskClusters[currentTaskIndex];
            return true;
        }
        else
        {
            return false;
        }
    }*/

    private void HandleInteraction(Interactable interactionMade)
    {
        var taskFromToDo = currentTasksToDo.Find(e => e.interactionToBeDone == interactionMade);

        if (taskFromToDo != null)
        {
            TaskCompleted(taskFromToDo);
        }
        
        if (interactionMade == Interactable.TV)
        {
            if (gameState.energy.Value + EnergyThatTvGives > gameState.maxEnergy)
            {
                gameState.energy.Value = gameState.maxEnergy;
            }
            else
            {
                gameState.energy.Value += EnergyThatTvGives;
            }
        }
    }
}
