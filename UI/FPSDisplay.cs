using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text _fpsText;
    float _pollingTime = 0.5f;
    float _time;
    int _frameCount;

    void Update()
    {
        _time += Time.deltaTime;
        _frameCount++;

        if (_time >= _pollingTime)
        {
            int frameRate = Mathf.RoundToInt(_frameCount / _time);
            _fpsText.text = $"FPS: {frameRate}";

            _time = 0f;
            _frameCount = 0;
        }
    }
}
