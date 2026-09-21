
public class InteractionResult
{
    public Define.EInteractionType Type;
    public Item Item;

    public InteractionResult(Define.EInteractionType type, Item item)
    {
        Type = type;
        Item = item;
    }

    public InteractionResult(Define.EInteractionType type)
    {
        Type = type;
    }
}
