using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
public class MeleeEnemy : EnemyTemplate
{
    [SerializeField] private int _distanceToAttack = 1;

    private GameObject _player;
    private float _delay = 0f;

    private Animator _animator;
    private EnemyCounter _enemyCounter;
    private AudioSource _stepSounds;

    private Rigidbody[] _rigidbodies;
    private Rigidbody _rb;
    private void Awake()
    {
        gameObject.tag = "Enemy";

        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        _enemyCounter = FindObjectOfType<EnemyCounter>();
        _player = GameObject.FindGameObjectWithTag("Player");

        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _stepSounds = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();

        _stepSounds.playOnAwake = false;
        _stepSounds.spatialBlend = 1;
        _stepSounds.volume = SoundsVolume.GetSoundsVolume();
    }
    private void FixedUpdate()
    {
        if (!_animator.enabled)
            return;

        if (_isDead)
            DeathAction(new Collider());

        _animator.speed = TimeManager.GetTimeScale();
        if ( (_player.transform.position - transform.position).magnitude < _distanceToAttack)
        {
            _agent.isStopped = true;
            _animator.SetBool("IsRunning", false);
            _animator.SetBool("IsIdle", true);
            if (_delay > _delayBetweenAttacks)
            {
                Attack();
                _delay = 0f;
            }
        }
        else
        {
            _agent.isStopped = false;
            _animator.SetBool("IsRunning", true);
            _animator.SetBool("IsIdle", false);
            Walk();
        }
        _delay += Time.fixedDeltaTime;

        FootStepSound();
    }
    protected override void Attack()
    {
        _gun.Attack();
    }
    public override void GetDamage(int value)
    {
        _hp -= value;
        _isDead = true;
    }
    protected override void Walk()
    {
        _agent.SetDestination(_player.transform.position);
        _agent.speed = _maxSpeed * TimeManager.GetTimeScale();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GetDamage(100);
            DeathAction(collision);
            Debug.Log("Death");
        }
        else if(collision.gameObject.CompareTag("Gun") && collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
        {
            DeathAction(collision);
        }
    }
    private void DeathAction(Collider collision)
    {
        GetComponent<BoxCollider>().enabled = false;
        _animator.enabled = false;
        _agent.enabled = false;
        _enemyCounter.IncreaseDefeatedEnemy();

        Vector3 _fallDirection = collision.gameObject.GetComponent<Rigidbody>().velocity.normalized;

        Rigidbody _rb = GetComponent<Rigidbody>();
        transform.gameObject.AddComponent<ObjectTimeScale>().InitialVelocityAndAngularVelocity(Physics.gravity, _rb.angularVelocity);

        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
  
            rigidbody.velocity = ( (_fallDirection * _fallVelocity ) + Physics.gravity ) * TimeManager.GetTimeScale();
            rigidbody.transform.gameObject.AddComponent<ObjectTimeScale>().InitialVelocityAndAngularVelocity(rigidbody.velocity, rigidbody.angularVelocity);
            rigidbody.transform.gameObject.AddComponent<RigidBodyConstraints>();
        }
    }
    private void FootStepSound()
    {
        if (!_agent.isStopped && !_stepSounds.isPlaying)
        {
            _stepSounds.Play();
        }
        ChangeSoundPitchScript.ChangePitch(_stepSounds);
    }
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