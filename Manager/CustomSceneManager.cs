using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomSceneManager : MonoBehaviour
{
    public static CustomSceneManager Instance { get; private set; }

    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Image _progressBar;
    [SerializeField] float _fadeSpeed = 1f;

    string _nextSceneName;
    bool _isLoading = false;

    public event Action OnBeforeSceneLoaded;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void HandleSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == _nextSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }
    }

    IEnumerator Fade(bool isFadeIn)
    {
        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.unscaledDeltaTime / _fadeSpeed;
            if (isFadeIn)
            {
                _canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);
            }
            else
            {
                _canvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);
            }

            yield return null;
        }

        if (isFadeIn == false) // 전환이 끝남
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
            _isLoading = false;
        }
        else
        {
            _canvasGroup.alpha = 1f;
        }
    }

    IEnumerator LoadSceneProgress()
    {
        _progressBar.fillAmount = 0.0f;

        yield return StartCoroutine(Fade(true)); // fade가 끝날 때 까지 대기

        AsyncOperation op = SceneManager.LoadSceneAsync(_nextSceneName);
        op.allowSceneActivation = false;

        float progress = 0f;
        while (op.isDone == false) // 씬 로드가 끝나지 않았다면
        {
            yield return null;

            if (op.progress < 0.9f) // 90% 미만이면 실제 로딩률만큼 보여주고 나머지 10% 자연스럽게 차오르는 연출
            {
                _progressBar.fillAmount = op.progress;
            }
            else
            {
                progress += Time.deltaTime * 5.0f;
                _progressBar.fillAmount = Mathf.Lerp(0.9f, 1.0f, progress);

                if (progress > 1.0f) // 로딩 완료 시 씬 활성화
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    /// <summary> 씬을 불러옴 </summary>
    public void LoadScene(string sceneName)
    {
        if (_isLoading) { return; }

        _isLoading = true;

        StopAllCoroutines(); // 이전 전환의 페이드가 남아있으면 정리

        gameObject.SetActive(true);
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;

        SceneManager.sceneLoaded -= HandleSceneLoaded; // 중복 구독 방지
        SceneManager.sceneLoaded += HandleSceneLoaded;

        _nextSceneName = sceneName;

        Managers.UI.Clear();
        OnBeforeSceneLoaded?.Invoke();

        StartCoroutine(LoadSceneProgress());
    }
}
