using UnityEngine;
using UnityEngine.Pool;

public class EffectPool
{
    const int _initSize = 5;
    const int _maxSize = 30;

    EffectEntry _entry;
    Transform _container;

    ObjectPool<GameObject> _pool;
    public WaitForSeconds WaitLifetime { get; private set; }

    public EffectPool(EffectEntry entry, Transform container)
    {
        _entry = entry;
        _container = container;

        _pool = new ObjectPool<GameObject>(
            CreatePooledItem,
            OnGetFromPool,
            OnReleasePrefab,
            OnDestroyPoolObject,
            maxSize: _maxSize
            );

        // 미리 초기 사이즈만큼 만들어놓기
        GameObject[] temp = new GameObject[_initSize];
        for (int i = 0; i < _initSize; i++)
        {
            temp[i] = _pool.Get();
        }

        for (int i = 0; i < _initSize; i++)
        {
            _pool.Release(temp[i]);
        }

        WaitLifetime = new WaitForSeconds(entry.Lifetime);
    }

    public GameObject Get()
    {
        return _pool.Get();
    }

    public void Release(GameObject go)
    {
        _pool.Release(go);
    }

    // 생성
    GameObject CreatePooledItem()
    {
        GameObject instance = Object.Instantiate(_entry.Prefab, _container);
        return instance;
    }

    // 오브젝트 가져오기
    void OnGetFromPool(GameObject instance)
    {
        instance.SetActive(true);
    }

    // 오브젝트를 풀 영역에 반환하기
    void OnReleasePrefab(GameObject instance)
    {
        instance.SetActive(false);
    }

    // 오브젝트를 풀 영역에서 제거하기
    void OnDestroyPoolObject(GameObject instance)
    {
        Object.Destroy(instance);
    }
}
