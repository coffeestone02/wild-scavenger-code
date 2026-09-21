using TMPro;
using UnityEngine;

public class UIMoneyView : MonoBehaviour
{
    [SerializeField] PlayerWallet _playerWallet;
    [SerializeField] TMP_Text _moneyText;

    void OnEnable()
    {
        _playerWallet.OnGoldChanged += HandleMoneyText;
        HandleMoneyText();
    }

    void OnDisable()
    {
        _playerWallet.OnGoldChanged -= HandleMoneyText;
    }

    public void HandleMoneyText()
    {
        _moneyText.text = $"보유금: {_playerWallet.Gold}";
    }
}
