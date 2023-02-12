using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private Quaternion _settingsRotation;
    [SerializeField] private Quaternion _mainMenuRotation;
    [SerializeField] private Quaternion _leaderBoardRotation;
    [SerializeField] private float _rotationSpeed = 0.01f;
    private GameObject _mainCamera;
    private bool _isRotatingToSettings = false;
    private bool _isRotatingToMainMenu = false;
    private bool _isRotatingToLeaderBoard = false;
    private void Awake()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    private void Update()
    {
        CameraRotation();
    }
    private void CameraRotation()
    {
        if (_isRotatingToSettings)
        {
            if (_mainCamera.transform.rotation != _settingsRotation)
                _mainCamera.transform.rotation = Quaternion.Lerp(_mainCamera.transform.rotation, _settingsRotation, _rotationSpeed);
            else
                _isRotatingToSettings = false;
        }
        else if (_isRotatingToMainMenu)
        {
            if (_mainCamera.transform.rotation != _mainMenuRotation)
                _mainCamera.transform.rotation = Quaternion.Lerp(_mainCamera.transform.rotation, _mainMenuRotation, _rotationSpeed);
            else
                _isRotatingToMainMenu = false;
        }
        else if (_isRotatingToLeaderBoard)
        {
            if (_mainCamera.transform.rotation != _leaderBoardRotation)
                _mainCamera.transform.rotation = Quaternion.Lerp(_mainCamera.transform.rotation, _leaderBoardRotation, _rotationSpeed);
            else
                _isRotatingToLeaderBoard = false;
        }
    }
    public void TurnCameraToSettings()
    {        
        _isRotatingToSettings = true;
        _isRotatingToMainMenu = false;
        _isRotatingToLeaderBoard = false;
    }
    public void TurnCameraToMainMenu()
    {        
        _isRotatingToMainMenu = true;
        _isRotatingToSettings = false;
        _isRotatingToLeaderBoard = false;
    }
    public void TurnCameraToLeaderBoard()
    {
        _isRotatingToLeaderBoard = true;
        _isRotatingToMainMenu = false;
        _isRotatingToSettings = false;
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
