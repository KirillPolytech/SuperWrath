using UnityEngine;

public class HeroCamera : MonoBehaviour
{
    [SerializeField] private float _sensivity = 5f;
    [SerializeField] private Color _color;

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
        _RayFromCamera = new Ray(transform.position, transform.forward);

        CameraMovement();
        ChangeColor();
    }
    private GameObject _tempColor, __tempTempColor;
    private void ChangeColor()
    {
        _tempColor = GetHittedGameObject(3);
        if (_tempColor && !__tempTempColor && _tempColor.GetComponent<IChangeColor>() != null) 
        {
            _tempColor.GetComponent<IChangeColor>().ChangeColor(_color);//new Color(0.4f, 0.4f, 1f, 1f));
            __tempTempColor = _tempColor;
        }else if (__tempTempColor && _tempColor != __tempTempColor)
        {
            __tempTempColor.GetComponent<IChangeColor>().ReturnColor();
            __tempTempColor = null;
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
    public Vector3 GetHitPoint(float meters) // to pick up item - 3 meters, other 500
    {
        if (Physics.Raycast(_RayFromCamera, out _hit, meters))
        {
            return _hit.point;
        }
        else
        {
            return _RayFromCamera.GetPoint(meters);
        }
    }
    public GameObject GetHittedGameObject(float meters) // 3 meter to pick up
    {
        if (Physics.Raycast(_RayFromCamera, out _hit, meters))
        {
            return _hit.transform.gameObject;
        }
        return null;
    }
    public float GetDistanceToHittedObject(float meters)
    {
        if (Physics.Raycast(_RayFromCamera, out _hit, meters))
        {
            return (_hit.transform.gameObject.transform.position - transform.position).magnitude;
        }
        return 0;
    }
}


/*public Vector3 GetHitPointOn3Meters()
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
*/
/*
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
*/

/*
    private void ChangeColorOfGun()
    {
        _tempGun = GetHittedGameObject(3);
        if (_tempGun && _tempGun.GetComponent<Gun>() && !__tempTempGun && !_tempGun.transform.parent)
        {;
            _tempGun.GetComponent<Gun>().ChangeColor(_color);//new Color(0.4f, 0.4f, 1f, 1f));
            __tempTempGun = _tempGun;
        }
        else if (__tempTempGun && _tempGun != __tempTempGun)
        {          
            __tempTempGun.GetComponent<Gun>().ReturnInitialColor();
            __tempTempGun = null;
        }
    }
    private void ChangeColorOfProp()
    {
        _tempProp = GetHittedGameObject(3);

        if (_tempProp && _tempProp.GetComponent<Prop>() && !__tempTempProp)
        {
            //_tempProp.GetComponent<Prop>().ChangeColor(_color);//new Color(0.4f, 0.4f, 1f, 1f));
            __tempTempProp = _tempProp;
        }
        else if (__tempTempProp && _tempProp != __tempTempProp)
        {
            //__tempTempProp.GetComponent<Prop>().ReturnColor();
            __tempTempProp = null;
        }
    }
    */