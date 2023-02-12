using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ObjectTimeScale))]
public class Pistol : FireArmWeapon
{
    [SerializeField] private AudioSource _attackSound;
    [SerializeField] private AudioSource _emptyShotSound;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private ParticleSystem _fireParticleSystem;
    [Range(1, 50)]
    [SerializeField] private int _bulletVelocity = 25;

    [Range(0.2f, 1f)]
    [SerializeField] private float _delayBetweenShots = 1f;
    private float _delay = 0f;

    [Range(0, 1f)]
    [SerializeField] private float __liftDelayTime = 0.2f;

    private ObjectsManager _objectsManager;
    private GameObject _tempBullet;
    private Rigidbody _rigidBody;
    private HeroCamera _camera;
    private ObjectTimeScale _objectTimeScale;
    private void Awake()
    {
        gameObject.layer = 7;
        gameObject.tag = "Gun";
        _ammo = 7;

        _objectTimeScale = GetComponent<ObjectTimeScale>();
        _camera = FindObjectOfType<HeroCamera>();
        _objectsManager = FindObjectOfType<ObjectsManager>();
        _rigidBody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();

        _rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        _attackSound.playOnAwake = false;
        _emptyShotSound.playOnAwake = false;
        _initialColor = Color.black;
        _rigidBody.useGravity = false;
        _attackSound.pitch = 1.5f;
        _attackSound.volume = SoundsVolume.GetSoundsVolume();
        _emptyShotSound.volume = SoundsVolume.GetSoundsVolume();

        var _fireParticleSystemMain = _fireParticleSystem.main;
        _fireParticleSystemMain.loop = false;
        _fireParticleSystemMain.playOnAwake = false;
    }
    private bool _isparent = false;
    private void FixedUpdate()
    {
        if (!transform.parent)
            return;

        PistolPosAndRot();
    }
    private void PistolPosAndRot()
    {
        if (transform.parent && !_isparent)
        {
            //transform.localPosition = _gunPosition;//new Vector3(-0.05f, 0.39f, 0.08f);
            transform.rotation = Quaternion.LookRotation(_camera.gameObject.transform.forward);
            _isparent = true;
            _rigidBody.isKinematic = true;
            //Debug.Log("Parented");
        }
        if (transform.parent.CompareTag("Enemy"))
        {
            transform.localPosition = _gunPosition;//new Vector3(-0.271f, 0.385f, 0.254f);
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _gunPosition, __liftDelayTime);
        }
        transform.rotation = Quaternion.LookRotation(_camera.GetHitPoint(500) - transform.position);
    }
    private void Update()
    {
        if (_isparent)
            Attack();
        ChangingSoundSpeed();
    }
    public override void Attack()
    {
        if (_ammo == 0)
        {
            if (ButtonsManager.IsLeftMousePressed() && !_emptyShotSound.isPlaying)
                _emptyShotSound.Play();
            return;
        }
        if (_delay > _delayBetweenShots && ButtonsManager.IsLeftMousePressed())
        {
            _tempBullet = Instantiate(_bullet);
            _tempBullet.gameObject.tag = "Bullet";
            _tempBullet.transform.SetPositionAndRotation(transform.position + transform.forward, Quaternion.LookRotation(_camera.GetHitPoint(500) - transform.position));
            _tempBullet.GetComponent<Bullet>().InitialBullet((_camera.GetHitPoint(500) - transform.position).normalized * _bulletVelocity);
            _objectsManager.AddBulletToList(_tempBullet);

            DecreaseAmmo();
            ShotSound();
            FireParticles();

            _delay = 0f;
        }
        _delay += Time.deltaTime;
    }
    public override void BeingThrown(float __throwingForce, Vector3 direction)
    {
        _objectTimeScale.InitialVelocityAndAngularVelocity(direction * __throwingForce + Physics.gravity * 0.1f, _rigidBody.angularVelocity);
        _objectsManager.AddObjectToList(GetComponent<ObjectTimeScale>());
    }
    private void DecreaseAmmo()
    {
        _ammo--;
    }
    public override void UnParent()
    {
        _isparent = false;
        transform.parent = null;
        _rigidBody.isKinematic = false;
        _rigidBody.constraints = RigidbodyConstraints.None;
        gameObject.AddComponent<BoxCollider>();
    }

    private void ChangingSoundSpeed()
    {
        if (_attackSound.isPlaying)
        {
            if (TimeManager.GetTimeScale() >= _attackSound.pitch)
            {
                _attackSound.pitch = Mathf.Clamp(_attackSound.pitch + Time.fixedDeltaTime, 0, 1);
            }
            else
            {
                _attackSound.pitch = Mathf.Clamp(_attackSound.pitch - Time.fixedDeltaTime, 0, 1);
            }
        }
    }
    private void ShotSound()
    {
        _attackSound.pitch = Mathf.Clamp(TimeManager.GetTimeScale(), 0.1f, 1f);
        _attackSound.Play();
    }
    private void FireParticles()
    {
        _fireParticleSystem.Play();
    }
    public override string GetName()
    {
        return GetType().ToString();
    }

    public override void ChangeColor(Color color)
    {
        _renderer.material.color = color;
    }
    public override void ReturnColor()
    {
        _renderer.material.color = _initialColor;
    }
}

//_rigidBody.velocity = direction * __throwingForce + Physics.gravity * 0.1f;
//if (TimeManager.GetTimeScale() == 1f)
// _objectTimeScale.InitialVelocityAndAngularVelocity(new Vector3(_rigidBody.velocity.x, Physics.gravity.y * 0.1f, _rigidBody.velocity.z), _rigidBody.angularVelocity);
//_tempBullet.GetComponent<Rigidbody>().velocity = (_camera.GetHitPoint() - transform.position).normalized * _bulletVelocity * TimeManager.GetTimeScale();        

/*
 *     //private float _delay = 0;
 *     private float _timeChangeDelay = 0.02f;
if (_delay > _timeChangeDelay)
{
    if (_timeValue < _maxTime && !_bigger)
    {
        _timeValue += _timeValueChangeSpeed;
        TimeManager.SetTimeScale(_timeValue);                 
    }
    else if (_timeValue >= 0f)
    {
        _bigger = true;
        _timeValue -= _timeValueChangeSpeed;
        TimeManager.SetTimeScale(_timeValue);
    }
    else
    {
        _bigger = false;
        _isAttack = false;
    }

    _delay = 0;
}
_delay += Time.fixedDeltaTime;
*/

/*timeacceleration
 *     [SerializeField] private float _maxTime = 1f;
    private float _timeValue = 0f, _timeValueChangeSpeed = 0.02f;
    private bool _bigger = false;
private void TimeAcceleration()
{
    if (!_isAttack || TimeManager.GetTimeScale() > 0.05f)
        return;
    if (_timeValue < _maxTime && !_bigger)
    {
        _timeValue += _timeValueChangeSpeed;
        TimeManager.SetTimeScale(_timeValue);
    }
    else if (_timeValue >= 0f)
    {
        _bigger = true;
        _timeValue -= _timeValueChangeSpeed;
        TimeManager.SetTimeScale(_timeValue);
    }
    else
    {
        _bigger = false;
        _isAttack = false;
        _timeValue = 0f;
    }       
}
*/

/*
public override void ChangeColor(Color color)
{
    _renderer.material.color = color;
}
public override void ReturnInitialColor()
{
    _renderer.material.color = _initialColor;
}
*/

/*
private void TimeAcceleration()
{        
    if (_isAttack)
    {
        _isAttack = TimeManager.TimeAcceleration(true);
    }
}
*/