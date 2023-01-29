using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    private static bool __leftMouseButtonValue = false, __rightMouseButtonValue;
    private static float __horizontalAxisValue = 0f, __verticalAxisValue = 0f;
    private static float __mouseX = 0f, __mouseY = 0f;

    private static float __delayBetweenPress = 0f, __delayLimit = 1f;
    private static bool __isDelayOn = false;
    private void Update()
    {
        if (__isDelayOn)
        {
            if (__delayBetweenPress <= __delayLimit)
            {
                __delayBetweenPress += Time.fixedDeltaTime;
            }
            else
            {
                __delayBetweenPress = 0f;
                __isDelayOn = false;
            }
        }

        __leftMouseButtonValue = Input.GetButton("Fire1");
        __rightMouseButtonValue = Input.GetButton("Fire2");

        __horizontalAxisValue = Input.GetAxis("Horizontal");
        __verticalAxisValue = Input.GetAxis("Vertical");

        __mouseX = Input.GetAxis("Mouse X");
        __mouseY = Input.GetAxis("Mouse Y");
    }
    public static bool IsLeftMousePressed()
    {
        return __leftMouseButtonValue;
    }
    public static bool IsRightMousePressed()
    {
        return __rightMouseButtonValue;
    }
    public static float HorizontalInput()
    {
        return __horizontalAxisValue;
    }
    public static float VerticalInput()
    {
        return __verticalAxisValue;
    }
    public static float HorizontalMouseInputValue()
    {
        return __mouseX;
    }
    public static float VerticalMouseInputValue()
    {
        return __mouseY;
    }
    public static void SetDelayBetweenPress()
    {
        if (!__isDelayOn)
            __isDelayOn = true;
    }
    public static bool GetDelayBetweenPress()
    {
        if (__delayBetweenPress > __delayLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
