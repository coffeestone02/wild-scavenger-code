using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    Dictionary<string, SceneUIBase> _SceneUIs = new Dictionary<string, SceneUIBase>();
    SceneUIBase _currentSceneUI;
    bool _cursorVisible;

    public bool IsSceneUIOpen => _currentSceneUI != null; // 열려있는 SceneUI이 있으면 true

    T Get<T>() where T : SceneUIBase
    {
        if (_SceneUIs.TryGetValue(typeof(T).Name, out var ui)) { return ui as T; }

        return null;
    }

    /// <summary> SceneUI 관리 딕셔너리에 추가 </summary>
    public void Register(SceneUIBase ui)
    {
        _SceneUIs[ui.UIName] = ui;
    }

    /// <summary> SceneUI 관리 딕셔너리에서 제거 </summary>
    public void Unregister(SceneUIBase ui)
    {
        if (_SceneUIs.TryGetValue(ui.UIName, out var registered))
        {
            _SceneUIs.Remove(ui.UIName);
        }
    }

    public void OpenSceneUI<T>() where T : SceneUIBase
    {
        var target = Get<T>();

        // SceneUIBase이 없는 경우
        if (target == null) { return; }

        // 열려있는 SceneUI가 있다면 먼저 닫음
        CloseCurrentSceneUI();

        // target UI를 열음
        target.Open();
        _currentSceneUI = target;
        Managers.Input.EnableUI(); // UI 입력 활성화

        ApplyCursor(true);
    }

    public bool IsCurrent<T>() where T : SceneUIBase
    {
        return _currentSceneUI is T;
    }

    public void CloseCurrentSceneUI()
    {
        if (_currentSceneUI == null) { return; }

        _currentSceneUI.Close();
        _currentSceneUI = null;
        Managers.Input.EnableGameplay(); // UI 입력 비활성화

        ApplyCursor(_cursorVisible);
    }

    public void SetMouseCursor(bool visible)
    {
        _cursorVisible = visible;
        ApplyCursor(visible);
    }

    void ApplyCursor(bool visible)
    {
        if (visible)
        {
            Cursor.lockState = CursorLockMode.None; // 마우스 커서 표시
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 숨김
            Cursor.visible = false;
        }
    }

    public void Clear()
    {
        CloseCurrentSceneUI();
        _SceneUIs.Clear();
    }
}
