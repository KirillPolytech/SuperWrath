using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _collsionParticleGameObject;
    [SerializeField] private GameObject _hitBodyGameObject;
    [SerializeField] private GameObject _hitWallGameObject;
    private Rigidbody _rb;
    private Vector3 _initialVelocity = Vector3.zero;
    private Vector3 _initialAngularVelocity = Vector3.zero;
    private int _numberOfCollisions = 0;
    public void InitialBullet(Vector3 velocity)
    {
        gameObject.tag = "Bullet";

        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.angularDrag = 0;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        _initialVelocity = velocity;

        _rb.velocity = Vector3.zero;
    }
    public void SetTimeScale()
    {
        _rb.velocity = _initialVelocity * TimeManager.GetTimeScale();
        _rb.angularVelocity = _initialAngularVelocity * TimeManager.GetTimeScale();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && _numberOfCollisions < 5)
        {
            _numberOfCollisions++;
            _hitBodyGameObject.GetComponent<AudioSource>().Play();
            return;
        }
        //Debug.Log(collision.gameObject.name + "   " + transform.position );


        //ChangeDirectionOnCollision(collision);

        _collsionParticleGameObject.transform.parent = null;
        ParticleSystem _temp = _collsionParticleGameObject.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule _module = _collsionParticleGameObject.GetComponent<ParticleSystem>().main;
        _module.playOnAwake = false;
        _module.loop = false;
        _temp.Play();

        _hitWallGameObject.GetComponent<AudioSource>().Play();

        Destroy(gameObject);
    }
    private void ChangeDirectionOnCollision(Collision collision)
    {
        _initialVelocity = Vector3.Reflect(_initialVelocity, collision.GetContact(0).normal);
        _rb.velocity = _initialVelocity * TimeManager.GetTimeScale();
        _rb.angularVelocity = _initialAngularVelocity * TimeManager.GetTimeScale();
        transform.rotation = Quaternion.LookRotation(_initialVelocity);
    }
}
//transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_initialVelocity.x, _initialVelocity.y, _initialVelocity.z), Mathf.Infinity);