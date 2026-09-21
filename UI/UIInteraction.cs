using TMPro;
using UnityEngine;

public class UIInteraction : MonoBehaviour
{
    [SerializeField] InteractionDetector _detector;
    [SerializeField] TMP_Text _alertText;

    void OnEnable()
    {
        _detector.OnInteractableChanged += HandleAlertText;
    }

    void OnDisable()
    {
        _detector.OnInteractableChanged -= HandleAlertText;
    }

    void HandleAlertText(IInteractable interactable)
    {
        if (interactable == null || Managers.UI.IsSceneUIOpen)
        {
            _alertText.text = "";
            return;
        }

        _alertText.text = $"[E] {interactable.GetName()}";
    }
}
