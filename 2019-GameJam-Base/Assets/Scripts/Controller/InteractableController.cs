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

    public InteractableEvent OnInteractionBegin;

    
    private float currentInteractionTime;
    
    private bool isInteracting;

    private bool canInteract;

	
    public float InteractionCompleteness => currentInteractionTime / InteractionTime;
     
    private float currentInteractionCooldown ;

	public float CooldownAbsolute => InteractionCooldown - currentInteractionCooldown;
	public float CooldownNormalized => 1-(currentInteractionCooldown/InteractionCooldown);

    public void BeginInteraction()
    {
        if(!isInteracting && canInteract)
        {
            isInteracting = true;
            OnInteractionBegin.Invoke(this);
        }
    }

	private void CooldownInteraction()
	{
		canInteract = true;
		currentInteractionCooldown = 0;
		OnInteractionCooldownReset.Invoke();
	}

    private void CompleteInteraction()
    { 
        isInteracting = false;
        currentInteractionTime = 0;
        canInteract = false;
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
        canInteract = true;
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
