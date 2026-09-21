using System.Collections;
using UnityEngine;
using System;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] EquipmentStatSO _weapon;
    [SerializeField] PlayerStat _playerStat;

    [Header("적 감지")]
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] LayerMask _blockingLayer;
    [SerializeField] Transform _aimStart;
    [SerializeField] Transform _aimDir;

    [Header("AudioSO")]
    [SerializeField] AudioSO _meleeHit;
    [SerializeField] AudioSO _meleeSwing;

    bool _canAttack;
    WaitForSeconds WaitCooldown;

    public event Action OnSwing;

    void Awake()
    {
        WaitCooldown = new WaitForSeconds(_weapon.AttackCooldown);
    }

    void OnEnable()
    {
        StartCoroutine(CoSwapDelay());
    }

    public void Swing()
    {
        if (_canAttack == false) { return; }

        PlaySwingSound();
        StartCoroutine(CoAttack());
    }

    IEnumerator CoSwapDelay()
    {
        yield return WaitCooldown;
        _canAttack = true;
    }

    IEnumerator CoAttack()
    {
        _canAttack = false;

        OnSwing?.Invoke();

        if (Physics.Raycast(_aimStart.position, _aimDir.forward, out RaycastHit hit, _weapon.AttackRange, _enemyLayer | _blockingLayer))
        {
            if (Util.IsInLayer(hit.collider.gameObject, _enemyLayer))
            {
                // 공격
                float damage = _playerStat.Attack;
                AttackerInfo info = new AttackerInfo(damage, gameObject);

                Health target = hit.collider.GetComponentInParent<Health>();
                if (target != null)
                {
                    target.TakeDamage(info);

                    // 이펙트 재생
                    Quaternion rotation = hit.transform.rotation;
                    EffectPlayer.Instance.Play(Define.EEffectType.NormalHit, hit.point, rotation);
                    PlayHitSound();
                }
            }
        }

        yield return WaitCooldown;

        _canAttack = true;
    }

    void PlaySwingSound()
    {
        AudioManager.Instance.PlaySFX(_meleeSwing, transform.position);
    }

    void PlayHitSound()
    {
        AudioManager.Instance.PlaySFX(_meleeHit, transform.position);
    }
}
