using System.Collections.Generic;
using UnityEngine;

public class WeaponVisual : MonoBehaviour
{
    [SerializeField] Transform _weaponRoot;

    protected Dictionary<EquipmentStatSO, GameObject> _weapons;
    protected IInventoryWeapon _weaponHandler;

    public EquipmentStatSO CurrentWeapon { get; private set; }

    public void Init(IInventoryWeapon weaponHandler)
    {
        _weaponHandler = weaponHandler;
        _weaponHandler.OnEquipped += HandleEquip;
        _weaponHandler.OnUnequipped += HandleUnequip;

        _weapons = new Dictionary<EquipmentStatSO, GameObject>();
        for (int i = 0; i < _weaponRoot.childCount; i++)
        {
            GameObject weaponGO = _weaponRoot.GetChild(i).gameObject;

            Item weaponItem = weaponGO.GetComponent<ItemInteract>().Item;
            EquipmentStatSO eqStatSO = weaponItem.GetEquipmentStatSO();

            _weapons[eqStatSO] = weaponGO;
        }
    }

    void OnDestroy()
    {
        if (_weaponHandler != null)
        {
            _weaponHandler.OnEquipped -= HandleEquip;
            _weaponHandler.OnUnequipped -= HandleUnequip;
        }
    }

    protected virtual void HandleEquip(EquipmentStatSO item)
    {
        // 공격무기 착용
        if (item != null && _weapons.TryGetValue(item, out GameObject weapon))
        {
            CurrentWeapon = item;
            weapon.SetActive(true);
        }
    }

    protected virtual void HandleUnequip(EquipmentStatSO item)
    {
        EquipmentStatSO target = item;
        if (item == null) // item이 null이면 현재 착용 중인 무기를 해제함
        {
            target = CurrentWeapon;
        }

        if (target != null && _weapons.TryGetValue(target, out GameObject weapon))
        {
            CurrentWeapon = null;
            weapon.SetActive(false);
        }
    }
}
