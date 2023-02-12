using TMPro;
using UnityEngine;

public class LogIn : MonoBehaviour
{
    [SerializeField] private TMP_InputField _loginInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TextMeshProUGUI __errorText;

    [SerializeField] private GameObject __logInWindow;
    [SerializeField] private GameObject __recordsWindow;
    private bool __isLogIn = false;
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            __isLogIn = AccesToDataBase.LoginUser(_loginInputField.text, _passwordInputField.text);
            if ( !__isLogIn)
            {
                __errorText.text = "Error";
            }
            else
            {
                __errorText.text = "";
            }
            //Debug.Log("Finding: " + _loginInputField.text + "  " + _passwordInputField);
        }

        if (__isLogIn)
        {
            __recordsWindow.SetActive(true);
            __logInWindow.SetActive(false);
        }
    }
}
