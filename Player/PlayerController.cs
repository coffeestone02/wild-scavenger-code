using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] Health _health;
    [SerializeField] Transform _cam;
    [SerializeField] WeaponController _weapon;
    [SerializeField] PlayerStat _stat;
    [SerializeField] PlayerAnimator _animator;

    bool _dead;

    void Awake()
    {
        _health.Init(_stat.MaxHP, _stat);
        _health.OnDead += Dead;
    }

    void FixedUpdate()
    {
        if (_dead || _animator.IsActionPlaying) { return; }

        Move();
        Rotate();
    }

    void OnDestroy()
    {
        _health.OnDead -= Dead;
    }

    void Dead()
    {
        _dead = true;
    }

    void Move()
    {
        float x = Managers.Input.MoveAmt.x;
        float y = Managers.Input.MoveAmt.y;

        Vector3 moveDir = Vector3.zero;
        moveDir = moveDir + _cam.forward * y; // 앞, 뒤
        moveDir = moveDir + _cam.right * x; // 좌, 우
        moveDir.y = 0f;
        moveDir.Normalize();

        float moveSpeed = _stat.MoveSpeed;
        if (_weapon.IsAiming) // 조준 이동속도
        {
            moveSpeed = _stat.AimMoveSpeed;
        }

        _rigidbody.MovePosition(_rigidbody.position + moveDir * moveSpeed * Time.deltaTime);
    }

    void Rotate()
    {
        if (_weapon.IsAiming) // 조준 시 회전
        {
            Vector3 camForward = _cam.forward;
            camForward.y = 0; // 수직 성분은 제외
            Quaternion rot = Quaternion.LookRotation(camForward); // 바라보는 방향으로 회전
            _rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, rot, _stat.RotateSpeed * Time.deltaTime));
            return;
        }

        Vector3 targetDir = Vector3.zero;
        targetDir = targetDir + _cam.forward * Managers.Input.MoveAmt.y; // 앞, 뒤
        targetDir = targetDir + _cam.right * Managers.Input.MoveAmt.x; // 좌, 우
        targetDir.y = 0f;
        targetDir.Normalize();

        if (targetDir == Vector3.zero) // 움직이지 않을 때 마지막 방향 유지
        {
            targetDir = transform.forward;
        }

        Quaternion targetRot = Quaternion.LookRotation(targetDir); // 바라보는 방향으로 회전
        _rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRot, _stat.RotateSpeed * Time.deltaTime));
    }
}
