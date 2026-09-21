using System;

public interface IInventoryStorage
{
    public int SlotCount { get; }

    bool TryAcquireItem(Item item);

    /// <summary> 인벤토리에 특정 아이템이 need 이상 있는지 확인(기본값 1) </summary>
    bool HasItem(ItemSO item, int need);

    /// <summary> 인벤토리에서 필요한 개수만큼 아이템 차감 </summary>
    bool ConsumeItem(ItemSO item, int need);

    /// <summary> 인덱스에 있는 아이템을 개수만큼 차감 </summary>
    bool ConsumeItemAt(int index, int need);

    /// <summary> 아이템 보유량 반환 </summary>
    int GetOwned(ItemSO item);

    bool IsEmptySlot(int index);

    /// <summary> 아이템 정보와 보유량을 반환 </summary>
    Item GetSlotItem(int index);

    event Action<int> OnSlotChanged;
}
