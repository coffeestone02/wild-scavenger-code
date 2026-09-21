using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[Serializable]
public class EffectEntry
{
    public Define.EEffectType Type;
    public GameObject Prefab;
    public float Lifetime = 2f;
}

public class EffectPlayer : MonoBehaviour
{
    public static EffectPlayer Instance { get; private set; }

    [SerializeField] List<EffectEntry> _effectEntries = new List<EffectEntry>(); // 인스펙터 할당
    Dictionary<Define.EEffectType, EffectPool> _effectPools = new Dictionary<Define.EEffectType, EffectPool>();
    Transform _poolContainer;

    void Awake()
    {
        Init();
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Init()
    {
        Instance = this;
        _poolContainer = new GameObject("@EffectPoolContainer").transform;

        foreach (EffectEntry entry in _effectEntries)
        {
            if (entry.Prefab == null || _effectPools.ContainsKey(entry.Type))
            {
                continue;
            }

            EffectPool pool = new EffectPool(entry, _poolContainer);
            _effectPools.Add(entry.Type, pool);
        }
    }

    // 수명시간만큼 기다린 후 풀에 반환함
    IEnumerator CoRelease(EffectPool effectPool, GameObject instance)
    {
        yield return effectPool.WaitLifetime;

        effectPool.Release(instance);
    }

    public void Play(Define.EEffectType type, Vector3 position, Quaternion rotation)
    {
        if (_effectPools.TryGetValue(type, out EffectPool effectPool) == false) { return; }

        GameObject instance = effectPool.Get();
        instance.transform.SetPositionAndRotation(position, rotation);

        StartCoroutine(CoRelease(effectPool, instance));
    }

    public void PlayTrail(Define.EEffectType type, Vector3 start, Vector3 dir, float speed)
    {
        if (dir.sqrMagnitude < 0.0001f) { return; } // 영벡터 방지
        dir = dir.normalized; // 정규화

        if (_effectPools.TryGetValue(type, out EffectPool effectPool) == false) { return; }

        GameObject instance = effectPool.Get();
        Quaternion rotation = Quaternion.LookRotation(dir);
        instance.transform.SetPositionAndRotation(start, rotation); // 위치, 방향 설정

        // 충돌 시 조기 리턴이 필요하면 IPoolReturnable 구현체에서 반환하도록 함
        if (instance.TryGetComponent(out IPoolReturnable poolReturnable))
        {
            poolReturnable.SetReturn(effectPool.Release);
        }
        else
        {
            StartCoroutine(CoRelease(effectPool, instance));
        }

        // 위치 보간으로 인한 잔상 방지
        if (instance.TryGetComponent(out Rigidbody trailRigid))
        {
            trailRigid.position = start;
            trailRigid.rotation = rotation;
            trailRigid.linearVelocity = dir * speed;
            trailRigid.angularVelocity = Vector3.zero;
        }

        if (instance.TryGetComponent(out TrailRenderer trail)) // 잔상 방지
        {
            trail.Clear();
        }
    }
}
