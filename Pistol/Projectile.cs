using UnityEngine;
using System;
using System.Collections;

public class Projectile : MonoBehaviour, IPoolReturnable
{
    [SerializeField] float _lifetime = 3f;

    Action<GameObject> _onReturn;
    Coroutine _lifetimeRoutine;
    WaitForSeconds _waitLifetime;
    bool _isReturned; // 다중 호출 방지용

    void Awake()
    {
        _waitLifetime = new WaitForSeconds(_lifetime);
    }

    void OnEnable()
    {
        _isReturned = false;
        _lifetimeRoutine = StartCoroutine(CoLifetime());
    }

    void OnDisable()
    {
        if (_lifetimeRoutine != null)
        {
            StopCoroutine(_lifetimeRoutine);
            _lifetimeRoutine = null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Return();
    }

    IEnumerator CoLifetime()
    {
        yield return _waitLifetime;

        Return();
    }

    void Return()
    {
        if (_isReturned) { return; }

        _isReturned = true;
        _onReturn?.Invoke(gameObject);
    }

    public void SetReturn(Action<GameObject> onReturn)
    {
        _onReturn = onReturn;
    }
}
