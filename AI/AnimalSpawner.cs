using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class AnimalSpawner : MonoBehaviour
{
    const int maxSize = 20;

    [SerializeField] SpawnArea _spawnArea;
    [SerializeField] Transform _container;
    [SerializeField] GameObject _prefab;
    [SerializeField] Transform _aiTarget;
    [SerializeField] int _count = 1;

    [Header("리스폰 설정")]
    [SerializeField] bool _isLoop = false; // 체크 시 반복하여 스폰시킴
    [SerializeField] float _respawnTime = 10f;

    AnimalPool _pool;
    WaitForSeconds _respawnCooldown;

    void Start()
    {
        _respawnCooldown = new WaitForSeconds(_respawnTime);
        _pool = new AnimalPool(_prefab, _container, maxSize);
        for (int i = 0; i < _count; i++)
        {
            Spawn();
        }

        if (_isLoop)
        {
            StartCoroutine(CoRespawn());
        }
    }

    // 시간마다 체크해서 리스폰시킴
    IEnumerator CoRespawn()
    {
        while (true)
        {
            yield return _respawnCooldown;

            int activeCount = _pool.GetActiveCount();
            if (activeCount < _count)
            {
                for (int i = 0; i < _count - activeCount; i++)
                {
                    Spawn();
                }
            }
        }
    }

    public GameObject Spawn()
    {
        GameObject go = _pool.Get();

        bool found = _spawnArea.TryFindPosition(out Vector3 result);
        if (found) // 위치를 찾음
        {
            NavMeshAgent agent = go.GetComponent<NavMeshAgent>();
            agent.Warp(result);
        }

        // 풀링용 오브젝트면 반환될 스포너를 지정해줌
        IPoolReturnable poolReturnable = go.GetComponent<IPoolReturnable>();
        if (poolReturnable != null)
        {
            poolReturnable.SetReturn(_pool.Release);
        }

        // 타겟이 필요한 경우 주입
        IAITargetReceiver receiver = go.GetComponent<IAITargetReceiver>();
        if (receiver != null)
        {
            receiver.SetTarget(_aiTarget);
        }

        go.SetActive(true);

        return go;
    }

    /// <summary> 특정 Pool에서 활성화된 오브젝트의 개수를 가져옴 </summary>
    public int GetActiveCount()
    {
        return _pool.GetActiveCount();
    }
}
