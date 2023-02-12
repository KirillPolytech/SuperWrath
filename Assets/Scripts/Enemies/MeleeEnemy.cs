using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
public class MeleeEnemy : EnemyTemplate
{
    [SerializeField] private MeleeWeapon _weapon;
    [Range(0.5f, 2f)]
    [SerializeField] private float _distanceToAttack = 1.2f;

    private GameObject _player;
    private float _delay = 0f;

    private Animator _animator;
    private EnemyCounter _enemyCounter;
    private AudioSource _stepSounds;
    private HeroController _heroController;
    private HeroCamera _heroCamera;

    private Rigidbody[] _rigidbodies;
    public bool GetDeadStatement { get { return _isDead; } }
    private void Awake()
    {
        InitialEnemy();
    }
    public void InitialEnemy()
    {
        gameObject.tag = "Enemy";

        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }

        _enemyCounter = FindObjectOfType<EnemyCounter>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _heroController = _player.GetComponent<HeroController>();

        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _stepSounds = GetComponent<AudioSource>();
        _heroCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<HeroCamera>();

        _stepSounds.playOnAwake = false;
        _stepSounds.spatialBlend = 1;
        _stepSounds.volume = SoundsVolume.GetSoundsVolume();

        GetComponent<BoxCollider>().isTrigger = true;

        _agent.enabled = false;
        _agent.enabled = true;
    }
    private void Update()
    {
        _delay += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (!_animator.enabled || !_agent.enabled)
        {
            return;
        }

        Walking();
    }
    private void Walking()
    {
        _animator.speed = TimeManager.GetTimeScale();
        if ((_player.transform.position - transform.position).magnitude < _distanceToAttack)
        {
            _agent.isStopped = true;
            if (_delay > _delayBetweenAttacks && !_heroController.IsPlayerDead)
            {
                Attack();
                _animator.SetBool("IsAttacking", true);
                _animator.SetBool("IsRunning", false);
                _animator.SetBool("IsIdle", false);
                _delay = 0f;
            }
            else if (_heroController.IsPlayerDead)
            {
                _animator.SetBool("IsIdle", true);
                _animator.SetBool("IsAttacking", false);
                _animator.SetBool("IsRunning", false);
            }
        }
        else
        {
            _agent.isStopped = false;
            _animator.SetBool("IsRunning", true);
            _animator.SetBool("IsIdle", false);
            _animator.SetBool("IsAttacking", false);
            Walk();
        }

        if (_agent.speed > 0f)
        {
            FootStepSound();
        }
    }
    public override void GetDamage(int value)
    {
        if (_hp > 0)
        {
            _hp -= value;
            _isDead = true;
            DeathAction();
        }
    }
    protected override void Walk()
    {
        _agent.SetDestination(_player.transform.position);
        _agent.speed = _maxSpeed * TimeManager.GetTimeScale();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if ((
            collision.gameObject.CompareTag("Bullet") ||
            collision.gameObject.CompareTag("Gun") ||
            collision.gameObject.CompareTag("Prop")) && collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 1f 
            )
        {
            GetDamage(100);
            _enemyCounter.IncreaseDefeatedEnemy();
        }
        if (collision.gameObject.CompareTag("Gun") && collision.gameObject.GetComponent<Gun>().GetName() == "Fists")
        {
            if (collision.gameObject.GetComponent<Fists>().IsAttacking)
            {
                GetDamage(100);
                _enemyCounter.IncreaseDefeatedEnemy();
            }
        }
    }
    private void DeathAction()
    {
        GetComponent<BoxCollider>().enabled = false;
        _animator.enabled = false;
        _agent.enabled = false;

        Vector3 _fallDirection = (_heroCamera.GetHitPoint(500) - transform.position).normalized;//-transform.forward;
        _fallDirection = new Vector3(_fallDirection.x, 0f, _fallDirection.z);
        Vector3 _dir = (_fallDirection * _fallVelocity) + Physics.gravity * 0.1f * Time.fixedDeltaTime;
        foreach (var rigidbody in _rigidbodies)
        {
            //
            if (rigidbody.gameObject.GetComponent<CharacterJoint>())
                rigidbody.gameObject.GetComponent<CharacterJoint>().breakForce = 0.1f;
            //
            rigidbody.isKinematic = false;
            rigidbody.transform.gameObject.AddComponent<ObjectTimeScale>().InitialVelocityAndAngularVelocity(_dir, rigidbody.angularVelocity);
            rigidbody.transform.gameObject.GetComponent<ObjectTimeScale>().TurnObjectConstaints(true);
        }
        _weapon.UnParent();
    }
    private void FootStepSound()
    {
        if (!_agent.isStopped && !_stepSounds.isPlaying)
        {
            _stepSounds.Play();
        }
        ChangeSoundPitchScript.ChangePitch(_stepSounds);
    }

    protected override void Attack()
    {    }
}
//[SerializeField] private Gun _gun;
//[SerializeField] private float _delayBetweenAttacks = 0.5f;
//[SerializeField] private float _maxSpeed = 1f;

/*
Rigidbody _rb = GetComponent<Rigidbody>();
_rb.isKinematic = false;
_rb.useGravity = false;
_rb.velocity = Vector3.zero;
_rb.angularVelocity = Vector3.zero;
_rb.constraints = RigidbodyConstraints.FreezeAll;
_rb.transform.gameObject.AddComponent<ObjectTimeScale>().InitialVelocityAndAngularVelocity((_fallDirection + Physics.gravity) * _fallVelocity, _rb.angularVelocity);
_rb.transform.gameObject.AddComponent<RigidBodyConstraints>();
*/

// _rb = GetComponent<Rigidbody>();
//_rb.constraints = RigidbodyConstraints.None;
/*
gameObject.tag = "Enemy";

_rigidbodies = GetComponentsInChildren<Rigidbody>();
foreach (var rigidbody in _rigidbodies)
{
    rigidbody.velocity = Vector3.zero;
    rigidbody.angularVelocity = Vector3.zero;
    rigidbody.isKinematic = true;
}

_enemyCounter = FindObjectOfType<EnemyCounter>();
_player = GameObject.FindGameObjectWithTag("Player");
_heroController = _player.GetComponent<HeroController>();
_heroCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<HeroCamera>();

_agent = GetComponent<NavMeshAgent>();
_animator = GetComponent<Animator>();
_stepSounds = GetComponent<AudioSource>();

_stepSounds.playOnAwake = false;
_stepSounds.spatialBlend = 1;
_stepSounds.volume = SoundsVolume.GetSoundsVolume();
*/