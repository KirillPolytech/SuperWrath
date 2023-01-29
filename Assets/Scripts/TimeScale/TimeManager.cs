using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static float _currentTimeScale = 1f;
    public static float GetTimeScale()
    {
        return _currentTimeScale;
    }
    public static void SetTimeScale(float timeValue)
    {
        _currentTimeScale = Mathf.Clamp(timeValue, 0, 1);
        //Debug.Log(_currentTimeScale);
    }
}
