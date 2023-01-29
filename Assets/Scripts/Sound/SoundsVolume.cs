using UnityEngine;

public class SoundsVolume : MonoBehaviour
{
    private static float __soundsVolume = 0.5f;
    private static float __musicVolume = 0.7f;
    private static GameObject __soundsVolumeGameObject = null;
    private void Awake()
    {
        if (!__soundsVolumeGameObject)
        {
            DontDestroyOnLoad(this);
            __soundsVolumeGameObject = gameObject;
        }
        else
            Destroy(this);
    }
    public static float GetSoundsVolume()
    {
        return __soundsVolume;
    }
    public static void SetSoundsVolume(float value)
    {
        __soundsVolume = value;
    }
    public static float GetMusicVolume()
    {
        return __musicVolume;
    }
}
