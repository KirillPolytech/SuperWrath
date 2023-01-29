using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    private HeroControllerWithAnimations _hero;
    private UIScript _UI;
    private void Awake()
    {
        _hero = FindObjectOfType<HeroControllerWithAnimations>();
        _UI = FindObjectOfType<UIScript>();
    }
    private void FixedUpdate()
    {
        if (_hero.IsPlayerDead && _UI.GetBlackScreenColor().a >= 1.5f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
