using UnityEngine;

public class FlashingLight : MonoBehaviour
{
    private Material _emissionMaterial;
    private Renderer _renderer;
    private Material _initialMaterial;

    private float _maxColorValue = 2.55f, _minColorValue = 1f;
    private float _colorValue = 0f, _timeValueChangeSpeed = 0.01f;
    private bool _bigger = false;
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _emissionMaterial = _renderer.material;
        _initialMaterial = _emissionMaterial;
        _colorValue = _initialMaterial.color.r;    
    }
    void FixedUpdate()
    {
        ChangeEmissionMaterial();
    }
    private void ChangeEmissionMaterial()
    {
        if (_colorValue < _maxColorValue && !_bigger)
        {
            _colorValue += _timeValueChangeSpeed * TimeManager.GetTimeScale();
            DynamicGI.SetEmissive(_renderer, new Color(_colorValue, _colorValue, _colorValue, _colorValue));
        }
        else if (_colorValue >= _minColorValue)
        {
            _bigger = true;
            _colorValue -= _timeValueChangeSpeed * TimeManager.GetTimeScale();
            DynamicGI.SetEmissive(_renderer, new Color(_colorValue, _colorValue, _colorValue, _colorValue));
        }
        else
        {
            _bigger = false;
        }
    }
}
