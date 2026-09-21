using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Health _health;
    [SerializeField] ActionController _action;
    [SerializeField] WeaponController _weapon;
    [SerializeField] MeleeWeapon[] _melees;
    [SerializeField] PistolWeapon _pistol;

    int _actionLayer = 1;
    int _armLayer = 2;
    float _crossFadeDuration = 0.1f;

    public bool IsActionPlaying { get; private set; }

    void Awake()
    {
        _health.OnDamaged += HandleDamage;
        _health.OnDead += HandleDead;

        _action.OnInteract += HandlePickUp;

        foreach (MeleeWeapon weapon in _melees)
        {
            weapon.OnSwing += HandleSwing;
        }
        _pistol.OnAim += HandlePistolAim;
        _pistol.OnAimEnd += HandlePistolAimEnd;
        _pistol.OnShoot += HandlePistolShoot;
    }

    void Update()
    {
        Move();
    }

    void OnDestroy()
    {
        _health.OnDamaged -= HandleDamage;
        _health.OnDead -= HandleDead;

        _action.OnInteract -= HandlePickUp;

        foreach (MeleeWeapon weapon in _melees)
        {
            weapon.OnSwing -= HandleSwing;
        }
        _pistol.OnAim -= HandlePistolAim;
        _pistol.OnAimEnd -= HandlePistolAimEnd;
        _pistol.OnShoot -= HandlePistolShoot;
    }

    void Move()
    {
        float moveSpeed = 0f;
        if (Managers.Input.MoveAmt.x != 0f || Managers.Input.MoveAmt.y != 0f)
        {
            if (_weapon.IsAiming) // 조준
            {
                moveSpeed = 0.75f;
            }
            else // 평소 상태
            {
                moveSpeed = 1f;
            }
        }

        _animator.SetFloat("Speed", moveSpeed);
    }

    void HandleDamage(AttackerInfo info)
    {
        _animator.CrossFade("Damage", _crossFadeDuration, _actionLayer);
    }

    void HandleDead()
    {
        _animator.CrossFade("Dead", _crossFadeDuration);
    }

    void HandleSwing()
    {
        IsActionPlaying = true;
        _animator.CrossFade("Swing", _crossFadeDuration, _actionLayer);
    }

    void HandlePickUp(Define.EInteractionType type)
    {
        _animator.CrossFade("PickUp", _crossFadeDuration, _armLayer);
    }

    void HandlePistolAim()
    {
        IsActionPlaying = false;
        _animator.CrossFade("PistolAim", _crossFadeDuration, _actionLayer);
    }

    void HandlePistolAimEnd()
    {
        _animator.CrossFade("Empty", _crossFadeDuration, _actionLayer);
    }

    void HandlePistolShoot()
    {
        _animator.CrossFade("PistolShoot", _crossFadeDuration, _actionLayer);
    }

    /// <summary> 애니메이션 이벤트로 종료시 자동 호출 </summary>
    public void OnActionFinished()
    {
        IsActionPlaying = false;
        _animator.CrossFade("Empty", _crossFadeDuration, _armLayer);
        _animator.CrossFade("Empty", _crossFadeDuration, _actionLayer);
    }
}
