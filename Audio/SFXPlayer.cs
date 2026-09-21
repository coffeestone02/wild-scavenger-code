using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class SFXPlayer : MonoBehaviour
{
    Transform _poolContainer;
    ObjectPool<AudioSource> _sfxPool;

    const int _initSize = 10;
    const int _maxSize = 30;

    #region ObjectPool

    public void Init(Transform parent)
    {
        _poolContainer = new GameObject("@AudioPoolContainer").transform;
        _poolContainer.SetParent(parent);

        _sfxPool = new ObjectPool<AudioSource>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            maxSize: _maxSize
            );

        // 미리 초기 사이즈만큼 만들어놓기
        AudioSource[] temp = new AudioSource[_initSize];
        for (int i = 0; i < _initSize; i++)
        {
            temp[i] = _sfxPool.Get();
        }

        for (int i = 0; i < _initSize; i++)
        {
            _sfxPool.Release(temp[i]);
        }
    }

    // 생성
    AudioSource CreatePooledItem()
    {
        GameObject go = new GameObject($"SFX_Source");
        go.transform.SetParent(_poolContainer);

        AudioSource source = go.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.spatialBlend = 0f;

        return source;
    }

    // 오브젝트 가져오기
    void OnTakeFromPool(AudioSource source)
    {
        source.gameObject.SetActive(true);
    }

    // 오브젝트를 풀 영역에 반환하기
    void OnReturnedToPool(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        source.pitch = 1f;
        source.volume = 1f;
        source.spatialBlend = 0f;
        source.maxDistance = 500f;
        source.outputAudioMixerGroup = null;
        source.transform.position = Vector3.zero;
        source.gameObject.SetActive(false);
    }

    // 오브젝트를 풀 영역에서 제거하기
    void OnDestroyPoolObject(AudioSource source)
    {
        if (source != null)
        {
            Destroy(source.gameObject);
        }
    }

    #endregion

    // 클립 재생 길이만큼 대기 후 풀에 반환
    IEnumerator CoRelease(AudioSource source)
    {
        float time = source.clip.length / Mathf.Max(0.01f, source.pitch); // 피치가 바뀌면 클립의 재생 시간도 바뀜
        yield return new WaitForSecondsRealtime(time);
        _sfxPool.Release(source);
    }

    // 풀에서 오디오 소스를 가져옴
    AudioSource GetReadySource(AudioSO audioSO)
    {
        if (audioSO == null) { return null; }

        // 풀에서 소스를 가져오고 세팅값을 적용
        AudioSource source = _sfxPool.Get();
        audioSO.ApplySetting(source);

        if (source.clip == null)
        {
            _sfxPool.Release(source);
            return null;
        }

        return source;
    }

    /// <summary> SFX 재생 </summary>
    public void PlaySFX(AudioSO audioSO, Vector3 pos = default)
    {
        AudioSource source = GetReadySource(audioSO);
        if (source == null) { return; }

        source.transform.position = pos; // 2D면 무시
        source.Play();

        StartCoroutine(CoRelease(source)); // 재생 끝나면 풀에 반환
    }
}
