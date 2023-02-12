using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(RigBuilder))]
public class HeroAnimationsController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private HeroGunsController _gunsController;

    [SerializeField] private float __maxAnimationDelay = 0.5f;//0.5f, 1f, 5f
    private bool _isAttacking = false;
    private float __animationDelay = 0f;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _gunsController = GetComponent<HeroGunsController>();
    }
    private void Update()
    {
        if (ButtonsManager.IsLeftMousePressed())
            _isAttacking = true;
        //_isAttacking = _gunsController.IsAttacking;
        AnimationsControl();
    }
    private void AnimationsControl()
    {
        if (_isAttacking 
            && __animationDelay >= __maxAnimationDelay 
            && _gunsController.GetMeleeWeapon && !_gunsController.GetFireArmWeapon && !_gunsController.GetProp)
        {
            _animator.SetBool("IsMeleeAttack", true);
            _animator.SetBool("IsIdle", false);
            _animator.SetBool("IsRunning", false);

            __animationDelay = 0f;
            _isAttacking = false;
            //Debug.Log("__animationDelay " + __animationDelay);
        }
        else if (_rigidbody.velocity.magnitude > 0.1f)
        {
            _animator.SetBool("IsRunning", true);
            _animator.SetBool("IsMeleeAttack", false);
            _animator.SetBool("IsIdle", false);
            //Debug.Log("__animationDelay " + __animationDelay + "  _isAttacking   " + _isAttacking + "  _gunsController.GetMeleeWeapon  "  + _gunsController.GetMeleeWeapon);
        }
        else
        {
            _animator.SetBool("IsIdle", true);
            _animator.SetBool("IsRunning", false);
            _animator.SetBool("IsMeleeAttack", false);
        }
        __animationDelay = Mathf.Clamp(__animationDelay + Time.deltaTime, 0f, __maxAnimationDelay + 1f);
    }
}
/*
if ((Input.GetButtonDown("Fire1") || _isAttacking) 
    && __animationDelay < __maxAnimationDelay 
    && _gunsController.GetMeleeWeapon && !_gunsController.GetFireArmWeapon && !_gunsController.GetProp)
{
    _animator.SetBool("IsMeleeAttack", true);
    _animator.SetBool("IsIdle", false);
    _animator.SetBool("IsRunning", false);

    __animationDelay += Time.fixedDeltaTime;
    _isAttacking = true;
    Debug.Log("__animationDelay " + __animationDelay);
}
else if (_rigidbody.velocity.magnitude > 0.1f)
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
*/