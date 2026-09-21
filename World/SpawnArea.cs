using UnityEngine;
using UnityEngine.AI;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] float _findingRadius = 10f;
    [SerializeField] float _rayHeight = 50f;
    [SerializeField] LayerMask _ground;

    int _attemptLimit = 10;

    public bool TryFindPosition(out Vector3 result)
    {
        // 일정 횟수 이상 못 찾으면 탐색 종료
        for (int i = 0; i < _attemptLimit; i++)
        {
            // 랜덤한 방향 벡터를 생성 후 스폰 장소에 더해서 위치를 찾음
            Vector2 randomCircle = Random.insideUnitCircle * _findingRadius;
            Vector3 tPos = transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

            // 랜덤 위치가 NavMesh 위에 있으면 목적지 갱신 후 종료
            NavMeshHit hit;
            bool pos = NavMesh.SamplePosition(tPos, out hit, _findingRadius, NavMesh.AllAreas);
            if (pos)
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    // 래이캐스트
    public bool TryFindSpawnPosition(out Vector3 position, out Vector3 normal)
    {
        // 일정 횟수 이상 못 찾으면 탐색 종료
        for (int i = 0; i < _attemptLimit; i++)
        {
            // 랜덤한 방향 벡터를 생성 후 스폰 장소에 더해서 위치를 찾음
            Vector2 randomCircle = Random.insideUnitCircle * _findingRadius;
            Vector3 tPos = transform.position + new Vector3(randomCircle.x, _rayHeight, randomCircle.y);

            // 바닥을 찾아서 위치와 노말 벡터를 반환
            if (Physics.Raycast(tPos, Vector3.down, out RaycastHit hit, _rayHeight * 2f, _ground))
            {
                position = hit.point;
                normal = hit.normal;
                return true;
            }
        }

        normal = Vector3.up;
        position = Vector3.zero;
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _findingRadius);
    }
}
