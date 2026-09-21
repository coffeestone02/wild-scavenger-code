using UnityEngine;

public class FriendlyAIController : AIControllerBase
{
    [Header("Stat")]
    [SerializeField] FriendlyStatSO _stat;

    // 스탯
    public override AIHealthStat HealthStat => _stat.Health;
    public override AIMoveStat MoveStat => _stat.Move;
    public AIFleeStat FleeStat => _stat.Flee;

    // 상태
    public FleeState Flee { get; private set; }

    // 공격받았을 때 자동으로 호출
    protected override void HandleDamaged(AttackerInfo info)
    {
        base.HandleDamaged(info);

        if (Health.CurrentHP > 0)
        {
            ChangeState(Flee);
        }
    }

    protected override void AdditionalStates()
    {
        Flee = new FleeState(this, FleeStat, _audio);
    }
}
