using UnityEngine;

public class PassingCarSound : MonoBehaviour
{
    private AudioSource __passingCarSound;
    private void Awake()
    {
        __passingCarSound = GetComponent<AudioSource>();
        __passingCarSound.playOnAwake = true;
        __passingCarSound.spatialBlend = 1;
        __passingCarSound.loop = true;
    }
    private void FixedUpdate()
    {
        ChangeSoundPitchScript.ChangePitch(__passingCarSound);
    }
}
