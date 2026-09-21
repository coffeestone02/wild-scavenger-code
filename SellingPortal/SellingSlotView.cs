using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellingSlotView : MonoBehaviour
{
    [SerializeField] Image _itemIcon;
    [SerializeField] TMP_Text _itemName;
    [SerializeField] TMP_Text _value;

    /// <summary> 재료 슬롯 UI 설정 </summary>
    public void Set(Sprite icon, string itemName, int count, int value)
    {
        _itemIcon.sprite = icon;
        _itemName.text = $"{itemName} x{count}";
        _value.text = $"{value} Gold";
    }
}
