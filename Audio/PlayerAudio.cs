using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("AudioSO")]
    [SerializeField] AudioSO _footstep;
    [SerializeField] AudioSO _getItem;
    [SerializeField] AudioSO _damaged;
    [SerializeField] AudioSO _dead;

    [Header("참조 컴포넌트")]
    [SerializeField] ActionController _action;
    [SerializeField] Health _player;

    void Start()
    {
        _action.OnInteract += HandleInteractSound;

        _player.OnDamaged += HandleDamagedSound;
        _player.OnDead += HandleDeadSound;
    }

    void OnDestroy()
    {
        _action.OnInteract -= HandleInteractSound;

        _player.OnDamaged -= HandleDamagedSound;
        _player.OnDead -= HandleDeadSound;
    }

    void HandleInteractSound(Define.EInteractionType type)
    {
        switch (type)
        {
            case Define.EInteractionType.Item:
                AudioManager.Instance.PlaySFX(_getItem);
                break;
            case Define.EInteractionType.CraftingStation:
                break;
            case Define.EInteractionType.SellingPortal:
                break;
        }
    }

    void HandleDamagedSound(AttackerInfo info)
    {
        AudioManager.Instance.PlaySFX(_damaged);
    }

    void HandleDeadSound()
    {
        AudioManager.Instance.PlaySFX(_dead);
    }

    public void PlayFootstep()
    {
        AudioManager.Instance.PlaySFX(_footstep, transform.position);
    }
}
