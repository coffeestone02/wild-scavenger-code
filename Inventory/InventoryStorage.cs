using System;

public class InventoryStorage : IInventoryStorage
{
    InventorySlotModel[] _slots;

    public int SlotCount { get; }

    public event Action<int> OnSlotChanged;
    public event Action<Item> OnItemAcquiredFailed;

    public InventoryStorage(int slotCount)
    {
        SlotCount = slotCount;
        _slots = new InventorySlotModel[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            _slots[i] = new InventorySlotModel(i);
            _slots[i].OnChanged += HandleSlotChanged;
        }
    }

    void HandleSlotChanged(int slotIndex)
    {
        OnSlotChanged?.Invoke(slotIndex);
    }

    public bool TryAcquireItem(Item item)
    {
        if (item == null || item.ItemCount <= 0) { return false; }

        if (Define.EItemType.Equipment != item.ItemType) // (장비제외)같은 아이템이 있는지 확인
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                // 같은 종류 아이템이면 해당 슬롯에 추가
                if (_slots[i].IsEmpty == false && _slots[i].SlotItem.ItemSO == item.ItemSO)
                {
                    _slots[i].AddCount(item.ItemCount);
                    return true;
                }
            }
        }

        for (int i = 0; i < _slots.Length; i++) // 빈 슬롯 찾아서 추가하기
        {
            if (_slots[i].IsEmpty)
            {
                _slots[i].SetSlot(item);
                return true;
            }
        }

        // 인벤토리가 가득차면 플레이어 위치에 드랍
        OnItemAcquiredFailed?.Invoke(item);
        return false;
    }

    public bool HasItem(ItemSO item, int need = 1)
    {
        if (item == null) { return false; }

        int owned = GetOwned(item);
        if (owned >= need) { return true; }

        return false;
    }

    public bool ConsumeItem(ItemSO item, int need)
    {
        // item이 null이거나 보유량이 충분하지 않으면 false 리턴
        if (item == null || HasItem(item, need) == false) { return false; }

        int count = need;
        for (int i = 0; i < _slots.Length && count > 0; i++)
        {
            if (_slots[i].IsEmpty || _slots[i].ItemSO != item) { continue; }

            int take = Math.Min(count, _slots[i].ItemCount);
            _slots[i].AddCount(-take);
            count -= take;
        }

        return true;
    }

    public bool ConsumeItemAt(int index, int need)
    {
        if (index < 0 || index >= _slots.Length) { return false; }
        if (_slots[index].IsEmpty || _slots[index].ItemCount < need) { return false; }

        _slots[index].AddCount(-need);
        return true;
    }

    public int GetOwned(ItemSO item)
    {
        if (item == null) { return 0; }

        int total = 0;
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].IsEmpty == false && _slots[i].ItemSO == item)
            {
                total += _slots[i].ItemCount;
            }
        }

        return total;
    }

    public bool IsEmptySlot(int index)
    {
        if (index < 0 || index >= _slots.Length) { return true; }

        return _slots[index].IsEmpty;
    }

    public Item GetSlotItem(int index)
    {
        if (index < 0 || index >= _slots.Length || _slots[index].IsEmpty) { return null; }

        return _slots[index].SlotItem;
    }

    /// <summary> index 슬롯에 아이템을 놓고 원래 있던 아이템을 반환(빈 칸은 null 반환) </summary>
    public Item PlaceItem(int index, Item newItem)
    {
        if (index < 0 || index >= _slots.Length) { return newItem; }

        Item old = _slots[index].SlotItem;
        _slots[index].SetSlot(newItem);
        return old;
    }

    public void ClearSlot(int index)
    {
        if (index < 0 || index >= _slots.Length) { return; }

        _slots[index].Clear();
    }

    public void AddCountAt(int index, int count)
    {
        if (index < 0 || index >= _slots.Length || _slots[index].IsEmpty) { return; }

        _slots[index].AddCount(count);
    }

    // 세이브용
    public InventorySlotModel[] GetSlots()
    {
        return _slots;
    }

    public void Dispose()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].OnChanged -= HandleSlotChanged;
        }
    }
}
