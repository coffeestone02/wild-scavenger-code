using UnityEngine;

[CreateAssetMenu(fileName = "FriendlyStatSO", menuName = "Scriptable Objects/FriendlyStatSO")]
public class FriendlyStatSO : ScriptableObject
{
    [SerializeField] AIHealthStat _health = new AIHealthStat();
    [SerializeField] AIMoveStat _move = new AIMoveStat();
    [SerializeField] AIFleeStat _flee = new AIFleeStat();

    public AIHealthStat Health => _health;
    public AIMoveStat Move => _move;
    public AIFleeStat Flee => _flee;
}
