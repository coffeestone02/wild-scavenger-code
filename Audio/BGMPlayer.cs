using System.Collections;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] float _crossFadeDuration = 1.5f;

    AudioSource _sourceA;
    AudioSource _sourceB;

    AudioSource _activeSource;
    AudioSO _currentSO;
    Coroutine _coFade;

    public void Init(Transform parent)
    {
        // _sourceA 설정
        GameObject a = new GameObject("@BGM_SourceA");
        _sourceA = a.AddComponent<AudioSource>();
        _sourceA.playOnAwake = false;
        a.transform.SetParent(parent);

        // _sourceB 설정
        GameObject b = new GameObject("@BGM_SourceB");
        _sourceB = b.AddComponent<AudioSource>();
        _sourceB.playOnAwake = false;
        b.transform.SetParent(parent);

        // 시작 소스는 _sourceA
        _activeSource = _sourceA;
    }

    AudioSource GetEmptySource()
    {
        if (_activeSource == _sourceA)
        {
            return _sourceB;
        }

        return _sourceA;
    }

    IEnumerator CoCrossFade(AudioSource from, AudioSource to)
    {
        float fromStartVolume = from.volume;
        float toTargetVolume = to.volume;

        // to를 0부터 시작하여 서서히 증가
        to.volume = 0f;
        to.Play();

        float elapsed = 0f;

        while (elapsed < _crossFadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _crossFadeDuration;

            from.volume = Mathf.Lerp(fromStartVolume, 0f, t); // 0까지 서서히 감소
            to.volume = Mathf.Lerp(0f, toTargetVolume, t); // 목표 볼륨까지 서서히 증가

            yield return null;
        }

        // from 종료
        from.volume = 0f;
        from.Stop();

        to.volume = toTargetVolume;

        _coFade = null;
    }

    IEnumerator CoFadeOutAll()
    {
        float aStartVolume = _sourceA.volume;
        float bStartVolume = _sourceB.volume;
        float elapsed = 0f;

        while (elapsed < _crossFadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _crossFadeDuration;

            _sourceA.volume = Mathf.Lerp(aStartVolume, 0f, t);
            _sourceB.volume = Mathf.Lerp(bStartVolume, 0f, t);

            yield return null;
        }

        _sourceA.Stop();
        _sourceB.Stop();

        _coFade = null;
    }

    // 다른 곡이면 크로스페이드로 전환하고 같은 곡이면 무시함
    public void Play(AudioSO audioSO)
    {
        if (audioSO == null || audioSO == _currentSO) { return; }

        // 현재 재생 SO를 audioSO로 지정하고 빈 소스를 가져와 세팅값을 적용시킴
        _currentSO = audioSO;
        AudioSource nextSource = GetEmptySource();
        audioSO.ApplySetting(nextSource);

        if (_coFade != null)
        {
            StopCoroutine(_coFade);
        }

        _coFade = StartCoroutine(CoCrossFade(_activeSource, nextSource));
        _activeSource = nextSource;
    }

    public void Stop()
    {
        if (_currentSO == null) { return; } // 멈춰있는 상태면 무시

        // 현재 재생 SO를 비우고 동작중인 코루틴은 중지한다
        _currentSO = null;
        if (_coFade != null)
        {
            StopCoroutine(_coFade);
        }

        _coFade = StartCoroutine(CoFadeOutAll());
    }
}
