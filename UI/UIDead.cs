using UnityEngine;
using UnityEngine.UI;

public class UIDead : SceneUIBase
{
    [SerializeField] Button _reviveBtn;

    [SerializeField] AudioSO _house;

    protected override void Awake()
    {
        base.Awake();

        _reviveBtn.onClick.AddListener(Revive);
    }

    void Revive()
    {
        Managers.UI.CloseCurrentSceneUI();
        AudioManager.Instance.PlayBGM(_house);
        CustomSceneManager.Instance.LoadScene("PlayerHouse");
    }
}
