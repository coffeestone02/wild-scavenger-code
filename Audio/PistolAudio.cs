using UnityEngine;

public class PistolAudio : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] PistolWeapon _pistol;

    [Header("AudioSO")]
    [SerializeField] AudioSO _pistolAim;
    [SerializeField] AudioSO _pistolAimEnd;
    [SerializeField] AudioSO _pistolShoot;
    [SerializeField] AudioSO _pistolHit;

    void OnEnable()
    {
        _pistol.OnAim += PlayAimSound;
        _pistol.OnAimEnd += PlayAimEndSound;
        _pistol.OnShoot += PlayShootSound;
        _pistol.OnHit += PlayHitSound;
    }

    void OnDisable()
    {
        _pistol.OnAim -= PlayAimSound;
        _pistol.OnAimEnd -= PlayAimEndSound;
        _pistol.OnShoot -= PlayShootSound;
        _pistol.OnHit -= PlayHitSound;
    }

    void PlayAimSound()
    {
        AudioManager.Instance.PlaySFX(_pistolAim);
    }

    void PlayAimEndSound()
    {
        AudioManager.Instance.PlaySFX(_pistolAimEnd);
    }

    void PlayShootSound()
    {
        AudioManager.Instance.PlaySFX(_pistolShoot);
    }

    void PlayHitSound()
    {
        AudioManager.Instance.PlaySFX(_pistolHit);
    }
}
