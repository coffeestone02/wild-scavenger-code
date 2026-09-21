using UnityEngine;

public class CraftingStation : MonoBehaviour, IInteractable
{
    public InteractionResult Interact()
    {
        return new InteractionResult(Define.EInteractionType.CraftingStation);
    }

    public string GetName()
    {
        return name;
    }
}
