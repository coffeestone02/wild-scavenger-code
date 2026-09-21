using UnityEngine;

public class AIAudio : MonoBehaviour
{
    [SerializeField] AudioSO _cry;

    public void PlayCrySound()
    {
        AudioManager.Instance.PlaySFX(_cry, transform.position);
    }
}
