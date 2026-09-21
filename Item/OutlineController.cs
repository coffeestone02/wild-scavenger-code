using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [Header("Outline")]
    [SerializeField] Renderer _renderer;
    [SerializeField] int _outlineMatIdx = 1;
    [SerializeField] float _outlineSize = 1.05f;

    int _outlineSizeID = Shader.PropertyToID("_OutlineSize");
    MaterialPropertyBlock _propertyBlock;

    void Awake()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }

        _propertyBlock = new MaterialPropertyBlock();
        HideOutline();
    }

    public void ShowOutline()
    {
        _renderer.GetPropertyBlock(_propertyBlock, _outlineMatIdx);
        _propertyBlock.SetFloat(_outlineSizeID, _outlineSize);
        _renderer.SetPropertyBlock(_propertyBlock, _outlineMatIdx);
    }

    public void HideOutline()
    {
        _renderer.GetPropertyBlock(_propertyBlock, _outlineMatIdx);
        _propertyBlock.SetFloat(_outlineSizeID, 0f);
        _renderer.SetPropertyBlock(_propertyBlock, _outlineMatIdx);
    }
}
