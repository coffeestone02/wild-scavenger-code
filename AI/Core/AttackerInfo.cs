using UnityEngine;

public class AttackerInfo
{
    public float Damage; // 공격 피해량
    public GameObject Attacker; // 공격자

    public AttackerInfo(float damage, GameObject attacker)
    {
        Damage = damage;
        Attacker = attacker;
    }
}
