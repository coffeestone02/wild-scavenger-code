using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreSlotView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image _itemIcon;
    [SerializeField] GameObject _selectedIcon;
    [SerializeField] TMP_Text _itemCountText;

    RectTransform _rectTF;

    public event Action<int> OnClicked;
    public event Action<int> OnHovered;

    public RectTransform Position => _rectTF;
    public int SlotIndex { get; private set; }

    public void Init(int index, Item item)
    {
        _rectTF = GetComponent<RectTransform>();
        SlotIndex = index;

        // UI 설정
        _itemIcon.sprite = item.ItemIcon;
        _itemCountText.text = item.ItemCount.ToString();
    }

    public void SetSelectedIcon(bool isActive)
    {
        _selectedIcon.SetActive(isActive);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke(SlotIndex);
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
