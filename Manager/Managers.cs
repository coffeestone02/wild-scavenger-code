using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance { get; private set; }

    InputManager _inputManager;
    UIManager _uiManager;
    Setting _settings;

    public static InputManager Input { get { return Instance._inputManager; } }
    public static UIManager UI { get { return Instance._uiManager; } }
    public static Setting Settings { get { return Instance._settings; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Bootstrap()
    {
        if (Instance != null) { return; }

        GameObject go = new GameObject("@Managers");
        Instance = go.AddComponent<Managers>();
        DontDestroyOnLoad(go);

        Instance._inputManager = new InputManager();
        Instance._uiManager = new UIManager();
        Instance._settings = new Setting();

        Input.Init();
    }

    void Update()
    {
        Input.OnUpdate();
    }

    void OnDestroy()
    {
        if (Instance != this) { return; }

        Input.Clear();
        UI.Clear();
        Instance = null;
    }
}
