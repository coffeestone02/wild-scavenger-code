using System;
using UnityEngine;

public class CraftingPresenter : SceneUIBase
{
    [SerializeField] CraftingView _craftingView;
    [SerializeField] Transform _craftingSlotParent;
    [SerializeField] UIItemTooltip _tooltip;
    [SerializeField] GameObject _craftingSlotPrefab;

    CraftingModel _model;
    CraftingSlotView[] _craftingSlotViews;
    IInventoryStorage _inventory;

    public event Action OnSlotClicked;
    public event Action OnCraftSuccessed;

    public void Init(CraftingModel model, IInventoryStorage inventory)
    {
        _model = model;
        _inventory = inventory;

        _craftingSlotViews = new CraftingSlotView[_model.Recipes.Length];
        for (int i = 0; i < _craftingSlotViews.Length; i++)
        {
            GameObject go = Instantiate(_craftingSlotPrefab, _craftingSlotParent);

            _craftingSlotViews[i] = go.GetComponent<CraftingSlotView>();
            _craftingSlotViews[i].Init(i, model.Recipes[i].ResultItem.ItemIcon);
            _craftingSlotViews[i].OnClicked += HandleRecipeSlotClicked;
            _craftingSlotViews[i].OnHovered += HandleItemTooltipChanged;
        }

        // 버튼
        _craftingView.OnCraftClicked += HandleCraftClicked;
        _craftingView.OnAddClicked += HandleAddClicked;
        _craftingView.OnSubtractClicked += HandleSubtractClicked;

        // 제작 후 인벤토리 변화
        _inventory.OnSlotChanged += HandleInventoryChanged;

        // 모델 변화
        _model.OnChanged += HandleModelChanged;
    }

    void OnDisable()
    {
        _tooltip.HideView();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        for (int i = 0; i < _craftingSlotViews.Length; i++)
        {
            _craftingSlotViews[i].OnClicked -= HandleRecipeSlotClicked;
            _craftingSlotViews[i].OnHovered -= HandleItemTooltipChanged;
        }

        _craftingView.OnCraftClicked -= HandleCraftClicked;
        _craftingView.OnAddClicked -= HandleAddClicked;
        _craftingView.OnSubtractClicked -= HandleSubtractClicked;

        _inventory.OnSlotChanged -= HandleInventoryChanged;

        _model.OnChanged -= HandleModelChanged;
    }

    // 레시피 슬롯 클릭
    void HandleRecipeSlotClicked(int index)
    {
        if (index < 0 || index >= _model.Recipes.Length) { return; }

        _model.SelectRecipe(index);
        OnSlotClicked?.Invoke();
    }

    // 제작 버튼 클릭
    void HandleCraftClicked()
    {
        if (_model.SelectedRecipe == null) { return; }

        bool result = _model.TryCraft();
        if (result)
        {
            OnCraftSuccessed?.Invoke();
        }
    }

    // 개수 증가 버튼 클릭
    void HandleAddClicked()
    {
        _model.AddCount();
    }

    // 개수 감소 버튼 클릭
    void HandleSubtractClicked()
    {
        _model.SubtractCount();
    }

    // 모델의 내용이 바뀌면 뷰 새로고침(제작 UI가 활성화 됐을 때만 실행)
    void HandleModelChanged()
    {
        if (isActiveAndEnabled == false) { return; }

        RefreshView();
    }

    // 인벤토리 내용이 바뀌면 뷰 새로고침(제작 UI가 활성화 됐을 때만 실행)
    void HandleInventoryChanged(int _)
    {
        if (isActiveAndEnabled == false) { return; }

        RefreshView();
    }

    // 아이템 툴팁
    void HandleItemTooltipChanged(int index)
    {
        if (index == -1)
        {
            _tooltip.HideView();
        }
        else
        {
            _tooltip.ShowView(_craftingSlotViews[index].Position, _model.Recipes[index].ResultItem);
        }
    }

    // 결과 아이템과 재료 슬롯 UI를 새로고침
    void RefreshView()
    {
        CraftingRecipeSO recipe = _model.SelectedRecipe;

        if (recipe == null) { return; }

        // 결과 아이템 UI
        Item resultItem = recipe.ResultItem;
        string nameAndCount = $"{resultItem.ItemName} x{resultItem.ItemCount}";
        _craftingView.SetResultItem(resultItem.ItemIcon, nameAndCount);// 1회 제작으로 얻을 수 있는 개수만 표시
        _craftingView.SetCraftCountText(_model.Count);

        // 재료 슬롯
        Item[] ingredients = recipe.Ingredients;
        _craftingView.PrepareIngredientSlot(ingredients.Length);
        for (int i = 0; i < ingredients.Length; i++)
        {
            Item ing = ingredients[i];
            int needed = ing.ItemCount * _model.Count;
            int owned = _inventory.GetOwned(ing.ItemSO);
            bool isEnough = _inventory.HasItem(ing.ItemSO, needed);

            _craftingView.SetIngredientSlot(i, ing.ItemIcon, ing.ItemName, owned, needed, isEnough);
        }

        _craftingView.SetCraftButtonInteractable(_model.CanCraft());
    }
}
