using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _target; // 타겟
    [SerializeField] Transform _aimTarget; // 조준 시 카메라 타겟
    [SerializeField] float _followSpeed = 10f; // 카메라 속도
    [SerializeField] float _clampAngle = 70f; // 상하 각도 제한
    [SerializeField] float _smoothness = 10f; // 카메라 움직임 부드러움 정도
    [SerializeField] float _controlSpeed = 0.001f;

    // 플레이어와 카메라 사이 물체가 있을 때, 카메라 거리 조정
    [SerializeField] LayerMask _collisionMask;
    [SerializeField] float _cameraRadius = 0.2f;
    [SerializeField] float _minDistance = 1f;
    [SerializeField] float _maxDistance = 2f;
    [SerializeField] Transform _camera;
    [SerializeField] WeaponController _weapon;

    float _finalDistance;
    float _rotX;
    float _rotY;
    Vector3 _direction;

    void Awake()
    {
        _rotX = transform.localRotation.eulerAngles.x;
        _rotY = transform.localRotation.eulerAngles.y;
        _direction = _camera.localPosition.normalized;
        _finalDistance = _camera.localPosition.magnitude;
        Managers.UI.SetMouseCursor(false);
    }

    void LateUpdate()
    {
        Rotate();
        Move();
    }

    void Move()
    {
        // 조준 상태면 조준용 타겟으로 교체
        Transform target = _target;
        if (_weapon.IsAiming && Managers.Input.AimPressed)
        {
            target = _aimTarget;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, _followSpeed * Time.deltaTime); // 카메라가 타겟을 따라감

        Vector3 origin = transform.position;
        Vector3 direction = transform.TransformDirection(_direction); // 로컬 좌표를 월드 좌표로 변환

        float targetDistance = _maxDistance;

        // 플레이어와 카메라 사이에 물체가 있을 때
        if (Physics.SphereCast(origin, _cameraRadius, direction, out RaycastHit hit, _maxDistance, _collisionMask, QueryTriggerInteraction.Ignore))
        {
            targetDistance = Mathf.Clamp(hit.distance, _minDistance, _maxDistance);
        }

        // 카메라가 장애물에 닿아 당겨질 때는 즉시, 장애물에서 멀어질 때만 부드럽게 이동
        if (targetDistance < _finalDistance)
        {
            _finalDistance = targetDistance;
        }
        else
        {
            _finalDistance = Mathf.Lerp(_finalDistance, targetDistance, _smoothness * Time.deltaTime);
        }

        _camera.localPosition = _direction * _finalDistance;
    }

    void Rotate()
    {
        // 입력값, 마우스 감도를 반영하여 카메라 회전값 계산
        _rotY += Managers.Input.LookAmt.x * Managers.Settings.MouseSensitivity * _controlSpeed;
        _rotX -= Managers.Input.LookAmt.y * Managers.Settings.MouseSensitivity * _controlSpeed;
        _rotX = Mathf.Clamp(_rotX, -_clampAngle, _clampAngle);

        Quaternion rot = Quaternion.Euler(_rotX, _rotY, 0);
        transform.rotation = rot;
    }
}
