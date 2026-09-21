using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI 컴포넌트")]
    [SerializeField] Image background;
    [SerializeField] Image _itemIcon;
    [SerializeField] GameObject _displayItem;
    [SerializeField] TMP_Text _itemCountText;
    [SerializeField] GameObject _selectedIcon;

    [Header("장비 착용 색상")]
    [SerializeField] Color originColor = Color.white;
    [SerializeField] Color equiColor = Color.cyan;

    public event Action<int> OnClicked; // 좌클릭
    public event Action<int> OnRightClicked; // 우클릭
    public event Action<int> OnHovered; // 호버링

    public int SlotIndex { get; private set; }
    public RectTransform Position { get; private set; }

    public void Init(int index)
    {
        SlotIndex = index;
        Position = GetComponent<RectTransform>();
    }

    /// <summary> UI 새로고침 </summary>
    public void Refresh(Item item)
    {
        if (item == null) // 아이템이 없음
        {
            Clear();
            return;
        }

        _displayItem.SetActive(true);
        _itemIcon.sprite = item.ItemIcon;

        // 장비인 경우 아이템 수량 텍스트 숨김
        if (item.ItemType == Define.EItemType.Equipment)
        {
            _itemCountText.text = "";
        }
        else
        {
            _itemCountText.text = item.ItemCount.ToString();
        }
    }

    public void Clear()
    {
        _displayItem.SetActive(false);
        SetSelectedIcon(false);
        _itemIcon.sprite = null;
        _itemCountText.text = "";
    }

    public void SetSelectedIcon(bool isActive)
    {
        _selectedIcon.SetActive(isActive);
    }

    public void Equip()
    {
        background.color = equiColor;
    }

    public void Unequip()
    {
        background.color = originColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button.Equals(PointerEventData.InputButton.Right)) // 우클릭
        {
            OnRightClicked?.Invoke(SlotIndex);
        }
        else // 좌클릭
        {
            OnClicked?.Invoke(SlotIndex);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHovered?.Invoke(SlotIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHovered?.Invoke(-1);
    }
}
