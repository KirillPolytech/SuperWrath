using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(HeroGunsController))]
[RequireComponent(typeof(HeroAnimationsController))]
[RequireComponent(typeof(BoxCollider))]
public class HeroController : MonoBehaviour
{
    [Range(0, 15)]
    [SerializeField] private int _speed = 3;

    private int __hp = 1;
    public int GetCurrentHp { get { return __hp; } }

    private bool _isPlayerDead = false, __isPlayerWon = false;
    public bool IsPlayerWon { get { return __isPlayerWon; } }
    public bool IsPlayerDead { get { return _isPlayerDead; } }

    private Rigidbody _rigidbody;
    private float _axisX, _axisY;
    private Vector3 _movementDirection;

    private EnemyCounter _enemyCounter;
    private AudioSource _stepSounds;
    private Animator _animator;
    private Rigidbody[] rigidbodies;
    private void Awake()
    {
        gameObject.tag = "Player";
        gameObject.layer = 2; // ingore raycast

        _enemyCounter = FindObjectOfType<EnemyCounter>();
        _rigidbody = GetComponent<Rigidbody>();
        _stepSounds = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();

        _stepSounds.volume = SoundsVolume.GetSoundsVolume();

        rigidbodies = GetComponentsInChildren<Rigidbody>();

        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        _rigidbody.useGravity = false;
        _rigidbody.mass = 100;
    }
    private void Update()
    {
        ChangeTimeOnMove();
        IsWon();
        if (!_isPlayerDead && __hp <= 0)
            IsDead();
    }
    private void FixedUpdate()
    {
        if (IsPlayerDead)
        {
            return;
        }

        Movement();
        FootStepSound();
    }
    private void Movement()
    {
        _axisX = ButtonsManager.HorizontalInput() * _speed;
        _axisY = ButtonsManager.VerticalInput() * _speed;
        _movementDirection = transform.forward * _axisY + transform.right * _axisX;
        _rigidbody.velocity = _movementDirection;
    }
    public void GetDamage(int damage)
    {
        __hp -= damage;
    }
    private void IsDead()
    {
        _isPlayerDead = true;
        DeathAction();
        Debug.Log("DEAD");
    }
    private void DeathAction()
    {
        GetComponent<BoxCollider>().enabled = false;
        _animator.enabled = false;

        Camera.main.gameObject.AddComponent<BoxCollider>();
        Camera.main.gameObject.AddComponent<Rigidbody>();
        Camera.main.gameObject.transform.SetParent(null);

        foreach (var rigidbody in rigidbodies)
        {
            if (rigidbody.gameObject.GetComponent<CharacterJoint>())
                rigidbody.gameObject.GetComponent<CharacterJoint>().breakForce = 0.1f;
            rigidbody.isKinematic = false;
            rigidbody.transform.gameObject.AddComponent<ObjectTimeScale>().InitialVelocityAndAngularVelocity(-Vector3.forward, rigidbody.angularVelocity);
            rigidbody.transform.gameObject.GetComponent<ObjectTimeScale>().TurnObjectConstaints(true);
        }
    }
    private void IsWon()
    {
        if (_enemyCounter.GetNumberOfDefeatedEnemies() >= _enemyCounter.EnemyToDefeat)
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
        if (_rigidbody.velocity.magnitude < 0.05f) //&& _rigidbody.velocity.magnitude > 0f)
        {
            TimeManager.DeacreaseTimeScale(-0.05f);
            _rigidbody.velocity = Vector3.zero;
        }
        else if (_rigidbody.velocity.magnitude < 0.9f && _rigidbody.velocity.magnitude > 0.05f)
            TimeManager.SetTimeScale(Mathf.Clamp(0 + _rigidbody.velocity.magnitude, 0, 1));
        else if (_rigidbody.velocity.magnitude >= 0.9f)
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

/*
    private HeroCamera _camera;
    private GameObject _tempGun;
    private RigController __rigController;

        /*
        if (__isTakingOrThrowingItem)
        {
            if (__delayBetweenThrowsOrPickUps <= __takingThrowingDelayLimit)
            {
                __delayBetweenThrowsOrPickUps += Time.deltaTime;
            }
            else
            {
                __delayBetweenThrowsOrPickUps = 0f;
                __isTakingOrThrowingItem = false;
            }
        }
        */

/*
 *     //[Range(0f, 1f)]
    //[SerializeField] private float _delayBetweenShots = 0.25f;
 * 
[SerializeField] private float __takingThrowingDelayLimit = 0.6f;
private float __delayBetweenThrowsOrPickUps = 0f;
private bool __isTakingOrThrowingItem = false;
*/

/*
if (ButtonsManager.IsRightMousePressed() && !__isTakingOrThrowingItem)
{
    ThrowWeapon();
}
if (ButtonsManager.IsRightMousePressed() && !__isTakingOrThrowingItem)
{
    PickUpGun();
}
*/

/*
 *     [Range(0f, 1f)]
    [SerializeField] private float __maxAnimationTime = 0.3f;
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
*/

/*
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
   */
/*
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
*/

/*
     private bool _isAttacking = false;
     private float __animationDelay = 0f;
     [SerializeField] private float __maxAnimationDelay = 5f;
     private void AnimationsControl()
     {
         if ( (Input.GetButtonDown("Fire1") || _isAttacking) && __animationDelay < __maxAnimationDelay && _gunsController.GetWeapon().GetName() == "Fists")
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
     public bool IsAttacking()
     {
         return _isAttacking;
     }
       */