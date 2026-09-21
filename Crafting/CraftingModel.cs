using System;

public class CraftingModel
{
    const int MinCount = 1;
    const int MaxCount = 999;

    IInventoryStorage _inventory;
    RecipeDatabaseSO _recipeDB;
    CraftingRecipeSO _selectedRecipe;

    public CraftingRecipeSO[] Recipes => _recipeDB.Recipes;
    public CraftingRecipeSO SelectedRecipe => _selectedRecipe;
    public int Count { get; private set; } = MinCount;
    public event Action OnChanged;

    public CraftingModel(IInventoryStorage inventory, RecipeDatabaseSO recipeDB)
    {
        _inventory = inventory;
        _recipeDB = recipeDB;
    }

    /// <summary> 현재 레시피 설정 </summary>
    public void SelectRecipe(int index)
    {
        _selectedRecipe = Recipes[index];
        Count = MinCount;
        OnChanged?.Invoke();
    }

    /// <summary> 제작할 아이템 개수 감소 </summary>
    public void SubtractCount()
    {
        Count = Math.Clamp(Count - 1, MinCount, MaxCount);

        OnChanged?.Invoke();
    }

    /// <summary> 제작할 아이템 개수 증가 </summary>
    public void AddCount()
    {
        Count = Math.Clamp(Count + 1, MinCount, MaxCount);

        OnChanged?.Invoke();
    }

    /// <summary> 제작 여부 확인 </summary>
    public bool CanCraft()
    {
        if (_selectedRecipe == null) { return false; }

        bool result = true;
        foreach (Item ingredient in _selectedRecipe.Ingredients)
        {
            ItemSO itemSO = ingredient.ItemSO;
            int totalCount = ingredient.ItemCount * Count;

            // 인벤토리에서 보유량 확인
            if (_inventory.HasItem(itemSO, totalCount) == false)
            {
                result = false;
                break;
            }
        }

        return result;
    }

    public bool TryCraft()
    {
        // 만들 수 없거나 레시피 결과 아이템이 없음
        if (CanCraft() == false || _selectedRecipe.ResultItem == null) { return false; }

        // 재료 사용
        foreach (Item ingredient in _selectedRecipe.Ingredients)
        {
            ItemSO itemSO = ingredient.ItemSO;
            int totalNeed = ingredient.ItemCount * Count;

            _inventory.ConsumeItem(itemSO, totalNeed);
        }

        // 아이템 지급
        Item result = new Item(_selectedRecipe.ResultItem);
        result.ItemCount = result.ItemCount * Count;
        _inventory.TryAcquireItem(result);

        OnChanged?.Invoke();
        return true;
    }
}
