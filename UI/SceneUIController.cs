using UnityEngine;

public class SceneUIController : MonoBehaviour
{
    [SerializeField] Health _player;

    void Awake()
    {
        Managers.Input.OnInventoryPressed += OnInventoryToggle;
        Managers.Input.OnUIClosePressed += OnEscape;

        if (_player != null)
        {
            _player.OnDead += ShowDeadPanel;
        }
    }

    void OnDestroy()
    {
        if (Managers.Instance == null) { return; }

        Managers.Input.OnInventoryPressed -= OnInventoryToggle;
        Managers.Input.OnUIClosePressed -= OnEscape;

        if (_player != null)
        {
            _player.OnDead -= ShowDeadPanel;
        }
    }

    void OnInventoryToggle() => Managers.UI.OpenSceneUI<InventoryPresenter>();
    void OnEscape()
    {
        if (Managers.UI.IsCurrent<UIDead>()) { return; } // 사망 패널은 못닫게 함

        if (Managers.UI.IsSceneUIOpen)
        {
            Managers.UI.CloseCurrentSceneUI();
            return;
        }

        Managers.UI.OpenSceneUI<UISettingSlider>();
    }
    void ShowDeadPanel() => Managers.UI.OpenSceneUI<UIDead>();
}
