using System;
using UnityEngine;

public class InventoryGridView : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] Transform _slotParent;
    [SerializeField] GameObject _slotPrefab;

    InventorySlotView[] _slotViews;
    int _equippedIndex = -1;

    public event Action<int> OnClicked; // 좌클릭
    public event Action<int> OnRightClicked; // 우클릭
    public event Action<int> OnHovered; // 호버링

    public void Init(int count)
    {
        _slotViews = new InventorySlotView[count];
        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(_slotPrefab, _slotParent);

            _slotViews[i] = go.GetComponent<InventorySlotView>();
            _slotViews[i].Init(i);
            _slotViews[i].OnClicked += HandleSlotClicked;
            _slotViews[i].OnRightClicked += HandleSlotRightClicked;
            _slotViews[i].OnHovered += HandleSlotHovered;
        }
    }

    public void Dispose()
    {
        foreach (InventorySlotView slot in _slotViews)
        {
            slot.OnClicked -= HandleSlotClicked;
            slot.OnRightClicked -= HandleSlotRightClicked;
            slot.OnHovered -= HandleSlotHovered;
        }
    }

    void HandleSlotClicked(int index)
    {
        OnClicked?.Invoke(index);
    }

    void HandleSlotRightClicked(int index)
    {
        OnRightClicked?.Invoke(index);
    }

    void HandleSlotHovered(int index)
    {
        OnHovered?.Invoke(index);
    }

    public void Refresh(int index, Item item)
    {
        if (index < 0 || index >= _slotViews.Length) { return; }
        _slotViews[index].Refresh(item);
    }

    public RectTransform GetPosition(int index)
    {
        return _slotViews[index].Position;
    }

    public void SetSelected(int index, bool isSelected)
    {
        _slotViews[index].SetSelectedIcon(isSelected);
    }

    public void ClearAllSelected()
    {
        for (int i = 0; i < _slotViews.Length; i++)
        {
            _slotViews[i].SetSelectedIcon(false);
        }
    }

    /// <summary> 인덱스에 있는 장비를 착용표시(-1은 착용표시 해제) </summary>
    public void SetEquipped(int index)
    {
        if (_equippedIndex == index) { return; }

        if (index == -1)
        {
            _slotViews[_equippedIndex].Unequip();
            _equippedIndex = -1;
        }
        else
        {
            _equippedIndex = index;
            _slotViews[_equippedIndex].Equip();
        }
    }
}
