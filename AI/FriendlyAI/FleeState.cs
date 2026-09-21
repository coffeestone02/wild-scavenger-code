using UnityEngine;
using UnityEngine.AI;

public class FleeState : IAIState
{
    FriendlyAIController _controller;
    NavMeshAgent _agent;
    Transform _transform;
    AIFleeStat _stat;
    AIAudio _audio;

    float _fleeTimer;
    Vector3 _destination;
    Vector3 _attackerDir;
    int _attemptLimit = 10;

    public FleeState(FriendlyAIController controller, AIFleeStat stat, AIAudio audio)
    {
        _controller = controller;
        _agent = controller.Agent;
        _transform = controller.transform;
        _stat = stat;
        _audio = audio;
    }

    public void Enter()
    {
        _fleeTimer = 0f;

        _attackerDir = _controller.LastAttackerPosition;
        bool found = TryFindFleeDestination(out _destination);

        // 갈 수 있는 위치가 없으면 Idle로 전이
        if (found == false)
        {
            _controller.ChangeState(_controller.Idle);
            return;
        }

        // NavMeshAgent 설정
        _agent.isStopped = false;
        _agent.speed = _stat.FleeSpeed;
        _agent.SetDestination(_destination);

        _controller.Anim.Play("Run");

        // 소리 재생
        _audio.PlayCrySound();
    }

    public void Tick()
    {
        _fleeTimer += Time.deltaTime;
        _controller.SmoothRotate(_destination, _controller.MoveStat.RotateSpeed);

        if (HasReachedDestination()) // 목적지에 도착하면 다시 찾기
        {
            bool found = TryFindFleeDestination(out _destination);

            // 갈 수 있는 위치가 없으면 Idle로 전이
            if (found == false)
            {
                _controller.ChangeState(_controller.Idle);
                return;
            }

            _agent.SetDestination(_destination);
        }

        // FleeDuration 시간 이상 도주했을 때 Idle로 전이
        if (_fleeTimer >= _stat.FleeDuration)
        {
            _controller.ChangeState(_controller.Idle);
            return;
        }
    }

    public void Exit()
    {
        _fleeTimer = 0f;
        _agent.speed = _controller.MoveStat.MoveSpeed; // 원래 속도로 복구
    }

    // 공격자 반대 방향에서 갈 수 있는 랜덤 목적지를 찾음
    bool TryFindFleeDestination(out Vector3 result)
    {
        // 공격자 반대 방향
        Vector3 fleeDirection = (_transform.position - _attackerDir).normalized;

        for (int i = 0; i < _attemptLimit; i++)
        {
            // angleOffsetRange 범위에서 랜덤하게 회전시켜 무작위성 부여
            float angleOffset = Random.Range(-_stat.ViewAngle, _stat.ViewAngle);

            // 현재 위치에 랜덤 위치를 더해서 목적지를 찾음
            Vector3 direction = Quaternion.Euler(0f, angleOffset, 0f) * fleeDirection;
            Vector3 tPos = _transform.position + direction * _stat.FleeDistance;

            // 랜덤 위치가 NavMesh 위에 있으면 목적지 갱신 후 종료
            NavMeshHit hit;
            bool pos = NavMesh.SamplePosition(tPos, out hit, _stat.FleeDistance, NavMesh.AllAreas);
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
