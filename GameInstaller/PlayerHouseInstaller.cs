using UnityEngine;

public class PlayerHouseInstaller : BaseGameInstaller
{
    [Header("Additional Presenters")]
    [SerializeField] CraftingPresenter _craftingPresenter;
    [SerializeField] SellingPortalPresenter _sellingPortalPresenter;
    [SerializeField] StorePresenter _storePresenter;

    [Header("Additional Gameplay")]
    [SerializeField] WeaponController _weaponController;
    [SerializeField] ActionController _actionController;
    [SerializeField] WeaponVisual _characterPreview;
    [SerializeField] QuickslotController _quickslot;

    [Header("Additional ItemDB")]
    [SerializeField] StoreSO _storeDB;
    [SerializeField] RecipeDatabaseSO _recipeDB;

    [Header("Additional Settings")]
    [SerializeField] int _quickslotCount = 5;

    CraftingModel _craftingModel;
    SellingModel _sellingModel;
    StoreModel _storeModel;

    protected override void InstallModel()
    {
        base.InstallModel();

        _craftingModel = new CraftingModel(_inventoryModel.Storage, _recipeDB);
        _sellingModel = new SellingModel(_inventoryModel.Storage, _playerWallet);
        _storeModel = new StoreModel(_inventoryModel.Storage, _storeDB, _playerWallet);
    }

    protected override void InstallPresenter()
    {
        base.InstallPresenter();

        _craftingPresenter.Init(_craftingModel, _inventoryModel.Storage);
        _sellingPortalPresenter.Init(_sellingModel, _playerWallet, _inventoryModel.Storage);
        _storePresenter.Init(_storeModel, _inventoryModel.Storage);
    }

    protected override void InstallGameplay()
    {
        _weaponController.Init(_inventoryModel.Weapon);

        _actionController.Init(_inventoryModel.Storage);

        _characterPreview.Init(_inventoryModel.Weapon);

        IInventoryStorage itemStorage = _inventoryModel.Storage;
        IInventoryWeapon weapon = _inventoryModel.Weapon;
        _quickslot.Init(itemStorage, weapon, _inventoryModel, _quickslotCount);
    }

}
