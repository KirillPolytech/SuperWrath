using UnityEngine;

public class FlashingLight : MonoBehaviour
{
    private Material _emissionMaterial;
    private Renderer _renderer;
    private Material _initialMaterial;

    private float _maxColorValue = 2f, _minColorValue = 1f;
    private float _colorValue = 0f;
    private bool _bigger = false;
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _emissionMaterial = _renderer.material;
        _initialMaterial = _emissionMaterial;
        _colorValue = _minColorValue;    
    }
    void Update()
    {
        ChangeEmissionMaterial();
    }
    private void ChangeEmissionMaterial()
    {
        if (_colorValue < _maxColorValue && !_bigger)
        {
            _colorValue += Time.deltaTime * TimeManager.GetTimeScale();
            DynamicGI.SetEmissive(_renderer, new Color(_colorValue, _colorValue, _colorValue, _colorValue));
        }
        else if (_colorValue >= _minColorValue)
        {
            _bigger = true;
            _colorValue -= Time.deltaTime * TimeManager.GetTimeScale();
            DynamicGI.SetEmissive(_renderer, new Color(_colorValue, _colorValue, _colorValue, _colorValue));
        }
        else
        {
            _bigger = false;
        }
    }
}
