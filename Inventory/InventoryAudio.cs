using UnityEngine;

public class InventoryAudio : MonoBehaviour
{
    [SerializeField] AudioSO _selectBtn;

    [SerializeField] InventoryPresenter _presenter;

    void Start()
    {
        _presenter.OnSlotClicked += HandleSelectSound;
    }

    void OnDestroy()
    {
        _presenter.OnSlotClicked -= HandleSelectSound;
    }

    // 슬롯 선택
    void HandleSelectSound()
    {
        AudioManager.Instance.PlaySFX(_selectBtn);
    }
}
