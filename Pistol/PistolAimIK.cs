using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PistolAimIK : MonoBehaviour
{
    [SerializeField] Rig _aimRig;

    void OnDisable()
    {
        _aimRig.weight = 0f;
    }

    public void SetAiming(bool isAiming)
    {
        if (isAiming)
        {
            _aimRig.weight = 1f;
        }
        else
        {
            _aimRig.weight = 0f;
        }
    }
}
