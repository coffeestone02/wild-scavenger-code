using System;

public class InventoryModel : IInvenSave, IItemUser
{
    ItemDatabaseSO _itemDB;
    CursorSlot _cursor;
    InventoryStorage _storage;
    InventoryWeapon _weapon;
    InventoryArmor _armor;

    public ICursorSlot Cursor => _cursor;
    public IInventoryStorage Storage => _storage;
    public IInventoryWeapon Weapon => _weapon;
    public IInventoryArmor Armor => _armor;

    public event Action<Item> OnItemDropped;

    public InventoryModel(int slotCount, int armorCount, ItemDatabaseSO db, ICombat playerStat)
    {
        // 인벤토리 슬롯
        _storage = new InventoryStorage(slotCount);

        // 무기
        _weapon = new InventoryWeapon(_storage, playerStat);

        // 장비 슬롯
        _armor = new InventoryArmor(armorCount, playerStat);

        // 커서
        _cursor = new CursorSlot();

        // 아이템 획득 실패 시 드랍
        _storage.OnItemAcquiredFailed += HandleAcquireFailed;

        // 아이템 데이터베이스
        _itemDB = db;
    }

    public void Dispose()
    {
        _storage.OnItemAcquiredFailed -= HandleAcquireFailed;

        _weapon.Dispose();
        _armor.Dispose();
        _storage.Dispose();
    }

    void HandleAcquireFailed(Item item)
    {
        OnItemDropped?.Invoke(item);
    }

    #region Slot Control

    /// <summary> 인벤 슬롯 좌클릭. 집기, 놓기, 합치기, 교환 동작 </summary>
    public bool ClickSlot(int index)
    {
        if (index < 0 || index >= _storage.SlotCount) { return false; }

        Item slotItem = _storage.GetSlotItem(index);

        // 빈손이면 슬롯의 아이템을 집는다
        if (_cursor.IsEmpty)
        {
            if (slotItem == null) { return false; }

            _cursor.Hold(slotItem);
            _storage.ClearSlot(index);
            return true;
        }

        // 같은 아이템이면 합치기(장비 제외)
        Item held = _cursor.CursorItem;
        if (slotItem != null && held.ItemType != Define.EItemType.Equipment && held.ItemSO == slotItem.ItemSO)
        {
            _storage.AddCountAt(index, held.ItemCount);
            _cursor.Clear();
            return true;
        }

        // 빈 슬롯이면 놓기, 아니면 교환
        _cursor.Hold(_storage.PlaceItem(index, held));
        return true;
    }

    /// <summary> 인벤 슬롯 우클릭. 아이템을 절반 나눠서 들음(장비는 착용 및 해제) </summary>
    public bool RightClickSlot(int index)
    {
        if (index < 0 || index >= _storage.SlotCount) { return false; }

        Item slotItem = _storage.GetSlotItem(index);
        if (slotItem == null) { return false; }

        if (slotItem.ItemType == Define.EItemType.Equipment)
        {
            UseItem(index);
            return true;
        }

        // 이미 뭔가를 들고 있으면 절반 집기는 하지 않음
        if (_cursor.IsEmpty == false) { return false; }

        int half = slotItem.ItemCount / 2 + slotItem.ItemCount % 2;
        _cursor.Hold(new Item(slotItem.ItemSO, half));
        _storage.ConsumeItemAt(index, half);
        return true;
    }

    /// <summary> 방어구 슬롯 클릭. 착용 및 해제 </summary>
    public bool ClickArmorSlot(int index)
    {
        if (index < 0 || index >= _armor.SlotCount) { return false; }

        // 빈손으로 착용중인 방어구를 집음
        if (_cursor.IsEmpty)
        {
            Item slotItem = _armor.GetSlotItem(index);
            if (slotItem == null) { return false; }

            _cursor.Hold(slotItem);
            _armor.UnequipArmor(index);
            return true;
        }

        // 들고 있는 아이템이 방어구가 아니면 무시
        Item held = _cursor.CursorItem;
        if (held.ItemType != Define.EItemType.Equipment) { return false; }

        EquipmentStatSO eq = held.GetEquipmentStatSO();
        if (eq == null || eq.EquipmentType != Define.EEquipmentType.Armor) { return false; }

        _cursor.Hold(_armor.PlaceArmor(index, held));
        return true;
    }

    public bool RightClickArmorSlot(int index)
    {
        if (index < 0 || index >= _armor.SlotCount) { return false; }

        Item slotItem = _armor.GetSlotItem(index);
        if (slotItem == null) { return false; }

        _armor.UnequipArmor(index);
        _storage.TryAcquireItem(slotItem);
        return true;
    }

    /// <summary> 들고 있는 아이템을 월드로 반환 </summary>
    public void DropHeldItem()
    {
        if (_cursor.IsEmpty) { return; }

        OnItemDropped?.Invoke(_cursor.Take());
    }

    /// <summary> 인덱스에 있는 아이템 사용 </summary>
    public void UseItem(int index)
    {
        if (index < 0 || index >= _storage.SlotCount || _storage.GetSlotItem(index) == null) { return; }

        Item slotItem = _storage.GetSlotItem(index);

        // 장비일 경우만 실행
        EquipmentStatSO eq = slotItem.GetEquipmentStatSO();
        if (eq != null)
        {
            if (eq.EquipmentType == Define.EEquipmentType.Armor)
            {
                // 빈공간 찾아서 넣기
                int armorIdx = _armor.EmptyArmorSlotIndex();
                if (armorIdx != -1)
                {
                    _armor.EquipArmor(slotItem, armorIdx);
                    _storage.ClearSlot(index);
                }
            }
            else if (eq.EquipmentType == Define.EEquipmentType.Weapon)
            {
                // 착용중인 장비를 다시 우클릭하면 장착 해제
                if (_weapon.UsingIndex == index)
                {
                    _weapon.Unequip();
                }
                else
                {
                    _weapon.Equip(index);
                }
            }
        }
    }

    #endregion

    public InventoryData GetSaveData()
    {
        return InventorySaveHandler.CreateData(_storage.GetSlots(), _armor.GetSlots());
    }

    public void ApplySaveData(InventoryData data)
    {
        InventorySaveHandler.Restore(data, _storage.GetSlots(), _armor.GetSlots(), _itemDB);
    }
}
