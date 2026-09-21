using UnityEngine;

public class SellingPortalAudio : MonoBehaviour
{
    [SerializeField] AudioSO _normalBtn;
    [SerializeField] AudioSO _selectBtn;
    [SerializeField] AudioSO _water;

    [SerializeField] SellingPortalPresenter _presenter;

    void Awake()
    {
        _presenter.OnSlotClicked += HandleSelectSound;
        _presenter.OnSold += HandleSellClicked;
    }

    void OnDestroy()
    {
        _presenter.OnSlotClicked -= HandleSelectSound;
        _presenter.OnSold -= HandleSellClicked;
    }

    // 판매 버튼
    void HandleSellClicked()
    {
        AudioManager.Instance.PlaySFX(_normalBtn);
        AudioManager.Instance.PlaySFX(_water);
    }

    // 슬롯 선택
    void HandleSelectSound()
    {
        AudioManager.Instance.PlaySFX(_selectBtn);
    }
}
