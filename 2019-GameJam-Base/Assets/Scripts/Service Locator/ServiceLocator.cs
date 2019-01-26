using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator instance;

    public GuiManager guiManager;
    public GameManager gameManager;

    private Kernel kernel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }

        kernel = new Kernel();

        kernel.Add<GameState>(new GameState());
        kernel.Add<GameEventsManager>(new GameEventsManager());

        kernel.Add<GuiManager>(guiManager);
        kernel.Add<GameManager>(Instantiate(gameManager));

        CharacterController characterController = FindObjectOfType<CharacterController>();
        kernel.Add<CharacterController>(characterController);
    }

    private void Start()
    {
        var initializables = kernel.GetInstancesOfType<IInitiatable>();

        for (int i = 0; i < initializables.Count; i++)
        {
            initializables[i].Initiate();
        }

        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();

        var initializables = kernel.GetInstancesOfType<ILateInitiatable>();

        for (int i = 0; i < initializables.Count; i++)
        {
            initializables[i].LateInitiate();
        }
    }

    public T GetInstanceOfType<T>()
        where T : class
    {
        return kernel.GetInstanceOfType<T>();
    }

    public List<T> GetInstancesOfType<T>()
        where T : class
    {
        return kernel.GetInstancesOfType<T>();
    }
}

public interface IInitiatable
{
    void Initiate();
}

public interface ILateInitiatable
{
    void LateInitiate();
}
