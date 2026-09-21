using TMPro;
using UnityEngine;

public class WorldBorder : MonoBehaviour
{
    [SerializeField] TMP_Text _worldInfoText;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _worldInfoText.text = "해당 구역으로 이동이 불가합니다.";
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _worldInfoText.text = "";
        }
    }
}
