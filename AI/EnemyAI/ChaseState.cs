using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IAIState
{
    // AI
    EnemyAIController _controller;
    NavMeshAgent _agent;
    Transform _transform;
    AIChaseStat _stat;

    public ChaseState(EnemyAIController controller, AIChaseStat stat)
    {
        _controller = controller;
        _agent = controller.Agent;
        _transform = controller.transform;
        _stat = stat;
    }

    public void Enter()
    {
        _agent.isStopped = false;
        _agent.speed = _stat.ChaseSpeed;

        _controller.Anim.Play("Run");
    }


    public void Tick()
    {
        Transform target = _controller.Target;

        if (target == null)
        {
            _controller.ChangeState(_controller.Idle);
            return;
        }

        _agent.SetDestination(target.position);
        _controller.SmoothRotate(target.position, _controller.MoveStat.RotateSpeed);

        if (CanAttack(target)) // 공격 범위에 들어오면 공격 상태로 전이
        {
            _controller.ChangeState(_controller.Attack);
            return;
        }

        if (MissingTarget(target)) // 추적 범위보다 멀리가면 Idle로 전이
        {
            _controller.ChangeState(_controller.Idle);
        }
    }

    public void Exit()
    {

    }

    bool MissingTarget(Transform target)
    {
        float distance = Vector3.Distance(_transform.position, target.position);
        return distance > _stat.LoseTargetDistance;
    }

    bool CanAttack(Transform target)
    {
        if (_agent.pathPending) { return false; } // 경로 계산 중이면 종료
        if (_controller.IsAttackReady == false) { return false; } // 쿨다운 중이면 사거리 안이어도 대기

        float distance = Vector3.Distance(_transform.position, target.position);
        return distance <= _controller.AttackStat.AttackRange;
    }
}
