using UnityEngine;

public class AimTargetFollower : MonoBehaviour
{
    [SerializeField] Transform _cam;
    [SerializeField] float _distance = 10f;
    [SerializeField] float _yOffset = 2f;

    void LateUpdate()
    {
        Vector3 pos = _cam.position + _cam.forward * _distance;
        pos.y -= _yOffset;
        transform.position = pos;
    }
}
