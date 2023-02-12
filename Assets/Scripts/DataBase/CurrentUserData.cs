using UnityEngine;

public class CurrentUserData : MonoBehaviour
{
    private static int _currentUserId = -1;
    private static string _currentEmail = null;
    public static int CurrentUserId{ get { return _currentUserId; } set { _currentUserId = Mathf.Clamp(value, 0, int.MaxValue); } }
    public static string CurrentEmail { get { return _currentEmail; } set { _currentEmail = value; } }
}
