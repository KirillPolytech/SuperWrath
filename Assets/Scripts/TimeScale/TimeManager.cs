using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static float _currentTimeScale = 0f;
    public static float GetTimeScale()
    {
        return _currentTimeScale;
    }
    public static void SetTimeScale(float timeValue)
    {
        _currentTimeScale = Mathf.Clamp(timeValue, 0, 1);
        //Debug.Log(_currentTimeScale);
    }
    public static void DeacreaseTimeScale(float timeValue)
    {
        _currentTimeScale = Mathf.Clamp(_currentTimeScale + timeValue, 0, 1);
        //Debug.Log(_currentTimeScale);
    }

    private static float _maxTime = 1f;
    private static bool _bigger = false;
    private static float _timeValue = 0f, _timeValueChangeSpeed = 0.08f; // 0.02f
    private static bool _isAccelerate = false;
    public static bool TimeAcceleration(bool isAccelerate)
    {
        if (GetTimeScale() > 0f && _isAccelerate == false)
        {
            _bigger = false;
            _isAccelerate = false;
            _timeValue = 0f;
            return false;
        }

        if (!_isAccelerate)
        {
            _isAccelerate = isAccelerate;
            if (!_isAccelerate)
            {
                _bigger = false;
                _isAccelerate = false;
                _timeValue = 0f;
            }
        }

        if (_timeValue < _maxTime && !_bigger)
        {
            _timeValue += _timeValueChangeSpeed;
            SetTimeScale(_timeValue);
            return true;
        }
        else if (_timeValue >= 0f)
        {
            _bigger = true;
            _timeValue -= _timeValueChangeSpeed;
            SetTimeScale(_timeValue);
            return true;
        }
        else
        {
            //Debug.Log(_timeValue + "  " + _isAccelerate);
            _bigger = false;
            _isAccelerate = false;
            _timeValue = 0f;
            return false;
        }
    }
}
/* 
    public static bool TimeAcceleration(float timeValue)
    {
        _timeValue = timeValue;
        if (_timeValue < _maxTime && !_bigger)
        {
            _timeValue += _timeValueChangeSpeed;
            SetTimeScale(_timeValue);
            return TimeAcceleration(_timeValue);
        }
        else if (_timeValue >= 0f)
        {
            _bigger = true;
            _timeValue -= _timeValueChangeSpeed;
            SetTimeScale(_timeValue);
            Debug.Log(_timeValue);
            return TimeAcceleration(_timeValue);
        }
        else
        {
            _bigger = false;
            _isAccelerate = false;
            return false;
        }
    } 
 */