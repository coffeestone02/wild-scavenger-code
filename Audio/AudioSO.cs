using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioSO", menuName = "Scriptable Objects/AudioSO")]
public class AudioSO : ScriptableObject
{
    [SerializeField] AudioClip[] _clips;
    [SerializeField] AudioMixerGroup _mixerGroup;

    [Header("Audio Values")]
    [Range(0f, 1f)]
    [SerializeField] float _volume = 1f;
    [Range(0.1f, 3f)]
    [SerializeField] float _minPitch = 1f;
    [Range(0.1f, 3f)]
    [SerializeField] float _maxPitch = 1f;
    [Min(1f)]
    [SerializeField] float _maxDistance = 15f;

    [Header("Flags")]
    [SerializeField] bool _isRandomPitch;
    [SerializeField] bool _is3D;
    [SerializeField] bool _isLoop; // BGM 전용

#if UNITY_EDITOR
    void OnValidate()
    {
        if (HasClip() == false)
        {
            Debug.LogWarning($"{name}: 클립이 비어있음");
        }
        if (_mixerGroup == null)
        {
            Debug.LogWarning($"{name}: 믹서 그룹이 지정되지 않음");
        }
        if (_minPitch > _maxPitch)
        {
            _minPitch = _maxPitch;
        }
    }
#endif

    bool HasClip()
    {
        if (_clips == null || _clips.Length == 0)
        {
            return false;
        }

        return true;
    }

    /// <summary> 여러 개가 있을 경우 랜덤으로 클립을 가져옴 </summary>
    public AudioClip GetClip()
    {
        if (HasClip() == false)
        {
            return null;
        }

        int index = Random.Range(0, _clips.Length);
        return _clips[index];
    }

    /// <summary> Pitch를 랜덤 조정 </summary>
    public float GetPitch()
    {
        return Random.Range(_minPitch, _maxPitch);
    }

    /// <summary> AudioSource 설정을 적용 </summary>
    public void ApplySetting(AudioSource source)
    {
        source.clip = GetClip();
        source.volume = _volume;
        source.outputAudioMixerGroup = _mixerGroup;
        source.spatialBlend = 0f;
        source.pitch = 1f;
        source.loop = _isLoop;
        source.maxDistance = _maxDistance;

        if (_isRandomPitch) // 랜덤 피치 적용
        {
            source.pitch = GetPitch();
        }

        if (_is3D) // 3d 설정
        {
            source.spatialBlend = 1f;
        }
    }
}
