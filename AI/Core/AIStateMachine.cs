
public class AIStateMachine
{
    public IAIState CurrentState { get; private set; }

    /// <summary> 현재 상태를 종료하고 새 상태로 변경 </summary>
    public void ChangeState(IAIState newState)
    {
        if (newState == null || CurrentState == newState) // 같은 상태 재전이 방지
        {
            return;
        }

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    /// <summary> 매 프레임 실행 </summary>
    public void Tick()
    {
        CurrentState?.Tick();
    }

    public void Clear()
    {
        CurrentState?.Exit();
        CurrentState = null;
    }
}
