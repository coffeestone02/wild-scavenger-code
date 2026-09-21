using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryPresenter : SceneUIBase, IPointerClickHandler
{
    [Header("Views")]
    [SerializeField] UIItemTooltip _tooltip;
    [SerializeField] InventoryClickView _clickView;
    [SerializeField] InventoryGridView _storageGrid;
    [SerializeField] InventoryGridView _armorGrid;

    [Header("Items")]
    [SerializeField] Transform _respawnPos;

    InventoryModel _model;

    public event Action OnSlotClicked;

    public void Init(InventoryModel model)
    {
        _model = model;
        _clickView.Clear();

        // 인벤토리 슬롯
        _storageGrid.Init(_model.Storage.SlotCount);
        _storageGrid.OnClicked += HandleSlotClicked;
        _storageGrid.OnRightClicked += HandleSlotRightClicked;
        _storageGrid.OnHovered += HandleItemTooltipView;

        // 장비 슬롯
        _armorGrid.Init(_model.Armor.SlotCount);
        _armorGrid.OnClicked += HandleArmorSlotClicked;
        _armorGrid.OnRightClicked += HandleArmorSlotRightClicked;

        // 모델 이벤트
        _model.Storage.OnSlotChanged += HandleSlotChanged;
        _model.Armor.OnArmorSlotChanged += HandleArmorSlotChanged;
        _model.Cursor.OnChanged += HandleCursorChanged;
        _model.Weapon.OnEquipped += HandleEquipped;
        _model.Weapon.OnUnequipped += HandleUnequipped;
        _model.OnItemDropped += HandleItemDropped;
    }

    #region Unity Function

    void OnDisable()
    {
        if (_model != null)
        {
            _model.DropHeldItem(); // 아이템을 든 상태로 인벤토리 창을 닫으면 월드로 반환
        }
        if (_tooltip != null)
        {
            _tooltip.HideView();
        }
        if (_clickView != null)
        {
            _clickView.Clear();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        // 인벤토리 슬롯
        _storageGrid.OnClicked -= HandleSlotClicked;
        _storageGrid.OnRightClicked -= HandleSlotRightClicked;
        _storageGrid.OnHovered -= HandleItemTooltipView;
        _storageGrid.Dispose();

        // 장비 슬롯
        _armorGrid.OnClicked -= HandleArmorSlotClicked;
        _armorGrid.OnRightClicked -= HandleArmorSlotRightClicked;
        _armorGrid.Dispose();

        if (_model != null)
        {
            _model.Storage.OnSlotChanged -= HandleSlotChanged;
            _model.Armor.OnArmorSlotChanged -= HandleArmorSlotChanged;
            _model.Cursor.OnChanged -= HandleCursorChanged;
            _model.Weapon.OnEquipped -= HandleEquipped;
            _model.Weapon.OnUnequipped -= HandleUnequipped;
            _model.OnItemDropped -= HandleItemDropped;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _model.DropHeldItem();
    }

    #endregion

    #region Slot Control 

    // 인벤 슬롯 좌클릭
    void HandleSlotClicked(int index)
    {
        if (_model.ClickSlot(index))
        {
            OnSlotClicked?.Invoke();
        }
    }

    // 인벤 슬롯 우클릭
    void HandleSlotRightClicked(int index)
    {
        if (_model.RightClickSlot(index))
        {
            OnSlotClicked?.Invoke();
        }
    }

    // 방어구 슬롯 좌클릭
    void HandleArmorSlotClicked(int index)
    {
        if (_model.ClickArmorSlot(index))
        {
            OnSlotClicked?.Invoke();
        }
    }

    // 방어구 슬롯 우클릭
    void HandleArmorSlotRightClicked(int index)
    {
        if (_model.RightClickArmorSlot(index))
        {
            OnSlotClicked?.Invoke();
        }
    }

    // 슬롯 변경됨
    void HandleSlotChanged(int index)
    {
        Item slotItem = _model.Storage.GetSlotItem(index);
        _storageGrid.Refresh(index, slotItem);
    }

    // 갑옷 슬롯 변경됨
    void HandleArmorSlotChanged(int index)
    {
        Item slotItem = _model.Armor.GetSlotItem(index);
        _armorGrid.Refresh(index, slotItem);
    }

    #endregion

    // 커서 아이템이 바뀜
    void HandleCursorChanged()
    {
        Item held = _model.Cursor.CursorItem;

        if (held == null)
        {
            _clickView.Clear();
        }
        else
        {
            _clickView.Selected(held.ItemSO, held.ItemCount);
        }
    }

    void HandleItemTooltipView(int index)
    {
        Item slotItem = _model.Storage.GetSlotItem(index);
        if (index == -1 || slotItem == null)
        {
            _tooltip.HideView();
        }
        else
        {
            _tooltip.ShowView(_storageGrid.GetPosition(index), slotItem);
        }
    }

    void HandleEquipped(EquipmentStatSO eq)
    {
        int index = _model.Weapon.UsingIndex;
        _storageGrid.SetEquipped(index);
    }

    void HandleUnequipped(EquipmentStatSO eq)
    {
        _storageGrid.SetEquipped(-1);
    }

    void HandleItemDropped(Item item)
    {
        if (item == null) { return; }

        Util.ItemSpawn(item, _respawnPos.position);
    }
}
