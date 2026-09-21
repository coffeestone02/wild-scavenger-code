using UnityEngine;

public class IdleState : IAIState
{
    AIControllerBase _controller;
    AIMoveStat _stat;
    float _idleTimer;

    public IdleState(AIControllerBase controller, AIMoveStat stat)
    {
        _controller = controller;
        _stat = stat;
    }

    public void Enter()
    {
        _idleTimer = 0f;

        _controller.StopAgent();

        // 애니메이션 설정
        _controller.Anim.Play("Idle");
    }

    public void Tick()
    {
        // IdleDuration만큼 대기 후 Patrol 상태로 전이
        _idleTimer += Time.deltaTime;

        if (_idleTimer >= _stat.IdleDuration)
        {
            _controller.ChangeState(_controller.Patrol);
        }
    }

    public void Exit()
    {
        _idleTimer = 0f;
    }
}
