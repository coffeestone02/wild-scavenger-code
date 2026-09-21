using System;
using UnityEngine;

public class WeaponController : WeaponVisual
{
    public bool IsAiming { get; private set; }

    public event Action<bool> OnAiming;

    MeleeWeapon _melee;
    PistolWeapon _pistol;

    void Update()
    {
        AttackAction();
    }

    protected override void HandleEquip(EquipmentStatSO item)
    {
        base.HandleEquip(item);

        CacheWeapon();
    }

    protected override void HandleUnequip(EquipmentStatSO item)
    {
        base.HandleUnequip(item);

        _melee = null;
        _pistol = null;
        SetAiming(false);
    }

    void CacheWeapon()
    {
        _melee = null;
        _pistol = null;

        if (CurrentWeapon == null) { return; }
        if (_weapons.TryGetValue(CurrentWeapon, out GameObject weapon) == false) { return; }

        _melee = weapon.GetComponent<MeleeWeapon>();
        _pistol = weapon.GetComponent<PistolWeapon>();
    }

    void SetAiming(bool value)
    {
        if (IsAiming == value) { return; }

        IsAiming = value;
        OnAiming?.Invoke(IsAiming);
    }

    void AttackAction()
    {
        if (_melee != null && Managers.Input.AttackPressed)
        {
            _melee.Swing();
        }
        else if (_pistol != null)
        {
            SetAiming(Managers.Input.AimPressed);
            _pistol.UpdateAim(Managers.Input.AimPressed, Managers.Input.AttackPressed);
        }
    }
}
