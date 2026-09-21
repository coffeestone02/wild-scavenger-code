using UnityEngine;

[System.Serializable]
public class Item
{
    public ItemSO ItemSO;
    public int ItemCount = 1;

    public Item(ItemSO item, int count)
    {
        if (item == null)
        {
            ItemCount = 0;
            return;
        }

        ItemSO = item;
        ItemCount = count;
    }

    public Item(Item item)
    {
        if (item == null)
        {
            ItemCount = 0;
            return;
        }
        ItemSO = item.ItemSO;
        ItemCount = item.ItemCount;
    }

    public int Id => ItemSO.Id;
    public string ItemName => ItemSO.ItemName;
    public Define.EItemType ItemType => ItemSO.ItemType;
    public string Description => ItemSO.Description;
    public Sprite ItemIcon => ItemSO.ItemIcon;
    public GameObject ItemPrefab => ItemSO.ItemPrefab;
    public int TotalGold => ItemSO.Gold * ItemCount;

    public EquipmentStatSO GetEquipmentStatSO()
    {
        if (ItemSO is EquipmentStatSO)
        {
            return ItemSO as EquipmentStatSO;
        }

        return null;
    }
}
