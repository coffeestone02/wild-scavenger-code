using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] int _spawnCount;
    [SerializeField] SpawnArea[] _spawnArea;
    [SerializeField] Transform _container;
    [SerializeField] GameObject[] _entries;

    void Start()
    {
        Init();
    }

    void Init()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            int randomSpawnIdx = Random.Range(0, _spawnArea.Length);
            int randomItemIdx = Random.Range(0, _entries.Length);
            if (_spawnArea[randomSpawnIdx].TryFindSpawnPosition(out Vector3 position, out Vector3 normal))
            {
                GameObject go = Instantiate(_entries[randomItemIdx], _container);
                PlaceItem(go, position, normal);
            }
        }
    }

    void PlaceItem(GameObject go, Vector3 position, Vector3 normal)
    {
        go.transform.position = position + normal;

        // 랜덤 회전
        float yaw = Random.Range(0f, 360f);
        Quaternion rotation = Quaternion.Euler(0f, yaw, 0f);
        go.transform.rotation = rotation;
    }
}
