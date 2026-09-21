
using UnityEngine;

public class Util
{
    public static void ItemSpawn(Item item, Vector3 position)
    {
        GameObject go = Object.Instantiate(item.ItemSO.ItemPrefab, position, Quaternion.Euler(0, 0, 0));
        ItemInteract info = go.GetComponent<ItemInteract>();
        info.Count = item.ItemCount;
    }

    /// <summary> 오브젝트가 해당 LayerMask에 포함되는지 검사 </summary>
    public static bool IsInLayer(GameObject go, LayerMask mask)
    {
        return (mask.value & (1 << go.layer)) != 0;
    }
}
