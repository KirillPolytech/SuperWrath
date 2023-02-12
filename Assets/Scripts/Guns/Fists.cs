using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Fists : MeleeWeapon
{
    [SerializeField] private HeroGunsController _heroGunsController;
    private Rigidbody _rb;
    private bool __isAttacking = false;
    public bool IsAttacking { get { return __isAttacking; } }
    private void Awake()
    {
        gameObject.tag = "Gun";
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        GetComponent<BoxCollider>().isTrigger = true;
        _heroGunsController = _parentGameObject.GetComponent<HeroGunsController>();
    }
    private void Update()
    {
        __isAttacking = _heroGunsController.IsAttacking;
    }
    public override string GetName()
    {
        return GetType().ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && _heroGunsController.IsAttacking && _heroGunsController.GetMeleeWeapon.GetName() == "Fists")
        {
            other.gameObject.GetComponent<EnemyTemplate>().GetDamage(100);
        }
    }
    public override void BeingThrown(float throwingForce, Vector3 direction) { }
    public override void UnParent() { }
    public override void Attack()        {    }
    public override void SetParent(GameObject parent)    {    }
}
//private Animator _animator;
//&& _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "FistAttack")