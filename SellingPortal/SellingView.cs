using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellingView : MonoBehaviour
{
    // 리스트, 판매 버튼
    [SerializeField] Transform _sellSlotRoot;
    [SerializeField] GameObject _sellSlotPrefab;
    [SerializeField] Button _sellButton;
    [SerializeField] TMP_Text _profitText;
    [SerializeField] TMP_Text _playerGoldText;

    List<SellingSlotView> _sellingSlots = new List<SellingSlotView>();

    public event Action OnSellClicked;


    void Awake()
    {
        _sellButton.onClick.AddListener(HandleSellClicked);

        // 미리 일정 개수 이상 만들어 놓기
        SellingSlotView[] views = _sellSlotRoot.GetComponentsInChildren<SellingSlotView>();
        foreach (SellingSlotView view in views)
        {
            _sellingSlots.Add(view);
            view.gameObject.SetActive(false);
        }
    }

    void HandleSellClicked()
    {
        OnSellClicked?.Invoke();
    }

    // 슬롯 준비
    public void PrepareSellingSlot(int count)
    {
        while (_sellingSlots.Count < count) // 부족하면 추가
        {
            GameObject go = Instantiate(_sellSlotPrefab, _sellSlotRoot);
            if (go.TryGetComponent(out SellingSlotView slot))
            {
                _sellingSlots.Add(slot);
            }
            else // 프리팹에 SellingSlotView가 없음
            {
                Destroy(go);
            }
        }

        for (int i = 0; i < _sellingSlots.Count; i++)
        {
            _sellingSlots[i].gameObject.SetActive(i < count);
        }
    }

    public void SetSellingSlot(int index, Sprite icon, string itemName, int count, int value)
    {
        _sellingSlots[index].Set(icon, itemName, count, value);
    }

    public void SetProfitText(int profit)
    {
        _profitText.text = "가격: " + profit.ToString();
    }

    public void SetMoneyText(int money)
    {
        _playerGoldText.text = $"보유금: {money}";
    }
}
