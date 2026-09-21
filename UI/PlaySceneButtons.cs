using UnityEngine;
using UnityEngine.UI;

public class PlaySceneButtons : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button _closeBtn;
    [SerializeField] Button _titleBtn;

    [Header("AudioSO")]
    [SerializeField] AudioSO _title;

    void Awake()
    {
        _closeBtn.onClick.AddListener(CloseSceneUI);
        _titleBtn.onClick.AddListener(Title);
    }

    void CloseSceneUI()
    {
        Managers.UI.CloseCurrentSceneUI();
    }

    void Title()
    {
        AudioManager.Instance.PlayBGM(_title);
        CustomSceneManager.Instance.LoadScene("Title");
    }
}
