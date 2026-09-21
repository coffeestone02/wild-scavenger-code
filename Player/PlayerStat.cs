using System;
using UnityEngine;

public class PlayerStat : MonoBehaviour, ICombat, IDamageCalculator
{
    [SerializeField] float _maxHP;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotateSpeed;
    [SerializeField] float _aimMoveSpeed;
    [SerializeField] float _interactRange;

    public float MaxHP => _maxHP;
    public float MoveSpeed => _moveSpeed;
    public float RotateSpeed => _rotateSpeed;
    public float AimMoveSpeed => _aimMoveSpeed;
    public float InteractRange => _interactRange;

    public float Attack { get; private set; }
    public float Defense { get; private set; }

    public event Action OnDefenseChanged;

    public void SetAttackStat(EquipmentStatSO weapon)
    {
        if (weapon == null) { return; }

        Attack = weapon.Attack;
    }

    public void ClearAttackStat()
    {
        Attack = 0;
    }

    public void SetDefenseStat(InventorySlotModel[] armorSlots)
    {
        float defense = 0;
        foreach (InventorySlotModel slot in armorSlots)
        {
            if (slot.IsEmpty == false)
            {
                EquipmentStatSO armor = slot.SlotItem.GetEquipmentStatSO();
                defense += armor.Defense;
            }
        }

        Defense = defense;
        OnDefenseChanged?.Invoke();
    }

    /// <summary> 플레이어 방어력에 따라 받을 피해량 계산 </summary>
    public float CalculateDamage(AttackerInfo info)
    {
        float damage = info.Damage - Defense;
        if (damage < 0)
        {
            damage = 0;
        }

        return damage;
    }
}
