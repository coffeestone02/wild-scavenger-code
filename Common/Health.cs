using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    IDamageCalculator _damageCalculator = null;

    public float MaxHP { get; private set; }
    public float CurrentHP { get; private set; }
    public bool IsDead => CurrentHP <= 0;

    public event Action<AttackerInfo> OnDamaged;
    public event Action OnDead;

    // 플레이어의 경우 장비에 따라 스탯이 바뀌기 때문에 데미지 계산용 인터페이스를 주입함
    public void Init(float maxHP, IDamageCalculator damageCalculator = null)
    {
        MaxHP = maxHP;
        CurrentHP = MaxHP;
        _damageCalculator = damageCalculator;
    }

    public void TakeDamage(AttackerInfo info)
    {
        if (IsDead || info.Damage <= 0) // 죽었거나 공격력이 0
        {
            return;
        }

        float damage = info.Damage;
        if (_damageCalculator != null) // 피해량을 계산함(방어력 포함)
        {
            damage = _damageCalculator.CalculateDamage(info);
        }

        CurrentHP -= damage;
        OnDamaged?.Invoke(info);

        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            OnDead?.Invoke();
        }
    }
}
