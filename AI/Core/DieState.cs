using UnityEngine;

public class DieState : IAIState
{
    AIControllerBase _controller;

    float _waiting;
    float _despawnDelay = 3f;

    public DieState(AIControllerBase controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        _controller.StopAgent();

        _controller.Agent.enabled = false;
        _controller.AICollider.enabled = false;

        _controller.Anim.Play("Die");
        _waiting = _despawnDelay + Time.time;
    }

    public void Tick()
    {
        if (_waiting > Time.time) { return; }

        // 아이템 드랍
        foreach (Item item in _controller.Drops)
        {
            Util.ItemSpawn(item, _controller.transform.position);
        }

        _controller.Despawn();
    }

    public void Exit()
    {

    }
}
