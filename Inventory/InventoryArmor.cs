using System;

// 방어구 슬롯의 보관, 착용, 해제
public class InventoryArmor : IInventoryArmor
{
    InventorySlotModel[] _slots;

    public int SlotCount { get; }

    public event Action<int> OnArmorEquipped;
    public event Action<int> OnArmorUnequipped;
    public event Action<int> OnArmorSlotChanged;

    ICombat _playerDefense;

    public InventoryArmor(int armorCount, ICombat playerDefense)
    {
        SlotCount = armorCount;
        _slots = new InventorySlotModel[armorCount];
        for (int i = 0; i < armorCount; i++)
        {
            _slots[i] = new InventorySlotModel(i);
            _slots[i].OnChanged += HandleArmorSlotChanged; // 슬롯 변동 이벤트 구독
        }

        _playerDefense = playerDefense;
    }

    void HandleArmorSlotChanged(int slotIndex)
    {
        _playerDefense.SetDefenseStat(_slots);
        OnArmorSlotChanged?.Invoke(slotIndex);
    }

    /// <summary> 가장 먼저 발견한 비어있는 방어구 슬롯 인덱스를 반환 </summary>
    public int EmptyArmorSlotIndex()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].IsEmpty)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary> n번 슬롯에 도구 착용 </summary>
    public void EquipArmor(Item armor, int armorIndex)
    {
        if (armorIndex < 0 || armorIndex >= _slots.Length) { return; }

        UnequipArmor(armorIndex);

        _slots[armorIndex].SetSlot(armor);
        OnArmorEquipped?.Invoke(armorIndex);
    }

    /// <summary> n번 슬롯 방어구 해제 </summary>
    public void UnequipArmor(int armorIndex)
    {
        if (armorIndex < 0 || armorIndex >= _slots.Length || _slots[armorIndex].IsEmpty) { return; }

        _slots[armorIndex].Clear();
        OnArmorUnequipped?.Invoke(armorIndex);
    }

    /// <summary> index 슬롯에 아이템을 놓고 원래 있던 아이템을 반환(빈 칸은 null 반환) </summary>
    public Item PlaceArmor(int armorIndex, Item armor)
    {
        if (armorIndex < 0 || armorIndex >= _slots.Length) { return armor; }

        Item old = _slots[armorIndex].SlotItem;
        EquipArmor(armor, armorIndex);
        return old;
    }

    public Item GetSlotItem(int index)
    {
        if (index < 0 || index >= _slots.Length || _slots[index].IsEmpty) { return null; }

        return _slots[index].SlotItem;
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
            _slots[i].OnChanged -= HandleArmorSlotChanged;
        }
    }
}
