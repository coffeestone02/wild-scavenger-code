using UnityEngine;

public class ItemInteract : MonoBehaviour, IInteractable
{
    [SerializeField] Item _item;

    public Item Item => _item;
    public int Count
    {
        get
        {
            return _item.ItemCount;
        }
        set
        {
            _item.ItemCount = value;
        }
    }

    public InteractionResult Interact()
    {
        gameObject.SetActive(false);
        return new InteractionResult(Define.EInteractionType.Item, _item);
    }

    public string GetName()
    {
        return _item.ItemName;
    }
}
