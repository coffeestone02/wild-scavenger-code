using UnityEngine;

public class SellingPortal : MonoBehaviour, IInteractable
{
    [SerializeField] SellingPortalPresenter _portalPresenter;
    [SerializeField] Transform _effectPos;

    void OnEnable()
    {
        _portalPresenter.OnSold += SellEffect;
    }

    void OnDisable()
    {
        _portalPresenter.OnSold -= SellEffect;
    }

    void SellEffect()
    {
        EffectPlayer.Instance.Play(Define.EEffectType.Wave, _effectPos.position, _effectPos.rotation);
    }

    public InteractionResult Interact()
    {
        return new InteractionResult(Define.EInteractionType.SellingPortal);
    }

    public string GetName()
    {
        return name;
    }
}