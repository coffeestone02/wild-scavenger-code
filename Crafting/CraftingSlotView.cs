using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingSlotView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image _itemIcon;
    RectTransform _rectTF;

    public RectTransform Position => _rectTF;
    public int SlotIndex { get; private set; }

    public event Action<int> OnClicked;
    public event Action<int> OnHovered;

    public void Init(int index, Sprite icon)
    {
        _itemIcon.sprite = icon;
        _rectTF = GetComponent<RectTransform>();
        SlotIndex = index;
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
