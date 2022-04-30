using UnityEngine;

namespace Interactables
{
    /**
     * This class is useful for the hamster to access the tube, jump pad and nuts.
     */
    public interface IInteractable
    {
        public bool IsInteractable(GameObject source);

        public bool TryInteract(GameObject source);

        public void OnInteractionFinished(GameObject source);
    }
}