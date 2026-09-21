using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingView : MonoBehaviour
{
    [Header("Result Item")]
    [SerializeField] Image _resultIcon;
    [SerializeField] TMP_Text _resultItemName;

    [Header("Ingredients")]
    [SerializeField] Transform _ingredientSlotRoot;
    [SerializeField] GameObject _ingredientSlotPrefab;

    [Header("Buttons")]
    [SerializeField] Button _craftButton;
    [SerializeField] Button _subtractButton;
    [SerializeField] Button _addButton;

    [Header("Count")]
    [SerializeField] TMP_Text _itemCountText;

    List<IngredientSlotView> _ingredientSlots = new List<IngredientSlotView>();

    public event Action OnCraftClicked;
    public event Action OnAddClicked;
    public event Action OnSubtractClicked;

    void Awake()
    {
        _craftButton.onClick.AddListener(HandleCraftClicked);
        _subtractButton.onClick.AddListener(HandleSubtractClicked);
        _addButton.onClick.AddListener(HandleAddClicked);

        SetCraftButtonInteractable(false);
    }

    void HandleCraftClicked()
    {
        OnCraftClicked?.Invoke();
    }

    void HandleAddClicked()
    {
        OnAddClicked?.Invoke();
    }

    void HandleSubtractClicked()
    {
        OnSubtractClicked?.Invoke();
    }

    /// <summary> 재료 슬롯을 준비 </summary>
    public void PrepareIngredientSlot(int slotCount)
    {
        // 슬롯이 부족하면 추가(슬롯이 충분하면 실행 안함)
        while (_ingredientSlots.Count < slotCount)
        {
            GameObject go = Instantiate(_ingredientSlotPrefab, _ingredientSlotRoot);
            IngredientSlotView slot = go.GetComponent<IngredientSlotView>();
            _ingredientSlots.Add(slot);
        }

        // 필요 개수만 활성화하고 슬롯이 남으면 비활성화
        for (int i = 0; i < _ingredientSlots.Count; i++)
        {
            _ingredientSlots[i].gameObject.SetActive(i < slotCount);
        }
    }

    /// <summary> 재료 슬롯 설정 </summary>
    public void SetIngredientSlot(int index, Sprite icon, string itemName, int owned, int needed, bool isEnough)
    {
        _ingredientSlots[index].Set(icon, itemName, owned, needed, isEnough);
    }

    /// <summary> 결과 아이템 설정 </summary>
    public void SetResultItem(Sprite icon, string nameAndCount)
    {
        _resultIcon.sprite = icon;
        _resultItemName.text = nameAndCount;
    }

    /// <summary> 제작 가능 개수 설정 </summary>
    public void SetCraftCountText(int count)
    {
        _itemCountText.text = count.ToString();
    }

    /// <summary> 제작 버튼 활성화 여부 </summary>
    public void SetCraftButtonInteractable(bool interactable)
    {
        _craftButton.interactable = interactable;
    }
}
