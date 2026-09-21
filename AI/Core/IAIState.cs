
public interface IAIState
{
    /// <summary> 처음 진입 시 1회 실행 </summary>
    void Enter();
    /// <summary> 매 프레임마다 실행하며 다른 상태로 전이하는 코드를 작성 </summary>
    void Tick();
    /// <summary> 상태를 종료할 때 1회 실행 </summary>
    void Exit();
}
