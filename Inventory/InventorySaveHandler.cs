using System;
using System.Collections.Generic;
using UnityEngine;

public static class InventorySaveHandler
{
    /// <summary> 인벤토리 슬롯 상황을 저장용 데이터로 변환 </summary>
    public static InventoryData CreateData(InventorySlotModel[] slots, InventorySlotModel[] armorSlots)
    {
        InventoryData data = new InventoryData();

        AppendSlotData(slots, data.Items);
        AppendSlotData(armorSlots, data.ArmorItems);

        return data;
    }

    /// <summary> 저장 데이터를 복원 </summary>
    public static void Restore(InventoryData data, InventorySlotModel[] slots, InventorySlotModel[] armorSlots, ItemDatabaseSO itemDB)
    {
        if (data == null) { return; }

        RestoreSlots(data.Items, slots, itemDB);
        RestoreSlots(data.ArmorItems, armorSlots, itemDB);
    }

    static void AppendSlotData(InventorySlotModel[] slots, List<ItemData> target)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            ItemData item = new ItemData();

            if (slots[i].IsEmpty)
            {
                item.IsEmpty = true;
            }
            else
            {
                Item forSave = slots[i].SlotItem;

                item.Id = forSave.Id;
                item.Count = forSave.ItemCount;
            }

            target.Add(item);
        }
    }

    static void RestoreSlots(List<ItemData> source, InventorySlotModel[] slots, ItemDatabaseSO itemDB)
    {
        int count = Math.Min(slots.Length, source.Count); // 짧은 쪽을 기준으로 하여 복원함
        for (int i = 0; i < count; i++)
        {
            ItemData item = source[i];
            if (item.IsEmpty)
            {
                slots[i].Clear();
            }
            else
            {
                ItemSO itemSO = itemDB.GetItemSO(item.Id);
                if (itemSO == null) // 삭제되었거나 Id가 바뀐 아이템은 슬롯을 비운채로 통과
                {
                    slots[i].Clear();
                    Debug.LogWarning($"[InventorySave] 알 수 없는 아이템 Id = {item.Id}, 슬롯 {i} 건너뜀");
                }
                else
                {
                    slots[i].SetSlot(new Item(itemSO, item.Count));
                }
            }
        }

        // 저장했을 때보다 슬롯이 줄었으면 경고
        if (source.Count > slots.Length)
        {
            Debug.LogWarning($"[InventorySave] 슬롯 축소로 {source.Count - slots.Length}칸 분량 유실");
        }
    }

}
