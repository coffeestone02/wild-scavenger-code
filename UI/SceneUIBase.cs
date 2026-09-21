using UnityEngine;

public class SceneUIBase : MonoBehaviour
{
    public string UIName => GetType().Name; // UISceneBase를 상속받은 클래스 이름을 사용
    [SerializeField] GameObject _hud;

    protected virtual void Awake()
    {
        Managers.UI.Register(this); // 생성하면서 UIScene에 등록
    }

    protected virtual void Start()
    {
        Close();
    }

    protected virtual void OnDestroy()
    {
        if (Managers.Instance == null) { return; }

        Managers.UI.Unregister(this);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        if (_hud != null)
        {
            _hud.SetActive(false);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        if (_hud != null)
        {
            _hud.SetActive(true);
        }
    }
}
