using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemTooltip : MonoBehaviour
{
    [Header("UI 컴포넌트")]
    [SerializeField] TMP_Text _nameText; // 이름 
    [SerializeField] TMP_Text _typeText; // 타입
    [SerializeField] Image _itemIcon; // 아이콘 이미지
    [SerializeField] TMP_Text _descriptionText; // 아이템 설명
    [SerializeField] TMP_Text _statText; // 장비인 경우 스탯 표시
    [SerializeField] TMP_Text _valueText; // 아이템 가치

    RectTransform _rectTF;

    void Awake()
    {
        _rectTF = GetComponent<RectTransform>();
    }

    public void ShowView(RectTransform targetRect, ItemSO item, int count = 1)
    {
        if (item == null) { return; }

        // UI 갱신
        _nameText.text = item.ItemName;
        _typeText.text = item.ItemType.ToString();
        _itemIcon.sprite = item.ItemIcon;
        _descriptionText.text = item.Description;
        _valueText.text = $"{item.Gold * count} Gold";

        if (item.ItemType == Define.EItemType.Equipment)
        {
            EquipmentStatSO stat = item as EquipmentStatSO;
            _statText.text = $"공격력: {stat.Attack} / 방어력: {stat.Defense}";
        }
        else
        {
            _statText.text = "";
        }

        // 위치 설정(아이템 설명 박스 크기 + 타겟 셀 크기)
        float x = _rectTF.rect.width * 0.5f + targetRect.rect.width * 0.5f;
        float y = _rectTF.rect.height * 0.5f + targetRect.rect.height * 0.5f;
        Vector3 pos = new Vector3(x, -y, 0f);

        // 화면 오른쪽이 잘리면 왼쪽으로 이동 (x + _rectTf.rect.width * 0.5) -> pivot 중앙 기준으로 오른쪽 끝을 계산
        float rightEdge = targetRect.position.x + x + _rectTF.rect.width * 0.5f;
        if (rightEdge > Screen.width)
        {
            pos.x = -x;
        }

        transform.position = targetRect.position + pos;

        gameObject.SetActive(true);
    }


    public void ShowView(RectTransform targetRect, Item item)
    {
        if (item == null) { return; }

        ShowView(targetRect, item.ItemSO, item.ItemCount);
    }

    public void HideView()
    {
        gameObject.SetActive(false);
    }
}
