using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    private AudioSource _musicSource;
    private HeroController _hero;
    private void Awake()
    {
        _hero = FindObjectOfType<HeroController>();

        _musicSource = GetComponent<AudioSource>();
        _musicSource.playOnAwake = false;
        _musicSource.volume = SoundsVolume.GetMusicVolume();
    }
    private void FixedUpdate()
    {
        if (!_hero.IsPlayerWon)
            MusicPlaying();
        else
            _musicSource.Pause();
    }
    private void MusicPlaying()
    {
        ChangeSoundPitchScript.ChangePitch(_musicSource);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            _musicSource.Pause();
        }
    }
}
/*
 *     [SerializeField] private float _musicChangeDelay = 0.02f;
if (_delay > _musicChangeDelay)
{
    if (TimeManager.GetTimeScale() >= _musicSource.pitch)
    {
        _musicSource.pitch = Mathf.Clamp(_musicSource.pitch  + Time.fixedDeltaTime, 0, 1);
        _delay = 0;
    }
    else
    {
        _musicSource.pitch = Mathf.Clamp(_musicSource.pitch - Time.fixedDeltaTime, 0, 1);
        _delay = 0;
    }

}
*/

/*
if (TimeManager.GetTimeScale() >= _musicSource.pitch)
{
    _musicSource.pitch = Mathf.Clamp(_musicSource.pitch + Time.fixedDeltaTime, 0, 1);
}
else
{
    _musicSource.pitch = Mathf.Clamp(_musicSource.pitch - Time.fixedDeltaTime, 0, 1);
}
*/