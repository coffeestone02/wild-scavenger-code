using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISettingSlider : SceneUIBase
{
    [SerializeField] TMP_Text _mouseText;
    [SerializeField] Slider _bgm;
    [SerializeField] Slider _sfx;
    [SerializeField] Slider _mouseSensitivity;

    Setting _setting;

    public void Init(Setting setting)
    {
        if (setting == null)
        {
            Debug.LogError("[UISettingSlider] 초기화 실패");
            return;
        }
        _setting = setting;

        // 슬라이더 이벤트 등록
        _bgm.onValueChanged.AddListener(_setting.SetBGMVolume);
        _sfx.onValueChanged.AddListener(_setting.SetSFXVolume);
        _mouseSensitivity.onValueChanged.AddListener(_setting.SetMouseSensitivity);

        // UI 설정
        _setting.OnBgmChanged += HandleBgmChanged;
        _setting.OnSfxChanged += HandleSfxChanged;
        _setting.OnSensitivityChanged += HandleSensitivityChanged;

        // 초기값을 UI에 적용
        HandleBgmChanged(_setting.BgmVolume);
        HandleSfxChanged(_setting.SfxVolume);
        HandleSensitivityChanged(_setting.MouseSensitivity);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_setting == null) { return; }

        _bgm.onValueChanged.RemoveListener(_setting.SetBGMVolume);
        _sfx.onValueChanged.RemoveListener(_setting.SetSFXVolume);
        _mouseSensitivity.onValueChanged.RemoveListener(_setting.SetMouseSensitivity);

        _setting.OnBgmChanged -= HandleBgmChanged;
        _setting.OnSfxChanged -= HandleSfxChanged;
        _setting.OnSensitivityChanged -= HandleSensitivityChanged;
    }

    void HandleBgmChanged(float value)
    {
        _bgm.SetValueWithoutNotify(value);
    }

    void HandleSfxChanged(float value)
    {
        _sfx.SetValueWithoutNotify(value);
    }

    public void HandleSensitivityChanged(float value)
    {
        _mouseSensitivity.SetValueWithoutNotify(value);
        _mouseText.text = $"마우스 감도: {value}";
    }
}
