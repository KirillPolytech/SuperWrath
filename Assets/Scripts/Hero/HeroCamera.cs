using UnityEngine;

public class HeroCamera : MonoBehaviour
{
    [SerializeField] private float _sensivity = 5f;
    [SerializeField] private int _maxRayDistance = 3;

    private float _xRotation = 0f;
    private float _mouseX, _mouseY;
    private GameObject _playerBody;
    private Ray _RayFromCamera;
    private RaycastHit _hit;
    private void Awake()
    {
        _playerBody = GameObject.FindGameObjectWithTag("Player");
        gameObject.tag = "MainCamera";
        gameObject.layer = 6;
    }
    private void FixedUpdate()
    {
        CameraMovement();
        GetHitPointOn3Meters();
        GetHitPointOn500Meters();
        ChangeColorOfGun();
    }
    private GameObject _tempGun, __tempTempGun;
    private void ChangeColorOfGun()
    {
        _tempGun = GetHittedGameObjectOn3Meters();

        if (_tempGun && _tempGun.GetComponent<Gun>() && !__tempTempGun)
        {
            _tempGun.GetComponent<Pistol>().ChangeColor(new Color(0.4f, 0.4f, 1f, 1f));
            __tempTempGun = _tempGun;
        }
        else if (__tempTempGun && _tempGun != __tempTempGun)
        {
            __tempTempGun.GetComponent<Pistol>().ReturnInitialColor();
            __tempTempGun = null;
        }
    }
    private void CameraMovement()
    {
        _mouseX = ButtonsManager.HorizontalMouseInputValue() * _sensivity;
        _mouseY = ButtonsManager.VerticalMouseInputValue() * _sensivity;

        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -60f, 60f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerBody.transform.Rotate(Vector3.up * _mouseX);
    }
    public Vector3 GetHitPointOn3Meters()
    {
        _RayFromCamera = new Ray(transform.position, transform.forward);
        Debug.DrawLine(_RayFromCamera.origin, _hit.point, Color.red);
        if (Physics.Raycast(_RayFromCamera, out _hit, _maxRayDistance))
        {
            return _hit.point;
        }
        else
        {
            return _RayFromCamera.GetPoint(_maxRayDistance);
        }        
    }
    public Vector3 GetHitPointOn500Meters()
    {
        _RayFromCamera = new Ray(transform.position, transform.forward);
        Debug.DrawLine(_RayFromCamera.origin, _hit.point, Color.red);
        if (Physics.Raycast(_RayFromCamera, out _hit, 500f))
        {
            return _hit.point;
        }
        else
        {
            return _RayFromCamera.GetPoint(_maxRayDistance);
        }
    }
    public GameObject GetHittedGameObjectOn3Meters()
    {
        if (Physics.Raycast(_RayFromCamera, out _hit, _maxRayDistance))
        {
            return _hit.transform.gameObject;
        }
        return null;
    }
    public GameObject GetHittedGameObjectOn500Meters()
    {
        if (Physics.Raycast(_RayFromCamera, out _hit, 500f))
        {
            return _hit.transform.gameObject;
        }
        return null;
    }
    public float GetDistanceToHittedObject()
    {
        if (Physics.Raycast(_RayFromCamera, out _hit, _maxRayDistance))
        {
            return (_hit.transform.gameObject.transform.position - transform.position).magnitude;
        }
        return 0;
    }
}
