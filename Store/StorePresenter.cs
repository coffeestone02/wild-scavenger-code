using UnityEngine;
using System;

public class StorePresenter : SceneUIBase
{
    [Header("View Parents")]
    [SerializeField] Transform _storeSlotParent;

    [Header("Views")]
    [SerializeField] UIItemTooltip _tooltip;
    [SerializeField] StoreView _storeView;
    [SerializeField] InventoryGridView _invenGrid;

    [Header("Slot Prefab")]
    [SerializeField] GameObject _storeSlotPrefab;

    StoreModel _model;
    IInventoryStorage _inventory;
    StoreSlotView[] _storeSlotViews;
    bool _isInitialized;

    public event Action OnSlotClicked;
    public event Action OnBought;

    public void Init(StoreModel model, IInventoryStorage inventory)
    {
        _model = model;
        _inventory = inventory;

        // 판매 아이템 목록
        _storeSlotViews = new StoreSlotView[_model.Products.Length];
        for (int i = 0; i < _storeSlotViews.Length; i++)
        {
            GameObject go = Instantiate(_storeSlotPrefab, _storeSlotParent);

            _storeSlotViews[i] = go.GetComponent<StoreSlotView>();
            _storeSlotViews[i].Init(i, _model.Products[i]);
            _storeSlotViews[i].OnClicked += HandleStoreSlotClicked;
            _storeSlotViews[i].OnHovered += HandleStoreTooltipView;
        }

        // 인벤토리 슬롯뷰
        _invenGrid.Init(_inventory.SlotCount);
        _invenGrid.OnHovered += HandleInvenTooltipView;

        _model.OnChanged += HandleModelChanged;

        // 구매 버튼
        _storeView.OnBuyClicked += HandleBuyClicked;
        _storeView.SetBuyButtonInteractable(false);

        // 인벤토리 슬롯 변동
        _inventory.OnSlotChanged += HandleInvenSlotChanged;

        _isInitialized = true;
        if (isActiveAndEnabled) // Init 전에 이미 OnEnable를 실행했다면 RefeshInventoryView 다시 실행
        {
            RefreshInventoryView();
            _storeView.ClearView();
        }
    }

    void OnEnable()
    {
        if (_isInitialized == false) { return; }

        RefreshInventoryView();
        _storeView.ClearView();
    }

    void OnDisable()
    {
        _tooltip.HideView();
        _model.ClearItems();
        ResetAllSelectedIcons();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        for (int i = 0; i < _storeSlotViews.Length; i++)
        {
            _storeSlotViews[i].OnClicked -= HandleStoreSlotClicked;
            _storeSlotViews[i].OnHovered -= HandleStoreTooltipView;
        }

        _invenGrid.Dispose();

        _model.OnChanged -= HandleModelChanged;

        _storeView.OnBuyClicked -= HandleBuyClicked;
        _inventory.OnSlotChanged -= HandleInvenSlotChanged;
    }

    void HandleModelChanged()
    {
        // 구매 가능 여부에 따라 버튼 활성화
        _storeView.SetBuyButtonInteractable(_model.CanBuy());

        // 상점 가격 업데이트
        _storeView.UpdatePriceText(_model.GetPrice());
    }

    void HandleStoreSlotClicked(int index)
    {
        if (index < 0 || index >= _storeSlotViews.Length) { return; }

        bool result = _model.UpdateItem(index);
        _storeSlotViews[index].SetSelectedIcon(result);

        OnSlotClicked?.Invoke();
    }

    void HandleBuyClicked()
    {
        bool result = _model.TryBuy();

        if (result)
        {
            _storeView.ClearView();
            ResetAllSelectedIcons();
            OnBought?.Invoke();
        }
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

    // 상점 슬롯에 마우스를 올리면 아이템 툴팁을 보여줌
    void HandleStoreTooltipView(int index)
    {
        if (index == -1)
        {
            _tooltip.HideView();
        }
        else
        {
            _tooltip.ShowView(_storeSlotViews[index].Position, _model.Products[index]);
        }
    }

    // 인벤토리에 변동이 있으면 내용을 새로고침
    void HandleInvenSlotChanged(int index)
    {
        _invenGrid.Refresh(index, _inventory.GetSlotItem(index));
    }

    // 창을 열었을 때 초기 로드용
    void RefreshInventoryView()
    {
        for (int i = 0; i < _inventory.SlotCount; i++)
        {
            _invenGrid.Refresh(i, _inventory.GetSlotItem(i));
        }
    }

    void ResetAllSelectedIcons()
    {
        foreach (StoreSlotView slot in _storeSlotViews)
        {
            slot.SetSelectedIcon(false);
        }
    }
}
