using UnityEngine;

public class ChangeSoundPitchScript : MonoBehaviour
{
    public static void ChangePitch(AudioSource _audio)
    {
        if (TimeManager.GetTimeScale() >= _audio.pitch)
        {
            _audio.pitch = Mathf.Clamp(_audio.pitch + Time.fixedDeltaTime, 0, 1);
        }
        else
        {
            _audio.pitch = Mathf.Clamp(_audio.pitch - Time.fixedDeltaTime, 0, 1);
        }
    }
}
