using UnityEngine;

public class ForestInstaller : BaseGameInstaller
{
    [Header("Additional Gameplay")]
    [SerializeField] WeaponController _weaponController;
    [SerializeField] ActionController _actionController;
    [SerializeField] WeaponVisual _characterPreview;
    [SerializeField] QuickslotController _quickslot;

    [Header("Additional Settings")]
    [SerializeField] int _quickslotCount = 5;

    protected override void InstallGameplay()
    {
        base.InstallGameplay();

        _weaponController.Init(_inventoryModel.Weapon);

        _actionController.Init(_inventoryModel.Storage);

        _characterPreview.Init(_inventoryModel.Weapon);

        IInventoryStorage itemStorage = _inventoryModel.Storage;
        IInventoryWeapon weapon = _inventoryModel.Weapon;
        _quickslot.Init(itemStorage, weapon, _inventoryModel, _quickslotCount);

    }
}
