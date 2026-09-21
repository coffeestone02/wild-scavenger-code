using System;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField] InteractionDetector _detector;

    IInventoryStorage _inventory;
    public event Action<Define.EInteractionType> OnInteract;

    public void Init(IInventoryStorage inventory)
    {
        _inventory = inventory;
    }

    void Update()
    {
        InteractAction();
    }

    void InteractAction()
    {
        if (Managers.Input.InteractPressed == false || _detector.InteractableTarget == null || Managers.UI.IsSceneUIOpen) { return; }

        IInteractable target = _detector.InteractableTarget;
        if (target == null || (target is UnityEngine.Object obj && obj == null)) { return; }

        InteractionResult interactionResult = target.Interact();
        switch (interactionResult.Type)
        {
            case Define.EInteractionType.Item:
                _inventory.TryAcquireItem(interactionResult.Item);
                break;
            case Define.EInteractionType.CraftingStation:
                Managers.UI.OpenSceneUI<CraftingPresenter>();
                break;
            case Define.EInteractionType.SellingPortal:
                Managers.UI.OpenSceneUI<SellingPortalPresenter>();
                break;
            case Define.EInteractionType.StorePortal:
                Managers.UI.OpenSceneUI<StorePresenter>();
                break;
            case Define.EInteractionType.EntryPortal:
                CustomSceneManager.Instance.LoadScene("Forest");
                break;
            case Define.EInteractionType.ReturnPortal:
                CustomSceneManager.Instance.LoadScene("PlayerHouse");
                break;
        }

        OnInteract?.Invoke(interactionResult.Type);
    }

}
