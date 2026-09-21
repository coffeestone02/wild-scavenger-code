using UnityEngine;

public class BaseGameInstaller : MonoBehaviour
{
    [Header("Core Presenters")]
    [SerializeField] InventoryPresenter _inventoryPresenter;

    [Header("Core Gameplay")]
    [SerializeField] protected PlayerStat _playerStat;
    [SerializeField] protected PlayerWallet _playerWallet;
    [SerializeField] Health _playerHealth;

    [Header("Core Settings")]
    [SerializeField] int _inventorySlotCount = 10;
    [SerializeField] int _armorSlotCount = 4;
    [SerializeField] int _maxTargetFrame = 60;
    [SerializeField] UISettingSlider _settingSlider;

    [Header("Core ItemDB")]
    [SerializeField] ItemDatabaseSO _itemDB;

    protected InventoryModel _inventoryModel;
    bool _canSave = true;

    protected virtual void Awake()
    {
        _itemDB.Init();
        InstallModel();
        InstallPresenter();
        InstallGameplay();
    }

    protected virtual void Start()
    {
        SaveManager.LoadGame(_inventoryModel, _playerWallet, Managers.Settings);

        CustomSceneManager.Instance.OnBeforeSceneLoaded += SaveCheckPoint;

        _playerHealth.OnDead += Checkpoint;

        _settingSlider.Init(Managers.Settings);
        Application.targetFrameRate = _maxTargetFrame;
    }

    protected virtual void OnDestroy()
    {
        if (CustomSceneManager.Instance != null)
        {
            CustomSceneManager.Instance.OnBeforeSceneLoaded -= SaveCheckPoint;
        }

        _playerHealth.OnDead -= Checkpoint;

        _inventoryModel.Dispose();
    }

    protected virtual void InstallModel()
    {
        _inventoryModel = new InventoryModel(_inventorySlotCount, _armorSlotCount, _itemDB, _playerStat);
    }

    protected virtual void InstallPresenter()
    {
        _inventoryPresenter.Init(_inventoryModel);
    }

    protected virtual void InstallGameplay()
    {

    }

    // 사망 시 이번 세션은 저장하지 않음
    void Checkpoint()
    {
        _canSave = false;
    }

    // 살아서 넘어갈 때만 진행 확정
    void SaveCheckPoint()
    {
        if (_canSave == false) { return; }

        SaveManager.SaveGame(_inventoryModel, _playerWallet, Managers.Settings);
    }
}
