using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WindSound : MonoBehaviour
{
    private AudioSource _windSound;
    private void Awake()
    {
        _windSound = GetComponent<AudioSource>();
        _windSound.playOnAwake = true;
        _windSound.spatialBlend = 1;
        _windSound.loop = true;
    }
    private void FixedUpdate()
    {
        ChangeSoundPitchScript.ChangePitch(_windSound);
    }
}
