using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealth : MonoBehaviour
{
    [SerializeField] Health _health;
    [SerializeField] Slider _slider;

    void Start()
    {
        _slider.maxValue = _health.CurrentHP;
    }

    void OnEnable()
    {
        _health.OnDamaged += UpdateHPSlider;
    }

    void OnDisable()
    {
        _health.OnDamaged -= UpdateHPSlider;
    }

    void UpdateHPSlider(AttackerInfo info)
    {
        _slider.value = _health.CurrentHP;
    }
}
