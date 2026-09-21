using UnityEngine;

public class QuickslotController : MonoBehaviour
{
    [SerializeField] InventoryGridView _quickSlotGrid;

    IInventoryStorage _inventory;
    IInventoryWeapon _weapon;
    IItemUser _itemUser;
    bool _isInitialized;
    int _quickslotCount;

    public void Init(IInventoryStorage inventory, IInventoryWeapon weapon, IItemUser itemUser, int quickslotCount)
    {
        _inventory = inventory;
        _weapon = weapon;
        _itemUser = itemUser;
        _quickslotCount = quickslotCount;

        _quickSlotGrid.Init(quickslotCount);
        _inventory.OnSlotChanged += HandleSlotChanged;
        _weapon.OnEquipped += HandleEquipped;
        _weapon.OnUnequipped += HandleUnequipped;

        _isInitialized = true;
    }

    void Update()
    {
        if (_isInitialized == false) { return; }

        QuickslotInput();
    }

    void OnDestroy()
    {
        _inventory.OnSlotChanged -= HandleSlotChanged;
        _weapon.OnEquipped -= HandleEquipped;
        _weapon.OnUnequipped -= HandleUnequipped;

        _quickSlotGrid.Dispose();
    }

    // 인벤토리 슬롯 변경됨
    void HandleSlotChanged(int index)
    {
        Item item = _inventory.GetSlotItem(index);

        _quickSlotGrid.Refresh(index, item);
    }

    // 장비 착용
    void HandleEquipped(EquipmentStatSO eq)
    {
        int index = _weapon.UsingIndex;

        if (index < 0 || index >= _quickslotCount) { return; }

        _quickSlotGrid.SetEquipped(index);
    }

    // 장비 해제
    void HandleUnequipped(EquipmentStatSO eq)
    {
        _quickSlotGrid.SetEquipped(-1);
    }

    void QuickslotInput()
    {
        if (Managers.Input.FirstQSPressed)
        {
            _itemUser.UseItem(0);
        }
        else if (Managers.Input.SecondQSPressed)
        {
            _itemUser.UseItem(1);
        }
        else if (Managers.Input.ThirdQSPressed)
        {
            _itemUser.UseItem(2);
        }
        else if (Managers.Input.FourQSPressed)
        {
            _itemUser.UseItem(3);
        }
        else if (Managers.Input.FifthQSPressed)
        {
            _itemUser.UseItem(4);
        }
    }
}
