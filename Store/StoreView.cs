using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreView : MonoBehaviour
{
    [SerializeField] Button _buyButton;
    [SerializeField] TMP_Text _priceText;

    public event Action OnBuyClicked;

    void Awake()
    {
        _buyButton.onClick.AddListener(HandleBuyClicked);
    }

    void HandleBuyClicked()
    {
        OnBuyClicked?.Invoke();
    }

    public void UpdatePriceText(int price)
    {
        _priceText.text = $"가격: {price}";
    }

    public void SetBuyButtonInteractable(bool interactable)
    {
        _buyButton.interactable = interactable;
    }

    public void ClearView()
    {
        _priceText.text = "가격: 0";
        SetBuyButtonInteractable(false);
    }
}
