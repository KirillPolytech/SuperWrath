using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(ObjectTimeScale))]
[RequireComponent(typeof(Rigidbody))]
public class Melee : MeleeWeapon
{
    private HeroController _hero;
    private Rigidbody _rigidBody;
    private ObjectsManager _objectsManager;
    private ObjectTimeScale _objectTimeScale;

    [Range(0, 1f)]
    [SerializeField] private float __liftDelayTime = 0.2f;
    private void Awake()
    {
        gameObject.tag = "Gun";

        _objectsManager = FindObjectOfType<ObjectsManager>();
        _rigidBody = GetComponent<Rigidbody>();
        _hero = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroController>();
        _objectTimeScale = GetComponent<ObjectTimeScale>();
        _renderer = GetComponent<Renderer>();

        GetComponent<BoxCollider>().isTrigger = true;
    }
    private bool _isparent = false;    
    private void FixedUpdate()
    {
        if (!_parentGameObject)
            return;
        if (transform.parent && !_isparent)
        {
            transform.localRotation = Quaternion.Euler(-130f, 35.7f, 94f);
            _isparent = true;
        }

        if (_parentGameObject.CompareTag("Enemy"))
        {
            transform.localPosition = _gunPosition;//new Vector3(-0.271f, 0.385f, 0.254f);
        }
        else 
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _gunPosition, __liftDelayTime);
        }
    }
    [Range(0.2f, 1f)]
    [SerializeField] private float _delayBetweenShots = 1f;
    private float _delay = 0f;
    private void Update()
    {
        if (!_isparent)
        {
            return;
        }

        if (_isparent)
            Attack();
    }
    public override void Attack() 
    {
        if (_delay > _delayBetweenShots && ButtonsManager.IsLeftMousePressed())
        {
            _delay = 0f;
        }
        _delay += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (!_parentGameObject)
            return;
        GameObject _temp = collision.gameObject;
        if (_temp.CompareTag("Player") && _parentGameObject.gameObject.CompareTag("Enemy"))
        {
            _hero.GetDamage(100);
        }else if(_temp.CompareTag("Enemy") && _parentGameObject.gameObject.CompareTag("Player"))
        {
            _temp.GetComponent<EnemyTemplate>().GetDamage(100);
        }
    }
    public override void BeingThrown(float __throwingForce, Vector3 direction)
    {
        transform.position += transform.forward;
        _rigidBody.velocity = direction * __throwingForce;
        _objectTimeScale.InitialVelocityAndAngularVelocity(_rigidBody.velocity, _rigidBody.angularVelocity);
        _objectsManager.AddObjectToList(_objectTimeScale);
    }
    public override string GetName()
    {
        return GetType().ToString();
    }
    public override void SetParent(GameObject parent)
    {
        _rigidBody.isKinematic = true;
        _parentGameObject = parent;
    }
    public override void UnParent()   
    {
        _isparent = false;
        _parentGameObject = null;
        transform.parent = null;
        _rigidBody.isKinematic = false;
        _rigidBody.constraints = RigidbodyConstraints.None;
        GetComponents<BoxCollider>()[1].enabled = true;
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


/*
if (_temp.CompareTag("Player") && !_parentMeleeEnemy.GetDeadStatement && _parentGameObject.gameObject.CompareTag("Enemy"))//&& _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "FistAttack" 
{
    _hero.GetDamage(_damage);
}
if (_temp && _parentGameObject != _temp && _temp.CompareTag("Enemy") && !_parentGameObject.gameObject.CompareTag("Enemy"))
{
    _temp.GetComponent<EnemyTemplate>().GetDamage(100);
}
*/
//Debug.Log(collision.gameObject);