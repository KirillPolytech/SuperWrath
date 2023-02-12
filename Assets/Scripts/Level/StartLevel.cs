using UnityEngine;

public class StartLevel : MonoBehaviour
{
    private UIScript _UI;
    private HeroController _hero;
    private HeroCamera _heroCamera;

    private float _alphaValue = 1.5f;
    private void Awake()
    {
        _hero = FindObjectOfType<HeroController>();
        _heroCamera = FindObjectOfType<HeroCamera>();
        _UI = FindObjectOfType<UIScript>();
    }
    private void Start()
    {
        _UI.SetBlackScreenColor( new Color(0, 0, 0, 1.5f) );
        _heroCamera.enabled = false;
        _hero.enabled = false;
    }
    private void FixedUpdate()
    {
        if (_hero.IsPlayerDead)
            return;
        if (_UI.GetBlackScreenColor().a > 0f)
        {
            _UI.SetBlackScreenColor(new Color(0, 0, 0, _alphaValue));
            _alphaValue -= Time.fixedDeltaTime;
        }   
        else
        {
            EnableCamera();
            EnableHero();
            _UI.SetBlackScreenColor(new Color(0, 0, 0, 0));
        }
    }
    private void EnableCamera()
    {
        _heroCamera.enabled = true;
    }
    private void EnableHero()
    {
        _hero.enabled = true;
    }
}
