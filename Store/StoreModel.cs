using System;
using System.Collections.Generic;

public class StoreModel
{
    IInventoryStorage _inventory;
    StoreSO _store;
    IWallet _playerWallet;

    public Item[] Products => _store.Products;
    public Dictionary<int, Item> ShoppingCart { get; private set; }

    public event Action OnChanged;

    public StoreModel(IInventoryStorage inventory, StoreSO store, IWallet playerWallet)
    {
        _inventory = inventory;
        _store = store;
        _playerWallet = playerWallet;

        ShoppingCart = new Dictionary<int, Item>();
    }

    /// <summary> 물건을 담거나 제거함 </summary>
    public bool UpdateItem(int index)
    {
        if (index < 0 || index >= Products.Length) { return false; }

        // 이미 선택한 물건이면 제거
        if (ShoppingCart.ContainsKey(index))
        {
            ShoppingCart.Remove(index);
            OnChanged?.Invoke();
            return false;
        }

        // 새 물건이면 추가
        Item add = new Item(Products[index]);
        ShoppingCart[index] = add;

        OnChanged?.Invoke();
        return true;
    }

    /// <summary> 카트 비우기 </summary>
    public void ClearItems()
    {
        ShoppingCart.Clear();
        OnChanged?.Invoke();
    }

    /// <summary> 카트 가격 계산 </summary>
    public int GetPrice()
    {
        int profit = 0;

        foreach (var elem in ShoppingCart)
        {
            Item item = elem.Value;
            profit += item.TotalGold;
        }

        return profit;
    }

    /// <summary> 구매 가능 여부 확인 </summary>
    public bool CanBuy()
    {
        int totalPrice = GetPrice();
        return ShoppingCart.Count > 0 && _playerWallet.Gold >= totalPrice;
    }

    /// <summary> 구매 </summary>
    public bool TryBuy()
    {
        if (CanBuy() == false) { return false; }

        int totalPrice = GetPrice();
        _playerWallet.TrySpendGold(totalPrice);

        // 인벤토리에 공간이 없으면 필드에 반환됨
        foreach (var elem in ShoppingCart)
        {
            Item item = elem.Value;
            _inventory.TryAcquireItem(item);
        }

        ClearItems();

        return true;
    }
}
