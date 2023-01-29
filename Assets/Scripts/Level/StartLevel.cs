using UnityEngine;

public class StartLevel : MonoBehaviour
{
    private UIScript _UI;
    private HeroControllerWithAnimations _hero;

    private float _alphaValue = 1.5f;
    private void Awake()
    {
        _hero = FindObjectOfType<HeroControllerWithAnimations>();
        _UI = FindObjectOfType<UIScript>();
    }
    private void Start()
    {
        _UI.SetBlackScreenColor( new Color(0, 0, 0, 1.5f) );
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
            _UI.SetBlackScreenColor(new Color(0, 0, 0, 0));
        }
    }
}
