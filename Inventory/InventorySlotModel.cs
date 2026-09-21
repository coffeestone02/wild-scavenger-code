using System;

public class InventorySlotModel
{
    public Item SlotItem { get; private set; }
    public int SlotIndex { get; private set; }

    public event Action<int> OnChanged;
    public bool IsEmpty => SlotItem == null;

    public ItemSO ItemSO => SlotItem.ItemSO;
    public int ItemCount => SlotItem.ItemCount;
    public Define.EItemType ItemType => SlotItem.ItemType;

    public InventorySlotModel(int index)
    {
        SlotIndex = index;
    }

    /// <summary> 아이템 설정 </summary>
    public void SetSlot(Item item)
    {
        if (item != null && item.ItemCount <= 0) { return; }

        SlotItem = item;
        OnChanged?.Invoke(SlotIndex);
    }

    /// <summary> 개수증감 </summary>
    public void AddCount(int count)
    {
        if (IsEmpty) { return; }

        SlotItem.ItemCount += count;

        if (SlotItem.ItemCount <= 0)
        {
            Clear();
            return;
        }

        OnChanged?.Invoke(SlotIndex);
    }

    public void Clear()
    {
        SlotItem = null;

        OnChanged?.Invoke(SlotIndex);
    }
}
