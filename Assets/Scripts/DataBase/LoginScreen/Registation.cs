using UnityEngine;
using TMPro;

public class Registation : MonoBehaviour
{
    [SerializeField] private TMP_InputField _loginInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TextMeshProUGUI __errorText;
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            AccesToDataBase.RegisterUser(_loginInputField.text, _passwordInputField.text, _emailInputField.text);
            Debug.Log("Inserted data");
        }
    }
}
