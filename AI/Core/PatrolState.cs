using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IAIState
{
    AIControllerBase _controller;
    NavMeshAgent _agent;
    Transform _transform;
    AIMoveStat _stat;
    AIAudio _audio;

    Vector3 _destination;
    int _attemptLimit = 10;

    public PatrolState(AIControllerBase controller, AIMoveStat stat, AIAudio audio)
    {
        _controller = controller;
        _agent = controller.Agent;
        _transform = controller.transform;
        _stat = stat;
        _audio = audio;
    }

    public void Enter()
    {
        bool found = TryFindRandomDestination(out _destination);

        if (found == false) // 유효 목적지를 찾지 못하면 Idle로 전이
        {
            _controller.ChangeState(_controller.Idle);
            return;
        }

        _agent.isStopped = false;
        _agent.speed = _stat.MoveSpeed;
        _agent.SetDestination(_destination);

        // 애니메이션 설정
        _controller.Anim.Play("Move");

        // 소리 재생
        _audio.PlayCrySound();
    }

    public void Tick()
    {
        _controller.SmoothRotate(_destination, _stat.RotateSpeed); // 회전

        if (HasReachedDestination()) // 목적지에 도달하면 Idle로 전이
        {
            _controller.ChangeState(_controller.Idle);
            return;
        }
    }

    public void Exit()
    {

    }

    // 활동 반경 안에서 갈 수 있는 랜덤 목적지를 찾음
    bool TryFindRandomDestination(out Vector3 result)
    {
        // 일정 횟수 이상 못 찾으면 탐색 종료
        for (int i = 0; i < _attemptLimit; i++)
        {
            // 랜덤한 방향 벡터를 생성 후 현재 장소에 더해서 위치를 찾음
            Vector2 randomCircle = Random.insideUnitCircle * _stat.PatrolRadius;
            Vector3 tPos = _transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

            // 랜덤 위치가 NavMesh 위에 있으면 목적지 갱신 후 종료
            NavMeshHit hit;
            bool pos = NavMesh.SamplePosition(tPos, out hit, _stat.PatrolRadius, NavMesh.AllAreas);
            if (pos)
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    bool HasReachedDestination()
    {
        if (_agent.pathPending) // 경로 계산 중이면 종료
        {
            return false;
        }

        float distance = Vector3.Distance(_transform.position, _destination);
        return distance <= 0.5f;
    }
}
