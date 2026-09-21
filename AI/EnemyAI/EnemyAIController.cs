using UnityEngine;

public class EnemyAIController : AIControllerBase, IAITargetReceiver
{
    [Header("Stat")]
    [SerializeField] EnemyStatSO _stat;

    // 스탯
    public override AIHealthStat HealthStat => _stat.Health;
    public override AIMoveStat MoveStat => _stat.Move;
    public AIChaseStat ChaseStat => _stat.Chase;
    public AIAttackStat AttackStat => _stat.Attack;

    // 상태
    public ChaseState Chase { get; private set; }
    public AttackState Attack { get; private set; }

    public Transform Target { get; private set; }
    public bool IsAttackReady => Time.time >= _nextAttackTime;

    float _nextAttackTime;

    protected override void OnEnable()
    {
        base.OnEnable();

        _nextAttackTime = 0f;
    }

    protected override void Update()
    {
        base.Update();

        // 타겟이 없으면 작동하지 않음
        if (Target == null) { return; }
        if (DetectTarget() == false) { return; }

        // Idle, Patrol은 추적가능상태
        switch (StateMachine.CurrentState)
        {
            case IdleState:
            case PatrolState:
                ChangeState(Chase);
                break;
            default:
                break;
        }
    }

    // 타겟이 추적 범위에 있는지 감지함
    bool DetectTarget()
    {
        if (Agent.pathPending) { return false; } // 경로 계산 중이면 종료

        float distance = Vector3.Distance(transform.position, Target.position);
        return distance <= ChaseStat.ChaseDistance;
    }

    protected override void HandleDamaged(AttackerInfo info)
    {
        base.HandleDamaged(info);

        if (Health.IsDead) { return; }
        if (StateMachine.CurrentState == Attack) { return; } // 공격 중엔 끊지 않아 쿨다운 우회를 막음

        ChangeState(Chase);
    }

    protected override void AdditionalStates()
    {
        Chase = new ChaseState(this, ChaseStat);
        Attack = new AttackState(this, AttackStat, _audio);
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }

    public void StartAttackCooldown()
    {
        _nextAttackTime = Time.time + AttackStat.AttackCooldown;
    }
}
