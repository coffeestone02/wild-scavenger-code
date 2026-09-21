using UnityEngine;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    [SerializeField] Button _playBtn;
    [SerializeField] Button _settingBtn;
    [SerializeField] Button _exitBtn;
    [SerializeField] Button _closeBtn;
    [SerializeField] Button _resetSaveDataBtn;

    [SerializeField] GameObject _titleSelectGroup;
    [SerializeField] AudioSO _main;

    void Awake()
    {
        _playBtn.onClick.AddListener(Play);
        _settingBtn.onClick.AddListener(Setting);
        _exitBtn.onClick.AddListener(Exit);
        _closeBtn.onClick.AddListener(CloseSceneUI);
        _resetSaveDataBtn.onClick.AddListener(ResetSaveData);

        Managers.UI.SetMouseCursor(true);
    }

    void Play()
    {
        _titleSelectGroup.SetActive(false);
        AudioManager.Instance.PlayBGM(_main);
        CustomSceneManager.Instance.LoadScene("PlayerHouse");
    }

    void Setting()
    {
        Managers.UI.OpenSceneUI<UISettingSlider>();
    }

    void CloseSceneUI()
    {
        Managers.UI.CloseCurrentSceneUI();
    }

    void ResetSaveData()
    {
        SaveManager.ResetGame();
    }

    void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
