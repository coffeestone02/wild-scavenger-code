using System;
using System.Collections.Generic;

public class SellingModel
{
    IInventoryStorage _inventory;
    IWallet _playerWallet;
    public Dictionary<int, Item> Cart { get; private set; }
    public event Action OnChanged;

    public SellingModel(IInventoryStorage inventory, IWallet playerWallet)
    {
        _inventory = inventory;
        _playerWallet = playerWallet;
        Cart = new Dictionary<int, Item>();
    }

    /// <summary> 아이템을 판매목록에 추가하거나 제거 </summary>
    public bool UpdateItem(int index)
    {
        Item item = _inventory.GetSlotItem(index);
        if (item == null) { return false; }

        if (Cart.ContainsKey(index)) // 카트에 이미 담은 아이템이 있으면 제거함
        {
            Cart.Remove(index);
            OnChanged?.Invoke();
            return false;
        }

        Cart[index] = item;
        OnChanged?.Invoke();
        return true;
    }

    /// <summary> 판매 리스트 비우기 </summary>
    public void ClearItems()
    {
        Cart.Clear();
        OnChanged?.Invoke();
    }

    /// <summary> 판매 </summary>
    public int SellItems()
    {
        int profit = 0;

        foreach (var elem in Cart)
        {
            int index = elem.Key;
            Item item = elem.Value;
            profit += item.TotalGold;
            _inventory.ConsumeItemAt(index, item.ItemCount);
        }
        Cart.Clear();

        _playerWallet.AddGold(profit); // 판매 골드 지급

        OnChanged?.Invoke();
        return profit;
    }

    /// <summary> 판매 수익 계산 </summary>
    public int GetProfit()
    {
        int profit = 0;

        foreach (var elem in Cart)
        {
            int index = elem.Key;
            Item item = elem.Value;
            profit += item.TotalGold;
        }

        return profit;
    }
}
