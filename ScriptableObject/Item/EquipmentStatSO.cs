using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentStatSO", menuName = "Scriptable Objects/EquipmentStatSO")]
public class EquipmentStatSO : ItemSO
{
    [SerializeField] Define.EEquipmentType _equipmentType;
    [SerializeField] float _attack;
    [SerializeField] float _defense;
    [SerializeField] float _attackCooldown;
    [SerializeField] float _attackRange;

    public Define.EEquipmentType EquipmentType => _equipmentType;
    public float Attack => _attack;
    public float Defense => _defense;
    public float AttackCooldown => _attackCooldown;
    public float AttackRange => _attackRange;
}
