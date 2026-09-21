using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryClickView : MonoBehaviour
{
    [Header("UI 컴포넌트")]
    [SerializeField] Image _itemIcon;
    [SerializeField] TMP_Text _itemCountText;

    void Update()
    {
        // 마우스 포인터 위치를 계속 따라다니게 함
        transform.position = Mouse.current.position.ReadValue();
    }

    /// <summary> 선택된 아이템이 마우스 포인터를 따라다니게 함</summary>
    public void Selected(ItemSO item, int count)
    {
        if (item == null) { return; }

        transform.position = Mouse.current.position.ReadValue();

        _itemIcon.sprite = item.ItemIcon;
        if (item.ItemType != Define.EItemType.Equipment) // 장비가 아니면 개수 표시
        {
            _itemCountText.text = count.ToString();
        }
        else
        {
            _itemCountText.text = "";
        }

        gameObject.SetActive(true);
    }

    public void Clear()
    {
        _itemIcon.sprite = null;
        _itemCountText.text = "";
        gameObject.SetActive(false);
    }
}
