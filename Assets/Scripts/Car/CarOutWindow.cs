using UnityEngine;

public class CarOutWindow : MonoBehaviour
{
    [SerializeField] private float _speed = 0.02f;
    [SerializeField] private float _maxX = 30f;
    [SerializeField] private bool _inverse = false;
    private Vector3 _initialPosition;
    private Vector3 _currentPosition;
    private void Awake()
    {
        _initialPosition = transform.position;
        _currentPosition = transform.position;
    }
    private void FixedUpdate()
    {
        _currentPosition = new Vector3(_currentPosition.x + _speed * TimeManager.GetTimeScale(), _currentPosition.y, _currentPosition.z);
        transform.position = _currentPosition;
        if (_inverse)
        {
            if (transform.position.x < _maxX)
            {
                transform.position = _initialPosition;
                _currentPosition = _initialPosition;
            }
        }
        else
        {
            if (transform.position.x > _maxX)
            {
                transform.position = _initialPosition;
                _currentPosition = _initialPosition;
            }
        }
    }
}
