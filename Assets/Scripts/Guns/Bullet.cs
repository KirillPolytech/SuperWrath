using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Bullet : MonoBehaviour
{
    private static float _timeScale = 0f;
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
        _timeScale = TimeManager.GetTimeScale();

        _rb.velocity = _initialVelocity * _timeScale;
        _rb.angularVelocity = _initialAngularVelocity * _timeScale;
    }
    private void OnCollisionEnter(Collision collision)
    {
        _numberOfCollisions++;
        //Debug.Log(collision.gameObject.name + "   " + transform.position );


        //ChangeDirectionOnCollision(collision);

        Destroy(gameObject);
    }
    private void ChangeDirectionOnCollision(Collision collision)
    {
        _initialVelocity = Vector3.Reflect(_initialVelocity, collision.GetContact(0).normal);
        _rb.velocity = _initialVelocity * _timeScale;
        _rb.angularVelocity = _initialAngularVelocity * _timeScale;
        transform.rotation = Quaternion.LookRotation(_initialVelocity);
    }
}
//transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_initialVelocity.x, _initialVelocity.y, _initialVelocity.z), Mathf.Infinity);