using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatSO", menuName = "Scriptable Objects/EnemyStatSO")]
public class EnemyStatSO : ScriptableObject
{
    [SerializeField] AIHealthStat _health = new AIHealthStat();
    [SerializeField] AIMoveStat _move = new AIMoveStat();
    [SerializeField] AIChaseStat _chase = new AIChaseStat();
    [SerializeField] AIAttackStat _attack = new AIAttackStat();

    public AIHealthStat Health => _health;
    public AIMoveStat Move => _move;
    public AIChaseStat Chase => _chase;
    public AIAttackStat Attack => _attack;
}
