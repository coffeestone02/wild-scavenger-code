using System;
using System.Collections;
using UnityEngine;

public class PistolWeapon : MonoBehaviour
{
    [SerializeField] PlayerStat _playerStat;

    [Header("Pistol")]
    [SerializeField] EquipmentStatSO _pistol;
    [SerializeField] PistolAimIK _pistolIK;

    [Header("적 감지")]
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] LayerMask _blockingLayer;
    [SerializeField] Transform _trailStart;
    [SerializeField] Camera _cam;

    bool _canAttack = false;
    Vector3 _aimVector = new Vector3(0.5f, 0.5f, 0f);
    WaitForSeconds WaitCooldown;

    public bool IsAiming { get; private set; }

    public event Action OnAim;
    public event Action OnAimEnd;
    public event Action OnShoot;
    public event Action OnHit;

    void Awake()
    {
        WaitCooldown = new WaitForSeconds(_pistol.AttackCooldown);
    }

    void OnEnable()
    {
        StartCoroutine(CoSwapDelay());
    }

    void OnDisable()
    {
        if (IsAiming) // 무기 해제로 비활성화되면 조준 상태 초기화
        {
            IsAiming = false;
            _canAttack = false;
            OnAimEnd?.Invoke();
        }

        StopAllCoroutines();
    }

    public void UpdateAim(bool aimHeld, bool attackHeld)
    {
        if (aimHeld)
        {
            if (IsAiming == false) // 처음 한 번만 조준 진입
            {
                IsAiming = true;
                _pistolIK.SetAiming(IsAiming);
                OnAim?.Invoke();
            }

            if (attackHeld && _canAttack) // 조준 중일 때만 발사
            {
                Shoot();
            }
        }
        else
        {
            if (IsAiming) // 뗀 프레임 한 번만 조준 해제
            {
                IsAiming = false;
                _pistolIK.SetAiming(IsAiming);
                OnAimEnd?.Invoke();
            }
        }
    }

    public void Shoot()
    {
        StartCoroutine(CoShoot());
    }

    // 스왑 대기용
    IEnumerator CoSwapDelay()
    {
        yield return WaitCooldown;
        _canAttack = true;
    }

    IEnumerator CoShoot()
    {
        _canAttack = false;

        // 화면 중앙으로 발사
        Ray ray = _cam.ViewportPointToRay(_aimVector);
        EffectPlayer.Instance.Play(Define.EEffectType.MuzzleFlash, _trailStart.position, _trailStart.rotation); // 총구화염 이펙트

        if (Physics.Raycast(ray, out RaycastHit hit, _pistol.AttackRange, _enemyLayer | _blockingLayer))
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

                    // 이펙트 재생(히트 표시)
                    Vector3 hitPos = hit.point;
                    Quaternion rotation = Quaternion.LookRotation(hit.normal);
                    EffectPlayer.Instance.Play(Define.EEffectType.NormalHit, hitPos, rotation);

                    OnHit?.Invoke();
                }
            }
        }

        // 트레일 생성
        Vector3 hitPoint = ray.GetPoint(_pistol.AttackRange);
        Vector3 dir = (hitPoint - _trailStart.position).normalized;
        EffectPlayer.Instance.PlayTrail(Define.EEffectType.BulletTrail, _trailStart.position, dir, 150f);

        OnShoot?.Invoke();

        yield return WaitCooldown;

        _canAttack = true;
    }
}
