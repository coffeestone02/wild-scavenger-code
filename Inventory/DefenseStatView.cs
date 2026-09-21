using TMPro;
using UnityEngine;

public class DefenseStatView : MonoBehaviour
{
    [SerializeField] PlayerStat _playerStat;
    [SerializeField] TMP_Text _statText;

    void OnEnable()
    {
        _playerStat.OnDefenseChanged += HandleDefenseText;
        HandleDefenseText();
    }

    void OnDisable()
    {
        _playerStat.OnDefenseChanged -= HandleDefenseText;
    }

    public void HandleDefenseText()
    {
        _statText.text = $"방어력: {_playerStat.Defense}";
    }
}
