using UnityEngine;

public class HeroWeaponCamera : MonoBehaviour
{
    private GameObject __mainCamera;
    private void Awake()
    {
        __mainCamera = Camera.main.gameObject;
    }
    private void FixedUpdate()
    {
        transform.position = __mainCamera.transform.position;
        transform.rotation = __mainCamera.transform.rotation;
    }
}
