using System;
using UnityEngine;

public class PlayerWallet : MonoBehaviour, IPlayerWalletSave, IWallet
{
    public int Gold { get; private set; } = 0;

    public event Action OnGoldChanged;

    void SetGold(int value)
    {
        Gold = value;
        OnGoldChanged?.Invoke();
    }

    public void AddGold(int amount)
    {
        if (amount < 0) { return; }
        SetGold(Gold + amount);
    }

    public bool TrySpendGold(int amount)
    {
        if (amount < 0 || Gold < amount) { return false; }

        SetGold(Gold - amount);
        return true;
    }

    public PlayerWalletData GetSaveData()
    {
        PlayerWalletData data = new PlayerWalletData();

        data.Gold = Gold;

        return data;
    }

    public void ApplySaveData(PlayerWalletData data)
    {
        if (data == null)
        {
            Debug.LogError("[PlayerStat] 세이브 데이터 적용 실패");
            return;
        }

        SetGold(data.Gold);
    }
}
