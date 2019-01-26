using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InteractableEvent : UnityEvent<InteractableController> {  }

public class InteractableController : MonoBehaviour
{
    public float InteractionTime = 1.5f;

	public bool IsRepeatable;

	public float InteractionCooldown;

	// public Interactable InteractionType;

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
        isInteracting = false;
        currentInteractionTime = 0;
        CanInteract = false;
        currentInteractionCooldown = InteractionCooldown;
        OnInteractionComplete.Invoke();
    }

    public void EndInteraction()
    {
        isInteracting = false;
        OnInteractionEnd.Invoke();
    }

    public void Start()
    {
        CanInteract = true;
        isInteracting= false;
        currentInteractionTime = 0;
        currentInteractionCooldown = 0;
    }

	public void Update()
	{
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
