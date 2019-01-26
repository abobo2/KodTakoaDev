using UnityEngine;

namespace UnityTemplateProjects.Controller
{
    public class TimedInteractable : BaseInteractable
    {
        public float TimeToInteract = .5f;

        private bool interacted;

        private float currentInteractTime = 0f;

        public void BeginPlayerInteraction()
        {
            if (CanBeInteractedWith)
            {
                interacted = true;
            }
        }

        public void Update()
        {
            if (interacted && currentInteractTime < TimeToInteract)
            {
                currentInteractTime += Time.deltaTime;
            }

            if (currentInteractTime > TimeToInteract)
            {
                // Interact Here.
                //TODO : Interact
                currentInteractTime = 0;
            }
        }

        public override void BeginInteract(GameObject other)
        {
            if (other.tag == Constants.PlayerTag)
            {
                BeginPlayerInteraction();
            }
        }

        public override void StopInteract(GameObject other)
        {
            if (CanBeInteractedWith)
            {
                interacted = false;
            }
        }

        public override void InvokeInteraction()
        {
            throw new System.NotImplementedException();
        }
    }
}