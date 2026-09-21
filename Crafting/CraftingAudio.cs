using UnityEngine;

public class CraftingAudio : MonoBehaviour
{
    [SerializeField] AudioSO _normalBtn;
    [SerializeField] AudioSO _countBtn;
    [SerializeField] AudioSO _selectBtn;
    [SerializeField] AudioSO _craftSuccess;

    [SerializeField] CraftingView _view;
    [SerializeField] CraftingPresenter _presenter;

    void Awake()
    {
        _view.OnAddClicked += HandleCountSound;
        _view.OnSubtractClicked += HandleCountSound;
        _view.OnCraftClicked += HandleCraftClickSound;

        _presenter.OnSlotClicked += HandleSelectSound;
        _presenter.OnCraftSuccessed += HandleCraftSuccessSound;
    }

    void OnDestroy()
    {
        _view.OnAddClicked -= HandleCountSound;
        _view.OnSubtractClicked -= HandleCountSound;
        _view.OnCraftClicked -= HandleCraftClickSound;

        _presenter.OnSlotClicked -= HandleSelectSound;
        _presenter.OnCraftSuccessed -= HandleCraftSuccessSound;
    }

    void HandleCountSound()
    {
        AudioManager.Instance.PlaySFX(_countBtn);
    }

    void HandleCraftClickSound()
    {
        AudioManager.Instance.PlaySFX(_normalBtn);
    }

    void HandleSelectSound()
    {
        AudioManager.Instance.PlaySFX(_selectBtn);
    }

    void HandleCraftSuccessSound()
    {
        AudioManager.Instance.PlaySFX(_craftSuccess);
    }
}
