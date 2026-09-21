using System;
using UnityEngine;

public class SellingPortalPresenter : SceneUIBase
{
    [SerializeField] SellingView _sellingView;
    [SerializeField] UIItemTooltip _tooltip;
    [SerializeField] InventoryGridView _invenGrid;

    SellingModel _model;
    IWallet _playerWallet;
    IInventoryStorage _inventory;
    bool _isInitialized;

    public event Action OnSold;
    public event Action OnSlotClicked;

    public void Init(SellingModel model, IWallet playerWallet, IInventoryStorage inventory)
    {
        _model = model;
        _playerWallet = playerWallet;
        _inventory = inventory;

        // 인벤토리 슬롯뷰
        _invenGrid.Init(_inventory.SlotCount);
        _invenGrid.OnClicked += HandleInvenSlotClicked;
        _invenGrid.OnHovered += HandleInvenTooltipView;

        // 판매 버튼
        _sellingView.OnSellClicked += HandleSellClicked;

        // 모델 변경
        _model.OnChanged += HandleModelChanged;

        // 인벤토리 슬롯 변동
        _inventory.OnSlotChanged += HandleInvenSlotChanged;

        _isInitialized = true;
        if (isActiveAndEnabled) // Init 전에 이미 OnEnable를 실행했다면 RefeshInventoryView 다시 실행
        {
            for (int i = 0; i < _inventory.SlotCount; i++)
            {
                _invenGrid.Refresh(i, _inventory.GetSlotItem(i));
            }
            _sellingView.SetMoneyText(_playerWallet.Gold);
        }
    }

    void OnEnable()
    {
        if (_isInitialized == false) { return; }

        for (int i = 0; i < _inventory.SlotCount; i++)
        {
            _invenGrid.Refresh(i, _inventory.GetSlotItem(i));
        }
        _sellingView.SetMoneyText(_playerWallet.Gold);
    }

    void OnDisable()
    {
        _tooltip.HideView();
        _model.ClearItems();
        _invenGrid.ClearAllSelected();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _invenGrid.OnClicked -= HandleInvenSlotClicked;
        _invenGrid.OnHovered -= HandleInvenTooltipView;
        _invenGrid.Dispose();

        _sellingView.OnSellClicked -= HandleSellClicked;

        _model.OnChanged -= HandleModelChanged;
        _inventory.OnSlotChanged -= HandleInvenSlotChanged;
    }

    // 판매 목록에 추가/삭제
    void HandleInvenSlotClicked(int index)
    {
        bool result = _model.UpdateItem(index);
        _invenGrid.SetSelected(index, result);

        OnSlotClicked?.Invoke();
    }

    // 판매
    void HandleSellClicked()
    {
        int profit = _model.SellItems();
        if (profit > 0)
        {
            OnSold?.Invoke();
            _sellingView.SetMoneyText(_playerWallet.Gold);
        }
    }

    // 판매 슬롯, 골드를 갱신함
    void HandleModelChanged()
    {
        _sellingView.PrepareSellingSlot(_model.Cart.Count);

        int i = 0;
        foreach (var elem in _model.Cart)
        {
            Item item = elem.Value;
            Sprite icon = item.ItemIcon;
            string itemName = item.ItemName;
            int count = item.ItemCount;

            _sellingView.SetSellingSlot(i, icon, itemName, count, item.TotalGold);
            i++;
        }

        _sellingView.SetProfitText(_model.GetProfit());
    }

    // 인벤 슬롯에 마우스를 올리면 아이템 툴팁을 보여줌
    void HandleInvenTooltipView(int index)
    {
        if (_inventory.IsEmptySlot(index))
        {
            _tooltip.HideView();
        }
        else
        {
            _tooltip.ShowView(_invenGrid.GetPosition(index), _inventory.GetSlotItem(index));
        }
    }

    // 인벤토리에 변동이 있으면 내용을 새로고침
    void HandleInvenSlotChanged(int index)
    {
        _invenGrid.Refresh(index, _inventory.GetSlotItem(index));
    }
}
