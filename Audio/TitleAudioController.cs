using UnityEngine;

public class TitleAudioController : MonoBehaviour
{
    [SerializeField] AudioSO _titleBGM;

    void Start()
    {
        AudioManager.Instance.PlayBGM(_titleBGM);
    }
}
