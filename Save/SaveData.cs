
using System;
using System.Collections.Generic;

/// 세이브용 클래스들
[Serializable]
public class SaveData
{
    public const int CurrentVersion = 1;

    public int Version = CurrentVersion;
    public InventoryData Inventory = new InventoryData();
    public PlayerWalletData Wallet = new PlayerWalletData();
    public SettingData Setting = new SettingData();

    public SaveData(InventoryData inventory, PlayerWalletData wallet, SettingData settingData)
    {
        Version = CurrentVersion;
        Inventory = inventory;
        Wallet = wallet;
        Setting = settingData;
    }
}

[Serializable]
public class ItemData
{
    public int Id;
    public int Count;
    public bool IsEmpty;
}

[Serializable]
public class InventoryData
{
    public List<ItemData> Items = new List<ItemData>();
    public List<ItemData> ArmorItems = new List<ItemData>();
}

[Serializable]
public class PlayerWalletData
{
    public int Gold;
}

[Serializable]
public class SettingData
{
    public float BgmVolume;
    public float SfxVolume;
    public float MouseSensitivity;
}
