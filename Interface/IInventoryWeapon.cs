using System;

public interface IInventoryWeapon
{
    int UsingIndex { get; }
    event Action<EquipmentStatSO> OnEquipped;
    event Action<EquipmentStatSO> OnUnequipped;
}
