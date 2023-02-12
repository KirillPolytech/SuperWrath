using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(NavMeshObstacle))]
[RequireComponent(typeof(ObjectTimeScale))]
[RequireComponent(typeof(Rigidbody))]
public class Prop : MonoBehaviour, IChangeColor
{
    [SerializeField] private Vector3 _position = new Vector3(0.4f, 1f, 1f);
    private Rigidbody _rb;
    private NavMeshObstacle _navMeshObstacle;
    private ObjectTimeScale _objectTimeScale;
    private bool _isObjectDestroyed = false;
    public bool IsObjectDestroyed { get { return _isObjectDestroyed; } }
    private bool _isDestroyed = false;
    public bool IsPropDestroyed { get { return _isDestroyed; } set { _isDestroyed = value; }   }
    private Renderer _renderer;
    private Color _initialColor;
    private bool _isPickedUp = false;
    private void Awake()
    {
        gameObject.tag = "Prop";
        gameObject.layer = 10;

        _navMeshObstacle = GetComponent<NavMeshObstacle>();
        _navMeshObstacle.size = new Vector3(0.001f, 0.001f, 0.007f); // new Vector3(0.005f, 0.0125f, 0.0125f)

        __dropSource = GetComponent<AudioSource>();
        __dropSource.playOnAwake = false;
        __dropSource.volume = SoundsVolume.GetSoundsVolume();
        __dropSource.volume = 0.05f;
        __dropSource.clip = Resources.Load<AudioClip>("Sounds\\DropSounds\\Drop");

        _objectTimeScale = GetComponent<ObjectTimeScale>();
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        _renderer = GetComponent<Renderer>();
        _initialColor = _renderer.material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Player"))
        {
            Rigidbody _collisionRigidBody = collision.gameObject.GetComponent<Rigidbody>();
            if (_collisionRigidBody.velocity.magnitude > 0.1f)
            {
                _isObjectDestroyed = true;

                _rb.constraints = RigidbodyConstraints.None;

                Vector3 _initialVelocity = _objectTimeScale.GetVelocity;
                Vector3 _initialAngularVelocity = _objectTimeScale.GetAngularVelocity;

                _initialVelocity += _collisionRigidBody.velocity.normalized;
                _initialAngularVelocity += _collisionRigidBody.angularVelocity.normalized;

                _objectTimeScale.InitialVelocityAndAngularVelocity(_initialVelocity, _initialAngularVelocity);

                _isDestroyed = true;
            }
        }
        else if (!collision.gameObject.CompareTag("Prop"))
        {
            if (_rb.velocity.magnitude > 1f)
            {
                __dropSource.Play();
            }
        }
    }
    private void FixedUpdate()
    {
        PlayDropSound();
        if (_isDestroyed)
        {
            _rb.constraints = RigidbodyConstraints.None;
        }else if(TimeManager.GetTimeScale() == 0f)
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        if (_isPickedUp && transform.localPosition != _position)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _position, 0.2f);
        }
    }

    [SerializeField] private float _musicChangeDelay = 0.02f;
    private AudioSource __dropSource;
    private float _delay = 0;
    private void PlayDropSound()
    {
        if (_delay > _musicChangeDelay)
        {
            if (TimeManager.GetTimeScale() > __dropSource.pitch)
            {
                __dropSource.pitch = Mathf.Clamp(__dropSource.pitch + Time.fixedDeltaTime, 0, 1);
                _delay = 0;
            }
            else
            {
                __dropSource.pitch = Mathf.Clamp(__dropSource.pitch - Time.fixedDeltaTime, 0, 1);
                _delay = 0;
            }
        }
        _delay += Time.fixedDeltaTime;
    }
    public void BeingThrown(Vector3 direction, Vector3 angular, float __throwingForce)
    {
        _isPickedUp = false;
        _objectTimeScale.InitialVelocityAndAngularVelocity(direction * __throwingForce + Physics.gravity * 0.1f, angular);
    }

    public void ChangeColor(Color color)
    {
        _renderer.material.color = color;
    }
    public void ReturnColor()
    {
        _renderer.material.color = _initialColor;
    }
    public void PickUp()
    {
        _isPickedUp = true;
    }
}



/*
 * 
 *     private static float _timeScale = 0f;
    readonly private float G = -9.81f;
_initialVelocity = new Vector3(0, - 2f, 0); //G * Time.fixedDeltaTime * 10
 * 
 public void SetTimeScale()
 {
     _timeScale = TimeManager.GetTimeScale();

     _rb.velocity = _initialVelocity * _timeScale;
     _rb.angularVelocity = _initialAngularVelocity * _timeScale;
     if (_timeScale == 0f)
          _rb.constraints = RigidbodyConstraints.FreezeAll;
     else
         _rb.constraints = RigidbodyConstraints.None;
 }
 */

/*
_rb = GetComponent<Rigidbody>();
_rb.useGravity = false;
_rb.constraints = RigidbodyConstraints.FreezeAll;
*/

/*
if (TimeManager.GetTimeScale() == 0f)
{
    _rb.constraints = RigidbodyConstraints.FreezeAll;
}
else if (_isDestroyed)
{
    _rb.constraints = RigidbodyConstraints.None;
}
*/