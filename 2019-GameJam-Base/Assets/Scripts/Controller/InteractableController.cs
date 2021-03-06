using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniRx;

[System.Serializable]
public class InteractableEvent : UnityEvent<InteractableController> {  }

public class InteractableController : MonoBehaviour
{
    public Image imgArrow;

    public float InteractionTime = 1.5f;

	public bool IsRepeatable;

    public MirzaBeig.ParticleSystems.ParticleSystems interactionParticles;
    public AudioSource interactionSound;

	public float InteractionCooldown;

	public Interactable InteractionType;

	public UnityEvent OnInteractionComplete;

    public UnityEvent OnInteractionEnd;

    public UnityEvent OnInteractionCooldownReset;

	public UnityEvent OnClosestMarked;

	public UnityEvent OnClosestUnmarked;

    public InteractableEvent OnInteractionBegin;
    
    private float currentInteractionTime;
    
    private bool isInteracting;

    public bool CanInteract;

	
    public float InteractionCompleteness => currentInteractionTime / InteractionTime;
     
    private float currentInteractionCooldown ;

	public float CooldownAbsolute => InteractionCooldown - currentInteractionCooldown;
	public float CooldownNormalized => 1-(currentInteractionCooldown/InteractionCooldown);

    private GameEventsManager gameEventsManager;
    private WordMiniGame wordMiniGame;
    private GameState gameState;

    public bool RequiresMiniGame;

	public void MarkClosest()
	{
		OnClosestMarked.Invoke();
	}

	public void UnmarkClosest()
	{
		OnClosestUnmarked.Invoke();
	}

    public void BeginInteraction()
    {
        if(!isInteracting && CanInteract)
        {
            if (interactionSound != null)
            {
                interactionSound.Play();
            }

            if (interactionParticles != null)
            {
                interactionParticles.gameObject.SetActive(true);
                interactionParticles.play();
            }

            isInteracting = true;
            OnInteractionBegin.Invoke(this);
        }
    }

	private void CooldownInteraction()
	{
		CanInteract = true;
		currentInteractionCooldown = 0;
		OnInteractionCooldownReset.Invoke();
	}

    private void CompleteInteraction()
    {
        if (interactionSound != null)
        {
            interactionSound.Stop();
        }

        if (interactionParticles != null)
        {
            interactionParticles.stop();
            interactionParticles.gameObject.SetActive(false);
        }

        isInteracting = false;
        currentInteractionTime = 0;
        CanInteract = false;
        currentInteractionCooldown = InteractionCooldown;
        OnInteractionComplete.Invoke();

        Debug.Log("iNTERACTION COMPLETE");
        if (RequiresMiniGame && !WordMiniGame.GameIsRunning)
        {
            wordMiniGame.StartMinigame(
                () => 
                {
                    gameEventsManager.InvokeInteraction(InteractionType);
                    gameState.playerSpeed.Value += ServiceLocator.instance.miniGameSpeedGiven;
                }, 
                () => 
                {
                    gameState.timer.Value -= ServiceLocator.instance.miniGameFailTakenTime;
                });
        }
        else
        {
            gameEventsManager.InvokeInteraction(InteractionType);
        }
    }

    public void EndInteraction()
    {
        if (interactionSound != null)
        {
            interactionSound.Stop();
        }

        if (interactionParticles != null)
        {
            interactionParticles.stop();
            interactionParticles.gameObject.SetActive(false);
        }

        isInteracting = false;
        OnInteractionEnd.Invoke();
    }

    public void Start()
    {
        gameEventsManager = ServiceLocator.instance.GetInstanceOfType<GameEventsManager>();
        wordMiniGame = ServiceLocator.instance.GetInstanceOfType<WordMiniGame>();
        gameState = ServiceLocator.instance.GetInstanceOfType<GameState>();

        gameEventsManager.ObserveLevelTaskStarted().Subscribe(t =>
        {
            if (InteractionType == t.interactionToBeDone)
            {
                imgArrow.gameObject.SetActive(true);
            }
        }).AddTo(this);

        gameEventsManager.ObserveLevelTaskCompleted().Subscribe(t =>
        {
            if (InteractionType == t.interactionToBeDone)
            {
                imgArrow.gameObject.SetActive(false);
            }
        }).AddTo(this);

        CanInteract = true;
        isInteracting= false;
        currentInteractionTime = 0;
        currentInteractionCooldown = 0;
    }

	public void Update()
	{
//	    if (Input.GetKeyUp(KeyCode.F10))
//	    {
//
//	        wordMiniGame.StartMinigame(
//	            () =>
//	            {
//	                gameEventsManager.InvokeInteraction(InteractionType);
//	                gameState.playerSpeed.Value += ServiceLocator.instance.miniGameSpeedGiven;
//	            },
//	            () =>
//	            {
//	                gameState.timer.Value -= ServiceLocator.instance.miniGameFailTakenTime;
//	            });
//        }
		if (isInteracting && currentInteractionCooldown == 0f)
		{
			currentInteractionTime += Time.deltaTime;
			if (currentInteractionTime >= InteractionTime)
			{
				CompleteInteraction();
			}

			//    OnInteractionProgress.Invoke(InteractionCompleteness);
		}else if (!isInteracting && currentInteractionTime < InteractionTime)
		{
			currentInteractionTime = 0;
		}


		if (IsRepeatable)
		{
			if (currentInteractionCooldown > 0)
			{
				currentInteractionCooldown -= Time.deltaTime;
				currentInteractionTime = 0;
				if (currentInteractionCooldown <= 0)
				{
					CooldownInteraction();
				}
			}
		}
	}
}
