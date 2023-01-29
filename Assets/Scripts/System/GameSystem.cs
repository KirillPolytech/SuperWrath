using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 60;
    }
    private void Update()
    {
        RestartLevel();
        QuitGame();
    }
    private void QuitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    private void RestartLevel()
    {
        if (Input.GetButtonDown("R"))
        {
            SceneManager.LoadScene("" + SceneManager.GetActiveScene().name);
        }
    }
}
