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
        if (Input.GetButtonDown("R"))
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
    public void RestartLevel()
    {
        SceneManager.LoadScene("" + SceneManager.GetActiveScene().name);
    }
}
