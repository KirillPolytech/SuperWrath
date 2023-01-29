using TMPro;
using UnityEngine;

public class ChangeTextFont : MonoBehaviour
{
    [SerializeField] private float _fontChangeDelay = 0.02f;
    [SerializeField] private float _fontChangeSpeed = 0.02f;
    [SerializeField] private float __minimumTextFont = 50f;
    [SerializeField] private float __maximumTextFont = 100f;
    private float _fontSize = 75f;
    private float _delay = 0;
    private bool _bigger = false;

    [SerializeField] private TextMeshProUGUI _text1;
    [SerializeField] private TextMeshProUGUI _text2;
    [SerializeField] private float _textChangeDelay = 0.02f;
    private float _textDelay = 0f;
    private bool _nextText = false;

    [SerializeField]private AudioSource _superWrathSound;
    private HeroControllerWithAnimations _hero;
    private void Awake()
    {
        _hero = FindObjectOfType<HeroControllerWithAnimations>();

        _superWrathSound.playOnAwake = false;
        _superWrathSound.loop = true;

        _text1.enabled = false;
        _text2.enabled = false;

        _superWrathSound.volume = SoundsVolume.GetSoundsVolume();
    }
    private void FixedUpdate()
    {
        if (!_superWrathSound.isPlaying && _hero.IsPlayerWon)
        {
            _superWrathSound.Play();
        }

        TextChange();
    }
    private void TextChange()
    {
        if (_textDelay > _textChangeDelay)
        {
            if (_fontSize < __maximumTextFont && !_nextText)
            {
                _text1.enabled = true;
                _text2.enabled = false;

                FontChange(_text1);
            }
            else if (_fontSize > __minimumTextFont)
            {
                _nextText = true;

                _text1.enabled = false;
                _text2.enabled = true;

                FontChange(_text2);
            }
            else
            {
                _nextText = false;
            }

            _textDelay = 0;
        }

        _textDelay += Time.fixedDeltaTime;
    }
    private void FontChange(TextMeshProUGUI __text)
    {
        if (_delay > _fontChangeDelay)
        {
            if (_fontSize < __maximumTextFont && !_bigger)
            {
                _fontSize += _fontChangeSpeed;
            }
            else if (_fontSize >= __minimumTextFont)
            {
                _bigger = true;
                _fontSize -= _fontChangeSpeed;
            }
            else
            {
                _bigger = false;
            }

            _delay = 0;
        }
        __text.fontSize = _fontSize;
        _delay += Time.fixedDeltaTime;
    }
}
