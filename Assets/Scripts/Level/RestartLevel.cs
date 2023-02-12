using UnityEngine;
using UnityEngine.UI;

public class RestartLevel : MonoBehaviour
{
    private HeroController _hero;
    private UIScript _UI;
    private GameSystem _gameSystem;

    private RawImage _redScreen;

    private float _alpha = 0f;
    private void Start()
    {
        _hero = FindObjectOfType<HeroController>();
        _UI = FindObjectOfType<UIScript>();
        _gameSystem = FindObjectOfType<GameSystem>();
        _redScreen = _UI.GetRedScreenImage();
    }
    private void FixedUpdate()
    {
        TurnRedScreen();
        RestartCondition();
        //Debug.Log(__player.IsPlayerDead);
    }
    private void TurnRedScreen()
    {
        if (_hero.IsPlayerDead)
        {
            _alpha += Time.fixedDeltaTime;
            _UI.SetRedScreenColor(new Color(1f, 0, 0, _alpha));
            //Debug.Log(_alpha + "  " + _redScreen);
        }
    }
    private void RestartCondition()
    {
        if (_hero.IsPlayerDead && _redScreen.color.a >= 1.5f)
        {
            _gameSystem.RestartLevel();
        }
    }
}
//_redScreen.color = new Color(0.2f, 0, 0, _alpha);
