using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class HeroControllerWithAnimations : MonoBehaviour
{
    [Range(0, 15)]
    [SerializeField] private int _speed = 3;
    [Range(0f, 1f)]
    [SerializeField] private float _delayBetweenShots = 0.25f;
    [Range(1, 15)]
    [SerializeField] private int __throwingForce = 5;
    [Range(0f, 1f)]
    [SerializeField] private float __maxAnimationTime = 0.3f;

    private int __hp = 1;
    public int GetCurrentHp { get { return __hp; } }

    private Rigidbody _rigidbody;
    private float _axisX, _axisY;
    private Vector3 _movementDirection;

    private bool _isPlayerDead = false, __isPlayerWon = false;
    public bool IsPlayerWon { get { return __isPlayerWon; } }
    public bool IsPlayerDead { get { return _isPlayerDead; } }

    private Gun _activeGun = null;

    private float _delay = 0f;

    private Animator _animator;
    private HeroCamera _camera;
    private GameObject _tempGun;
    private EnemyCounter _enemyCounter;
    private RigController __rigController;

    private AudioSource _stepSounds;
    private void Awake()
    {
        gameObject.tag = "Player";
        gameObject.layer = 2; // ingore raycast

        _enemyCounter = FindObjectOfType<EnemyCounter>();
        _camera = FindObjectOfType<HeroCamera>();
        _rigidbody = GetComponent<Rigidbody>();
        _activeGun = GetComponent<Fists>();
        _animator = GetComponent<Animator>();
        __rigController = FindObjectOfType<RigController>();
        _stepSounds = GetComponent<AudioSource>();
    }
    private void Update()
    {
        ChangeTimeOnMove();
        IsWon();
        IsDead();
    }
    private float __delayBetweenThrowsOrPickUps = 0f, __delayLimit = 0.2f;
    private bool __isTakingOrThrowingItem = false;
    private void FixedUpdate()
    {
        if (__isTakingOrThrowingItem)
        {
            if (__delayBetweenThrowsOrPickUps <= __delayLimit)
            {
                __delayBetweenThrowsOrPickUps += Time.fixedDeltaTime;
            }
            else
            {
                __delayBetweenThrowsOrPickUps = 0f;
                __isTakingOrThrowingItem = false;
            }
        }
        if (ButtonsManager.IsRightMousePressed() && !__isTakingOrThrowingItem)
        {
            ThrowWeapon();
        }
        if (ButtonsManager.IsRightMousePressed() && !__isTakingOrThrowingItem)
        {
            PickUpGun();
        }

        Movement();
        AnimationsControl();
        FootStepSound();
        Debug.DrawRay(transform.position, transform.forward, Color.blue);


        if ( (ButtonsManager.IsLeftMousePressed() || _isAttacking ) && _activeGun.GetName() == "Fists") 
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > __maxAnimationTime && _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "FistAttack")
            {
                _activeGun.Attack();
                Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
        }
        else if (_delay > _delayBetweenShots && ButtonsManager.IsLeftMousePressed())
        {
            _activeGun.Attack();
            _delay = 0f;
        }
        _delay = Mathf.Clamp(_delay + Time.fixedDeltaTime, 0f, _delayBetweenShots + 1f);
    }
    private void Movement()
    {
        _axisX = ButtonsManager.HorizontalInput() * _speed;
        _axisY = ButtonsManager.VerticalInput() * _speed;
        _movementDirection = transform.forward * _axisY + transform.right * _axisX;
        _rigidbody.velocity = _movementDirection;
    }
    private void PickUpGun()
    {
        _tempGun = _camera.GetHittedGameObjectOn3Meters();

        if (ButtonsManager.IsRightMousePressed() && _tempGun && _tempGun.GetComponent<Gun>() && _activeGun == GetComponent<Fists>())
        {
            _tempGun.transform.SetParent(transform);
            SetWeapon(_tempGun.GetComponent<Gun>());
            
            __rigController.UpdatewoBoneIKConstraint(_activeGun.transform.GetChild(2).transform);
            __isTakingOrThrowingItem = true;
        }
    }
    private bool _isAttacking = false;
    private float __animationDelay = 0f;
    [SerializeField] private float __maxAnimationDelay = 5f;
    private void AnimationsControl()
    {
        if ( (Input.GetButtonDown("Fire1") || _isAttacking) && __animationDelay < __maxAnimationDelay && _activeGun.GetName() == "Fists")
        {
            _animator.SetBool("IsMeleeAttack", true);
            _animator.SetBool("IsIdle", false);
            _animator.SetBool("IsRunning", false);

            __animationDelay += Time.fixedDeltaTime;
            _isAttacking = true;
            //Debug.Log("Delay");
        }
        else if (_rigidbody.velocity.magnitude > 0.1f )
        {
            _animator.SetBool("IsRunning", true);
            _animator.SetBool("IsMeleeAttack", false);
            _animator.SetBool("IsIdle", false);

            _isAttacking = false;
            __animationDelay = 0f;
        }
        else
        {
            _animator.SetBool("IsIdle", true);
            _animator.SetBool("IsRunning", false);
            _animator.SetBool("IsMeleeAttack", false);

            _isAttacking = false;
            __animationDelay = 0f;
        }
    }
    public void SetWeapon(Gun _gun)
    {
        _activeGun = _gun;
    }
    public Gun GetWeapon()
    {
        return _activeGun;
    }
    public void ThrowWeapon()
    {
        if (_activeGun.GetName() == "Fists")
        {
            return;
        }
        _activeGun.UnParent();
        _activeGun.BeingThrown(__throwingForce, transform.forward);
        _activeGun = GetComponent<Fists>();

        __rigController.UpdatewoBoneIKConstraint(__rigController.transform);

        __isTakingOrThrowingItem = true;
    }
    public void GetDamage(int damage)
    {
        __hp -= damage;
    }
    private void IsDead()
    {
        if (__hp <= 0)
        {
            _isPlayerDead = true;
            Debug.Log("DEAD");
        }
    }
    private void IsWon()
    {
        if (_enemyCounter.GetAmountEnemies() <= _enemyCounter.GetDefeatedEnemies() && _enemyCounter.GetAmountEnemies() != 0)
        {
            __isPlayerWon = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            GetDamage(100);
        }
    }

    private void ChangeTimeOnMove()
    {
        if (_rigidbody.velocity.magnitude < 0.1f)
            TimeManager.SetTimeScale(0f);
        else if (_rigidbody.velocity.magnitude < 0.9f)
            TimeManager.SetTimeScale(Mathf.Clamp(0 + _rigidbody.velocity.magnitude, 0, 1));
        else
            TimeManager.SetTimeScale(1f);
    }
    private void FootStepSound()
    {
        if (_rigidbody.velocity.magnitude > 0.2f && !_stepSounds.isPlaying)
        {
            _stepSounds.Play();
        }
        ChangeSoundPitchScript.ChangePitch(_stepSounds);
    }
}
/*
[SerializeField] private static float _timeChangeRate = 0.2f; // Delete
[Range(1, 100)] 
*/
/*
if (Input.GetButtonDown("E"))
{
    TimeManager.SetTimeScale(Mathf.Clamp(TimeManager.GetTimeScale() + _timeChangeRate, 0, 1));
}
else if (Input.GetButtonDown("Q"))
{
    TimeManager.SetTimeScale(Mathf.Clamp(TimeManager.GetTimeScale() - _timeChangeRate, 0, 1));
}
*/

/*changecolor
if (__tempTempGun == null)
{
    __tempTempGun = _tempGun;
}
else if (__tempTempGun != _tempGun)
{
    if (__tempTempGun.gameObject.GetComponent<Renderer>())
        __tempTempGun.gameObject.GetComponent<Renderer>().material.color = Color.white;
    __tempTempGun = null;
}
if (_tempGun && _tempGun.CompareTag("Gun"))
    _tempGun.gameObject.GetComponent<Renderer>().material.color = new Color(0,0, 0.5f, 0.5f);

if (_tempGun && _tempGun.CompareTag("Gun"))
{
    if (_tempGun.GetComponent<Pistol>()?.GetAmmo > 0 && !_activeGun)
    {
        _tempGun.GetComponent<BoxCollider>().enabled = false;
        _tempGun.GetComponent<Pistol>().enabled = true;
        _tempGun.GetComponent<Prop>().enabled = false;
        _tempGun.transform.SetParent(transform);
        _activeGun = _tempGun.GetComponent<Pistol>();
    }
}
*/
/*
public float GetAttackAnimationState()
{
    if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "FistAttack")
    {
        return _animator.playbackTime;
    }
    return -999;
}
*/

//if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > __test && _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "FistAttack")

/*
if (_tempGun && _tempGun.GetComponent<Gun>() && !__tempTempGun)
{
    //_tempGun.GetComponent<Pistol>().ChangeColor(new Color(0.4f, 0.4f, 1f, 1f));
    __tempTempGun = _tempGun;
}
else if(__tempTempGun && _tempGun != __tempTempGun)
{
    //__tempTempGun.GetComponent<Pistol>().ReturnInitialColor();
    __tempTempGun = null;
}
*/