using UnityEngine;

public class AttackState : IAIState
{
    // AI
    EnemyAIController _controller;
    Transform _transform;
    AIAttackStat _stat;
    AIAudio _audio;

    public AttackState(EnemyAIController controller, AIAttackStat stat, AIAudio audio)
    {
        _controller = controller;
        _transform = controller.transform;
        _stat = stat;
        _audio = audio;
    }

    public void Enter()
    {
        Attack();
        _audio.PlayCrySound(); // 소리 재생
    }

    public void Tick()
    {
        if (_controller.IsAttackReady == false) { return; }

        _controller.ChangeState(_controller.Chase); // 공격 후 일정시간 동안 대기하고 Chaes 상태로 전이
    }

    public void Exit()
    {

    }

    void Attack()
    {
        _controller.StartAttackCooldown();
        _controller.Anim.Play("Attack");

        AttackerInfo info = new AttackerInfo(_stat.AttackDamage, _transform.gameObject);

        Transform target = _controller.Target;
        if (target != null)
        {
            Health targetHealth = target.GetComponentInParent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(info);
            }
        }
    }
}
