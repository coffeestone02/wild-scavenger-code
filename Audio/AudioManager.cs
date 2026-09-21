using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(BGMPlayer))]
[RequireComponent(typeof(SFXPlayer))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioMixer _mixer;

    BGMPlayer _bgmPlayer;
    SFXPlayer _sfxPlayer;

    void Awake()
    {
        Init();
    }

    void OnDestroy()
    {
        if (Instance == this) // 중복 생성본이 자신을 지우면서 정품 참조까지 날리는 것을 방지
        {
            Instance = null;
        }
    }

    void Init()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _bgmPlayer = GetComponent<BGMPlayer>();
        _sfxPlayer = GetComponent<SFXPlayer>();

        _bgmPlayer.Init(transform);
        _sfxPlayer.Init(transform);

        Managers.Settings.OnBgmChanged += SetBGMVolume;
        Managers.Settings.OnSfxChanged += SetSFXVolume;

        SetBGMVolume(Managers.Settings.BgmVolume);
        SetSFXVolume(Managers.Settings.SfxVolume);
    }

    /// <summary> SFX 재생(2D/3D 여부는 AudioSO의 _is3D로 설정) </summary>
    public void PlaySFX(AudioSO audioSO, Vector3 pos = default)
    {
        _sfxPlayer.PlaySFX(audioSO, pos);
    }

    public void PlayBGM(AudioSO audioSO)
    {
        _bgmPlayer.Play(audioSO);
    }

    public void StopBGM()
    {
        _bgmPlayer.Stop();
    }

    public void SetBGMVolume(float value)
    {
        // 0이 들어갔을 때 -infinity가 되는 것을 방지
        value = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        _mixer.SetFloat("BGM", value);
    }

    public void SetSFXVolume(float value)
    {
        // 0이 들어갔을 때 -infinity가 되는 것을 방지
        value = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        _mixer.SetFloat("SFX", value);
    }
}
