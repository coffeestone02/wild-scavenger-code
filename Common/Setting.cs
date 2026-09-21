
using System;
using UnityEngine;

public class Setting : ISettingSave
{
    // 저장되는 세팅값
    public float BgmVolume { get; private set; }
    public float SfxVolume { get; private set; }
    public float MouseSensitivity { get; private set; }

    public event Action<float> OnBgmChanged;
    public event Action<float> OnSfxChanged;
    public event Action<float> OnSensitivityChanged;

    public void SetBGMVolume(float value)
    {
        float output = Mathf.Clamp01(value);
        BgmVolume = output;
        OnBgmChanged?.Invoke(output);
    }

    public void SetSFXVolume(float value)
    {
        float output = Mathf.Clamp01(value);
        SfxVolume = output;
        OnSfxChanged?.Invoke(output);
    }

    public void SetMouseSensitivity(float value)
    {
        float output = Mathf.Clamp(value, 1f, 100f);
        MouseSensitivity = output;
        OnSensitivityChanged?.Invoke(output);
    }

    public SettingData GetSaveData()
    {
        SettingData data = new SettingData();
        data.BgmVolume = BgmVolume;
        data.SfxVolume = SfxVolume;
        data.MouseSensitivity = MouseSensitivity;

        return data;
    }

    public void ApplySaveData(SettingData data)
    {
        if (data == null)
        {
            Debug.LogError("[Setting] 세이브 데이터 적용 실패");
            return;
        }

        SetBGMVolume(data.BgmVolume);
        SetSFXVolume(data.SfxVolume);
        SetMouseSensitivity(data.MouseSensitivity);
    }
}
