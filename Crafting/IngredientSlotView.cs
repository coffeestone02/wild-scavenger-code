using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSlotView : MonoBehaviour
{
    [SerializeField] Image _itemIcon;
    [SerializeField] TMP_Text _itemName;
    [SerializeField] TMP_Text _count;

    /// <summary> 재료 슬롯 UI 설정 </summary>
    public void Set(Sprite icon, string itemName, int owned, int needed, bool isEnough)
    {
        _itemIcon.sprite = icon;
        _itemName.text = itemName;
        _count.text = $"{owned} / {needed}";

        if (isEnough)
        {
            _itemName.color = Color.white;
        }
        else
        {
            _itemName.color = Color.red;
        }
    }
}
