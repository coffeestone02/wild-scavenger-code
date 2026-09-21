using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabaseSO", menuName = "Scriptable Objects/ItemDatabaseSO")]
public class ItemDatabaseSO : ScriptableObject
{
    [SerializeField] ItemSO[] _items;

    Dictionary<int, ItemSO> _table;

    public void Init()
    {
        _table = new Dictionary<int, ItemSO>();

        for (int i = 0; i < _items.Length; i++)
        {
            ItemSO item = _items[i];

            if (item == null)
            {
                Debug.LogError($"[ItemDatabase] {i}번 항목이 비어있음.");
                continue;
            }

            if (_table.TryGetValue(item.Id, out ItemSO exist))
            {
                Debug.LogError($"[ItemDatabase] ID {item.Id} 중복: {exist.name}");
                continue;
            }

            _table.Add(item.Id, item);
        }
    }

    public ItemSO GetItemSO(int id)
    {
        if (_table == null) { Init(); } // 아이템 테이블이 초기화되지 않았으면 테이블을 초기화하고 진행
        if (_table.TryGetValue(id, out ItemSO item)) { return item; }

        return null;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        HashSet<int> ids = new HashSet<int>(); // 중복 확인용
        foreach (ItemSO item in _items)
        {
            if (item != null && ids.Add(item.Id) == false)
            {
                Debug.LogError($"[ItemDatabase] ID {item.Id} 중복: {item.name}");
            }
        }
    }
# endif
}
