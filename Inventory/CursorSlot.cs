using System;

public class CursorSlot : ICursorSlot
{
    public Item CursorItem { get; private set; }
    public bool IsEmpty => CursorItem == null;
    public event Action OnChanged;

    /// <summary> 아이템을 들게 함. null이거나 0개 이하면 비워짐 </summary>
    public void Hold(Item item)
    {
        if (item != null && item.ItemCount <= 0)
        {
            item = null;
        }

        CursorItem = item;
        OnChanged?.Invoke();
    }

    /// <summary> 들고 있던 아이템을 꺼내면서 커서를 비움 </summary>
    public Item Take()
    {
        if (CursorItem == null) { return null; }

        Item item = CursorItem;
        CursorItem = null;
        OnChanged?.Invoke();

        return item;
    }

    public void Clear()
    {
        if (CursorItem == null) { return; }

        CursorItem = null;
        OnChanged?.Invoke();
    }
}
