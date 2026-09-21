using UnityEngine;
using System;

/// <summary> 공통 상태 </summary>
[Serializable]
public class AIHealthStat
{
    [SerializeField] float _maxHealth = 100;

    public float MaxHealth => _maxHealth;
}

/// <summary> 공통 상태 </summary>
[Serializable]
public class AIMoveStat
{
    [SerializeField] float _moveSpeed = 3.5f;
    [SerializeField] float _rotateSpeed = 5f;
    [SerializeField] float _patrolRadius = 5f;
    [SerializeField] float _idleDuration = 3f;

    public float MoveSpeed => _moveSpeed;
    public float RotateSpeed => _rotateSpeed;
    public float PatrolRadius => _patrolRadius;
    public float IdleDuration => _idleDuration;
}

[Serializable]
public class AIFleeStat
{
    [SerializeField] float _fleeSpeed = 5f;
    [SerializeField] float _fleeDuration = 4f;
    [SerializeField] float _fleeDistance = 10f;
    [SerializeField] float _viewAngle = 30f;

    public float FleeSpeed => _fleeSpeed;
    public float FleeDuration => _fleeDuration;
    public float FleeDistance => _fleeDistance;
    public float ViewAngle => _viewAngle;
}


[Serializable]
public class AIChaseStat
{
    [SerializeField] float _chaseSpeed = 4f;
    [SerializeField] float _chaseDistance = 7f;
    [SerializeField] float _loseTargetDistance = 21f;

    public float ChaseSpeed => _chaseSpeed;
    public float ChaseDistance => _chaseDistance;
    public float LoseTargetDistance => _loseTargetDistance;
}

[Serializable]
public class AIAttackStat
{
    [SerializeField] float _attackSpeed = 3f;
    [SerializeField] float _attackRange = 2f;
    [SerializeField] float _attackCooldown = 2f;
    [SerializeField] int _attackDamage = 25;

    public float AttackSpeed => _attackSpeed;
    public float AttackRange => _attackRange;
    public float AttackCooldown => _attackCooldown;
    public int AttackDamage => _attackDamage;
}
