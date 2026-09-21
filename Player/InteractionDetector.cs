using System;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] PlayerStat _stat;
    [SerializeField] LayerMask _interactLayer;
    [SerializeField] LayerMask _blockingLayer;
    [SerializeField] Transform _aimStart;
    [SerializeField] Transform _aimDir;

    OutlineController _currentOutline;

    public IInteractable InteractableTarget { get; private set; }

    public event Action<IInteractable> OnInteractableChanged;

    void Update()
    {
        DetectInteractable();
    }

    // 카메라 방향으로 상호작용 가능 오브젝트 감지
    void DetectInteractable()
    {
        IInteractable found = null;
        OutlineController foundOutline = null;

        // 카메라 방향(시선)으로 레이캐스트
        int mask = _interactLayer | _blockingLayer; // 상호작용 레이어와 장애물 레이어를 같이 검사
        if (Physics.Raycast(_aimStart.position, _aimDir.forward, out RaycastHit hit, _stat.InteractRange, mask))
        {
            // 장애물에 먼저 맞으면 인식하지 않음
            if (Util.IsInLayer(hit.collider.gameObject, _interactLayer))
            {
                found = hit.collider.GetComponent<IInteractable>();
                hit.collider.TryGetComponent(out foundOutline);
            }
        }

        if (found != InteractableTarget) // 상호작용 대상이 바뀔 때만 UI 갱신
        {
            if (_currentOutline != null)
            {
                _currentOutline.HideOutline();
            }

            if (foundOutline != null)
            {
                foundOutline.ShowOutline();
            }

            _currentOutline = foundOutline;
            InteractableTarget = found;
            OnInteractableChanged?.Invoke(InteractableTarget);
        }
    }
}