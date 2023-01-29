using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public int Speed = 10;
    public float RotationSpeed = 0.1f;
    public int DetectionDistance = 25;
    public int BulletVelocity = 25;
    public GameObject GunGameObject;
    public GameObject Bullet;
    public int GetHP { get { return HP; } }

    private int HP = 100;

    private Rigidbody _rigidbody;
    private GameObject Player;
    private Ray _detectionRay;

    private Vector3 _directionToPlayer;
    private Vector3 _moveDirection;
    private Quaternion _rotateDirection;


    private Ray _gunRay;
    private RaycastHit _hit;
    private RaycastHit _PlayerDetectionHit;

    private int _timer = 0;

    private ObjectsManager _objectsManager;
    private GameObject _tempBullet;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Player = GameObject.FindGameObjectWithTag("Player");

        _objectsManager = FindObjectOfType<ObjectsManager>();
    }
    private void FixedUpdate()
    {
        _directionToPlayer = Player.transform.position - GunGameObject.transform.position;
        PlayerDetection();
        Rotate();
        Movement();

        if (_timer > 10)
        {
            Gun();
            _timer = 0;
        }      
        _timer++;
        Debug.DrawLine(GunGameObject.transform.position, Player.transform.position, Color.black);
    }
    private void Movement()
    {
        _rigidbody.velocity = _moveDirection;
    }
    private void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, _rotateDirection, RotationSpeed);
    }
    private void PlayerDetection()
    {
        _detectionRay = new Ray(transform.position, _directionToPlayer);

        if (Physics.Raycast(_detectionRay, out _PlayerDetectionHit, DetectionDistance) 
            && _PlayerDetectionHit.transform.gameObject.CompareTag("Player"))
        {
            _moveDirection = _directionToPlayer.normalized * Speed;
            _rotateDirection = Quaternion.LookRotation(_directionToPlayer, Vector3.up);
        }
        else
        {
            _moveDirection = Vector3.zero;
            _rotateDirection = new(0,0,0,0);
        }
    }
    private void Gun()
    {
        GunGameObject.transform.rotation = Quaternion.LookRotation(_directionToPlayer);

        _gunRay = new(GunGameObject.transform.position, _directionToPlayer);

        if (Physics.Raycast(_gunRay, out _hit, DetectionDistance) && _hit.transform.gameObject.CompareTag("Player"))
        {
            _tempBullet = Instantiate(Bullet);

            _tempBullet.transform.SetPositionAndRotation(
                GunGameObject.transform.position + GunGameObject.transform.forward, 
                Quaternion.LookRotation(_gunRay.direction));

            _tempBullet.GetComponent<Rigidbody>().AddForce(GunGameObject.transform.forward * BulletVelocity);
            _tempBullet.GetComponent<Bullet>().InitialBullet(GunGameObject.transform.forward * BulletVelocity);
            _objectsManager.AddBulletToList(_tempBullet);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            GetDamage(100);
        }
    }
    public void DealDamage(HeroControllerWithAnimations hero)
    {
        hero.GetDamage(100);
    }
    public void GetDamage(int damage)
    {
        HP -= damage;
    }
}
/*
if ( Physics.SphereCast(transform.position, DetectionDistance, Player.transform.position - transform.position, out _PlayerDetectionHit) )
{
    _moveDirection = (Player.transform.position - transform.position).normalized * Speed;
    Debug.Log("Detected Player");
}
else if ( (Player.transform.position - transform.position).magnitude < 10)
{
    _moveDirection = Vector3.zero;
}
*/