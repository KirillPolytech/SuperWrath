using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ObjectTimeScale))]
public class Pistol : Gun
{
    [SerializeField] private AudioSource _attackSound;
    [SerializeField] private AudioSource _emptyShotSound;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private ParticleSystem _fireParticleSystem;
    [Range(1, 50)]
    [SerializeField] private int _bulletVelocity = 25;

    private ObjectsManager _objectsManager;
    private GameObject _tempBullet;
    private Rigidbody _rigidBody;
    private HeroCamera _camera;
    private ObjectTimeScale _objectTimeScale;
    private void Awake()
    {
        gameObject.tag = "Gun";

        _objectTimeScale = GetComponent<ObjectTimeScale>();
        _camera = FindObjectOfType<HeroCamera>();
        _objectsManager = FindObjectOfType<ObjectsManager>();
        _rigidBody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();

        _attackSound.playOnAwake = false;
        _emptyShotSound.playOnAwake = false;
        _initialColor = Color.white;
        _rigidBody.useGravity = false;
        _attackSound.pitch = 1.5f;
        _attackSound.volume = SoundsVolume.GetSoundsVolume();
        _emptyShotSound.volume = SoundsVolume.GetSoundsVolume();

        var _fireParticleSystemMain = _fireParticleSystem.main;
        _fireParticleSystemMain.loop = false;
        _fireParticleSystemMain.playOnAwake = false;
    }
    private void FixedUpdate()
    {
        if (transform.parent == null)
            return;

        transform.localPosition = _gunPosition;
        transform.rotation = Quaternion.LookRotation(_camera.GetHitPointOn500Meters() - transform.position);

        TimeAcceleration();
        ChangingSoundSpeed();
    }
    public override void Attack()
    {
        if (_ammo == 0)
        {
            _emptyShotSound.Play();
            return;
        }

        _tempBullet = Instantiate(_bullet);
        _tempBullet.gameObject.tag = "Bullet";
        _tempBullet.transform.SetPositionAndRotation(transform.position + transform.forward, Quaternion.LookRotation(_camera.GetHitPointOn500Meters() - transform.position));
        _tempBullet.GetComponent<Bullet>().InitialBullet((_camera.GetHitPointOn500Meters() - transform.position).normalized * _bulletVelocity);
        _objectsManager.AddBulletToList(_tempBullet);

        DecreaseAmmo();
        ShotSound();
        FireParticles();

        _isAttack = true;
    }

    private float _maxTime = 1f;
    private float _timeValue = 0f, _timeValueChangeSpeed = 0.04f;
    private bool _bigger = false;
    private bool _isAttack = false;
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
        }       
    }
    public override void BeingThrown(float __throwingForce, Vector3 direction)
    {
        _objectTimeScale.InitialVelocityAndAngularVelocity(direction * __throwingForce + Physics.gravity * 0.1f, _rigidBody.angularVelocity);
        _objectsManager.AddObjectToList( GetComponent<ObjectTimeScale>());
    }
    private void ShotSound()
    {
        _attackSound.pitch = Mathf.Clamp(TimeManager.GetTimeScale(), 0.1f, 1f);
        _attackSound.Play();
    }
    private void DecreaseAmmo()
    {
        _ammo--;
    }
    public override void UnParent()
    {
        transform.parent = null;
        _rigidBody.isKinematic = false;
        _rigidBody.constraints = RigidbodyConstraints.None;
        gameObject.AddComponent<BoxCollider>();
    }
    public override void ChangeColor(Color color)
    {
        _renderer.material.color = color;
    }
    public override void ReturnInitialColor()
    {
        _renderer.material.color = _initialColor;
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
    private void FireParticles()
    {
        _fireParticleSystem.Play();
    }

    public override string GetName()
    {
        return GetType().ToString();
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