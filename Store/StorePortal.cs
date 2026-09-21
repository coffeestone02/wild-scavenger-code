using UnityEngine;

public class StorePortal : MonoBehaviour, IInteractable
{
    public InteractionResult Interact()
    {
        return new InteractionResult(Define.EInteractionType.StorePortal);
    }

    public string GetName()
    {
        return name;
    }
}
