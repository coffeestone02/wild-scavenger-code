using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    [SerializeField] int _id; // 0~99:장비, 100~199: 소모품, 200~299:재료, 300~399 판매용 아이템
    [SerializeField] string _itemName; // 아이템 이름
    [SerializeField] Define.EItemType _itemType; // 아이템 타입 (장비, 소모품, 재료 등)
    [TextArea]
    [SerializeField] string _description; // 아이템 설명
    [SerializeField] Sprite _itemIcon; // 아이템 아이콘
    [SerializeField] GameObject _itemPrefab; // 아이템 프리팹 (월드에 배치할 때 사용)
    [SerializeField] int _gold; // 아이템 가치

    public int Id => _id;
    public string ItemName => _itemName;
    public Define.EItemType ItemType => _itemType;
    public string Description => _description;
    public Sprite ItemIcon => _itemIcon;
    public GameObject ItemPrefab => _itemPrefab;
    public int Gold => _gold;
}
