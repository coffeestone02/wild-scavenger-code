using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public abstract class AIControllerBase : MonoBehaviour, IPoolReturnable
{
    [Header("Audio")]
    [SerializeField] protected AIAudio _audio;

    [Header("Settings")]
    [SerializeField] Item[] _drops; // 죽었을 때 필드에 드랍할 아이템들

    protected AIStateMachine StateMachine { get; private set; }
    public Item[] Drops => _drops;

    // 컴포넌트
    public Collider AICollider { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Health Health { get; private set; }
    public Animator Anim { get; private set; }

    // 스탯
    public abstract AIHealthStat HealthStat { get; }
    public abstract AIMoveStat MoveStat { get; }

    // 공통 상태
    public IdleState Idle { get; private set; }
    public PatrolState Patrol { get; private set; }
    public DieState Die { get; private set; }

    public Vector3 LastAttackerPosition { get; private set; }

    Action<GameObject> _onReturn;

    protected abstract void AdditionalStates();

    protected virtual void Awake()
    {
        AICollider = GetComponent<Collider>();
        Agent = GetComponent<NavMeshAgent>();
        Health = GetComponent<Health>();
        Anim = GetComponent<Animator>();

        StateMachine = new AIStateMachine();

        // 공통 상태
        Idle = new IdleState(this, MoveStat);
        Patrol = new PatrolState(this, MoveStat, _audio);
        Die = new DieState(this);

        AdditionalStates();
    }

    protected virtual void OnEnable()
    {
        Health.OnDead += HandleDie;
        Health.OnDamaged += HandleDamaged;

        Agent.enabled = true;
        AICollider.enabled = true;
        Health.Init(HealthStat.MaxHealth);
        ChangeState(Idle);
    }

    protected virtual void Update()
    {
        StateMachine.Tick();
    }

    protected virtual void OnDisable()
    {
        Health.OnDead -= HandleDie;
        Health.OnDamaged -= HandleDamaged;

        StateMachine.Clear();
    }

    protected virtual void OnDespawn()
    {
        _onReturn?.Invoke(gameObject);
    }

    void HandleDie()
    {
        StateMachine.ChangeState(Die);
    }

    protected virtual void HandleDamaged(AttackerInfo info)
    {
        if (info == null) // 공격자 정보가 없으면 바라보는 방향으로 설정
        {
            LastAttackerPosition = transform.position + transform.forward;
        }
        else
        {
            LastAttackerPosition = info.Attacker.transform.position;
        }
    }

    /// <summary> 현재 상태를 종료하고 새 상태로 변경 </summary>
    public void ChangeState(IAIState newState)
    {
        StateMachine.ChangeState(newState);
    }

    public void StopAgent()
    {
        // agent 정지 및 경로 초기화
        Agent.isStopped = true;
        Agent.velocity = Vector3.zero;
        Agent.updateRotation = false;
        Agent.ResetPath();
    }

    public void SmoothRotate(Vector3 destination, float rotateSpeed)
    {
        transform.forward = Vector3.Lerp(transform.forward, destination - transform.position, rotateSpeed * Time.deltaTime);
    }

    public void SetReturn(Action<GameObject> onReturn)
    {
        _onReturn = onReturn;
    }

    /// <summary> 기본 방식은 풀링이지만, 풀링 미사용 시 OnDespawn을 오버라이드 하여 Destroy </summary>
    public void Despawn()
    {
        OnDespawn();
    }
}
