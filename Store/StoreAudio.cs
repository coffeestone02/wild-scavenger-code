using UnityEngine;

public class StoreAudio : MonoBehaviour
{
    [SerializeField] AudioSO _selectBtn;
    [SerializeField] AudioSO _normalBtn;

    [SerializeField] StorePresenter _presenter;

    void Awake()
    {
        _presenter.OnSlotClicked += HandleSelectSound;
        _presenter.OnBought += HandleBuyClickSound;
    }

    void OnDestroy()
    {
        _presenter.OnSlotClicked -= HandleSelectSound;
        _presenter.OnBought -= HandleBuyClickSound;
    }

    void HandleSelectSound()
    {
        AudioManager.Instance.PlaySFX(_selectBtn);
    }

    void HandleBuyClickSound()
    {
        AudioManager.Instance.PlaySFX(_normalBtn);
    }
}
