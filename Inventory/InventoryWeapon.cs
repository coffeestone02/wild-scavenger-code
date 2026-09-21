using System;

public class InventoryWeapon : IInventoryWeapon
{
    InventoryStorage _storage;

    public int UsingIndex { get; private set; } = -1;

    public event Action<EquipmentStatSO> OnEquipped;
    public event Action<EquipmentStatSO> OnUnequipped;

    ICombat _playerStat;
    EquipmentStatSO _equipped;

    public InventoryWeapon(InventoryStorage storage, ICombat playerStat)
    {
        _storage = storage;
        _playerStat = playerStat;
        _storage.OnSlotChanged += HandleInvenSlotChanged;
    }

    void HandleInvenSlotChanged(int slotIndex)
    {
        if (slotIndex == UsingIndex)
        {
            Unequip();
        }
    }

    /// <summary> 도구 착용 </summary>
    public void Equip(int index)
    {
        Item slotItem = _storage.GetSlotItem(index);
        if (slotItem == null) { return; }

        EquipmentStatSO eq = slotItem.GetEquipmentStatSO();
        if (eq.EquipmentType != Define.EEquipmentType.Weapon) { return; }

        if (UsingIndex != -1)
        {
            Unequip();
        }

        _equipped = eq;
        _playerStat.SetAttackStat(eq);
        UsingIndex = index;
        OnEquipped?.Invoke(eq);
    }

    /// <summary> 현재 착용중인 장비 해제 </summary>
    public void Unequip()
    {
        if (UsingIndex == -1) { return; }

        EquipmentStatSO eq = _equipped;
        _equipped = null;
        UsingIndex = -1;
        _playerStat.ClearAttackStat();
        OnUnequipped?.Invoke(eq);
    }

    public void Dispose()
    {
        _storage.OnSlotChanged -= HandleInvenSlotChanged;
    }
}
