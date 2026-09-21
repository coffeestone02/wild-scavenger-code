using UnityEngine;

public class EntryPortal : MonoBehaviour, IInteractable
{
    [SerializeField] AudioSO _forest;
    [SerializeField] AudioSO _jump;
    bool _isUsed = false;

    public InteractionResult Interact()
    {
        if (_isUsed == false) // 소리 중복 재생 방지
        {
            AudioManager.Instance.PlaySFX(_jump);
            AudioManager.Instance.PlayBGM(_forest);
        }
        _isUsed = true;

        return new InteractionResult(Define.EInteractionType.EntryPortal);
    }

    public string GetName()
    {
        return name;
    }
}
