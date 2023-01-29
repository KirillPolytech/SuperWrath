using UnityEngine;

public class Melee : Gun
{
    [Range(0,100)]
    [SerializeField] private int _damage = 100;

    private GameObject _player;
    private HeroControllerWithAnimations _hero;
    private Rigidbody _rigidBody;
    private ObjectsManager _objectsManager;
    private ObjectTimeScale _objectTimeScale;
    private void Awake()
    {
        _objectsManager = _objectsManager = FindObjectOfType<ObjectsManager>();
        _rigidBody = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _hero = _player.GetComponent<HeroControllerWithAnimations>();
        _objectTimeScale = GetComponent<ObjectTimeScale>();
    }
    public override void Attack()
    {
        _hero.GetDamage(_damage);
    }
    public override void BeingThrown(float __throwingForce, Vector3 direction)
    {
        _rigidBody.velocity = direction * __throwingForce;
        _objectTimeScale.InitialVelocityAndAngularVelocity(_rigidBody.velocity, _rigidBody.angularVelocity);
        _objectsManager.AddObjectToList(_objectTimeScale);
    }

    public override void ChangeColor(Color color)
    {
        _renderer.material.color = color;
    }
    public override void ReturnInitialColor()
    {
        _renderer.material.color = _initialColor;
    }

    public override string GetName()
    {
        return GetType().ToString();
    }

    public override void UnParent()
    {

    }
}
