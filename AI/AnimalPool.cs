using UnityEngine;
using UnityEngine.Pool;

public class AnimalPool
{
    Transform _container;
    GameObject _prefab;
    ObjectPool<GameObject> _pool;

    public AnimalPool(GameObject prefab, Transform container, int maxSize)
    {
        _prefab = prefab;
        _container = container;
        _pool = new ObjectPool<GameObject>(
            createFunc: CreatePrefab,
            actionOnRelease: OnReleasePrefab,
            actionOnDestroy: OnDestroyPoolObject,
            maxSize: maxSize
        );
    }

    public GameObject Get()
    {
        return _pool.Get();
    }

    public void Release(GameObject go)
    {
        _pool.Release(go);
    }

    public int GetActiveCount()
    {
        return _pool.CountActive;
    }

    GameObject CreatePrefab()
    {
        return Object.Instantiate(_prefab, _container);
    }

    void OnReleasePrefab(GameObject go)
    {
        go.SetActive(false);
    }

    void OnDestroyPoolObject(GameObject go)
    {
        Object.Destroy(go);
    }
}
