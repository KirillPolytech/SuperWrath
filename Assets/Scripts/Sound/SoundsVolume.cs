using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundsVolume : MonoBehaviour
{
    [SerializeField] private static Slider _volumeSlider;
    [SerializeField] private static Slider _musicSlider;
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
        {
            Destroy(this);
        }

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            _volumeSlider = GameObject.Find("GameSettings").transform.GetChild(3).GetComponent<Slider>();
            _musicSlider = GameObject.Find("GameSettings").transform.GetChild(4).GetComponent<Slider>();
        }
    }
    public static void UpdateVolumeFromSlider()
    {
        __soundsVolume = _volumeSlider.value;
        __musicVolume = _musicSlider.value;
        //Debug.Log(__soundsVolume);
    }
    public static void SetSoundsVolume(float value)
    {
        __soundsVolume = value;
    }
    public static float GetMusicVolume()
    {
        return __musicVolume;
    }
    public static float GetSoundsVolume()
    {
        return __soundsVolume;
    }
}
