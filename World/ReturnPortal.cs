using UnityEngine;

public class ReturnPortal : MonoBehaviour, IInteractable
{
    [SerializeField] AudioSO _main;
    [SerializeField] AudioSO _jump;
    bool _isUsed = false;

    public InteractionResult Interact()
    {
        if (_isUsed == false) // 소리 중복 재생 방지
        {
            AudioManager.Instance.PlaySFX(_jump);
            AudioManager.Instance.PlayBGM(_main);
        }
        _isUsed = true;
        return new InteractionResult(Define.EInteractionType.ReturnPortal);
    }

    public string GetName()
    {
        return name;
    }
}
