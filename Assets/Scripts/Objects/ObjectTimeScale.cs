using UnityEngine;

public class ObjectTimeScale : MonoBehaviour
{
    private Rigidbody _rb = null;
    private Vector3 _initialVelocity = Vector3.zero;
    private Vector3 _initialAngularVelocity = Vector3.zero;
    public Vector3 GetVelocity { get { return _initialVelocity; } }
    public Vector3 GetAngularVelocity { get { return _initialAngularVelocity; } }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _initialVelocity = Physics.gravity;
    }
    public void InitialVelocityAndAngularVelocity(Vector3 velocity, Vector3 angularVelocity)
    {
        _initialVelocity = velocity;
        _initialAngularVelocity = angularVelocity;
    }
    public void SetTimeScale()
    {
        _rb.velocity = _initialVelocity * TimeManager.GetTimeScale();
        _rb.angularVelocity = _initialAngularVelocity * TimeManager.GetTimeScale();
    }

    private void FixedUpdate()
    {
        //DecreaseVelocity();
    }
    private void DecreaseVelocity()
    {
        if (TimeManager.GetTimeScale() > 0f)
        {
            _initialVelocity = Vector3.Scale(_initialVelocity, new Vector3(0.99f, 1f, 0.99f));

            if (_initialVelocity.y > Physics.gravity.y )
            {
                _initialVelocity = new Vector3(_initialVelocity.x, _initialVelocity.y + Physics.gravity.y * Time.fixedDeltaTime, _initialVelocity.z);
                //Debug.Log(_initialVelocity.y);
            }
        }
    }
}

/*
if (_timeScale == 0f)
    _rb.constraints = RigidbodyConstraints.FreezeAll;
else
    _rb.constraints = RigidbodyConstraints.None;
*/
/*
 *     private Vector3 _temporary;
_temporary = new Vector3(_initialVelocity.x * Time.fixedDeltaTime, _initialVelocity.y, _initialVelocity.z * Time.fixedDeltaTime);
float x = _initialVelocity.x < 0 ? x = -1 : x = 1;
float z = _initialVelocity.z < 0 ? z = -1 : z = 1;
_initialVelocity = new Vector3(Mathf.Clamp(_initialVelocity.x - _temporary.x, -1000, 1000), _initialVelocity.y, Mathf.Clamp(_initialVelocity.z - _temporary.z, -1000, 1000));
*/

//private static float _timeScale = 0f;
