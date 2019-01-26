using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterController : MonoBehaviour, IInitiatable
{
    public Quaternion targetRotation;

    public Vector2 playerInput;

  //  public float MoveSpeed;

    public float RotationSpeed;

    public bool Interact;

	private bool isInteracting = false;

	private List<InteractableController> Interactables;

	private InteractableController currentInteractable;
	private InteractableController closestInteractable;

    private GameState gameState; 

    public void Initiate()
    {
        gameState = ServiceLocator.instance.GetInstanceOfType<GameState>();
    }

    // Start is called before the first frame update
    void Start()
    {
		Interactables = new List<InteractableController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetInput();
        Rotate();
		Move();
	    HandleInteraction();
    }

	public void OnTriggerEnter(Collider other)
	{
//		Debug.Log(other.gameObject.tag);
		if (other.gameObject.tag == Constants.InteractableTag)
		{
			var ic = other.GetComponent<InteractableController>();
			if (!Interactables.Contains(ic) && ic != null)
			{
//				Debug.Log("Add Interactable");
				Interactables.Add(ic);
			}
		}
	}

	public void OnTriggerExit(Collider other)
	{
//		Debug.Log(other.gameObject.tag);

		if (other.gameObject.tag == Constants.InteractableTag)
		{
			var ic = other.GetComponent<InteractableController>();
			if (Interactables.Contains(ic))
			{
//				Debug.Log("Remove Interactable");
				Interactables.Remove(ic);
			}
		}
	}

	void HandleInteraction()
	{
		InteractableController closest = null;
		if (Interactables.Count > 0)
		{
			if (Interactables.Any(i => i.CanInteract))
			{
				closest = Interactables.OrderBy(i => Vector3.Distance(i.transform.position, transform.position)).First(i => i.CanInteract);
			}
			if (closest != closestInteractable && closest != null)
			{
				if (closestInteractable != null)
				{
					closestInteractable.UnmarkClosest();
				}
				Debug.Log("Marking interactable");
				closestInteractable = closest;
				closestInteractable.MarkClosest();
			}
		}

		if (closest == null )
		{
			if (closestInteractable != null)
			{
				if (closestInteractable.CanInteract)
				{
					closestInteractable.UnmarkClosest();
				}
				closestInteractable = null;
			}
		}

		if (Interact)
		{
			if (Interactables.Count > 0 && closest != null)
			{
				if (closest != currentInteractable && currentInteractable != null)
				{
					currentInteractable.EndInteraction();
					isInteracting = false;
				}else if (currentInteractable == null)
				{
//					Debug.Log("interacting");
					currentInteractable = closest;
					currentInteractable.BeginInteraction();
					isInteracting = true;
				}

			}
			else if ( currentInteractable != null )
			{
				currentInteractable.EndInteraction();
				currentInteractable = null;
				isInteracting = false;
			}
		}
		else if (isInteracting)
		{
			if (currentInteractable != null)
			{
				currentInteractable.EndInteraction();
				currentInteractable = null;
				isInteracting = false;
			}
		}
	}

    void GetInput()
    {
        playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Interact = Input.GetButton("Interact");
    }

    void Rotate()
    { 
        if (playerInput.magnitude > 0.2f)
        {
            Quaternion targetRot = Quaternion.LookRotation(Constants.InputToMotionFunction(playerInput));
            transform.rotation = (Quaternion.RotateTowards(transform.rotation, targetRot, RotationSpeed) );
        }
    }

    void Move()
    {
        if (playerInput.magnitude > 0.2f)
        {
            transform.GetComponent<Rigidbody>().velocity = transform.forward * gameState.playerSpeed.Value * playerInput.magnitude;
        }
    }
}
