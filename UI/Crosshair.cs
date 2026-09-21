using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] WeaponController _weapon;
    [SerializeField] GameObject _crosshair;

    void OnEnable()
    {
        _weapon.OnAiming += SetCrosshair;
    }

    void OnDisable()
    {
        _weapon.OnAiming -= SetCrosshair;
    }

    void SetCrosshair(bool isActive)
    {
        _crosshair.SetActive(isActive);
    }
}
