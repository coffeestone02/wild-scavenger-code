using System;

public interface IInventoryArmor
{
    public int SlotCount { get; }
    event Action<int> OnArmorEquipped;
    event Action<int> OnArmorUnequipped;
    event Action<int> OnArmorSlotChanged;
    Item GetSlotItem(int index);
}
