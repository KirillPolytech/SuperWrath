using UnityEngine;

public class HeroGunsController : MonoBehaviour
{
    [SerializeField]private GameObject _arm;

    private MeleeWeapon _meleeGun = null;
    private FireArmWeapon _fireArmGun = null;
    private Prop _activeProp = null;
    public MeleeWeapon GetMeleeWeapon { get { return _meleeGun; } }
    public FireArmWeapon GetFireArmWeapon { get { return _fireArmGun; }  }
    public Prop GetProp { get { return _activeProp; } }
    [Range(0, 20)]
    [SerializeField] private int __PropthrowingForce = 5;// 5
    [Range(0, 100)]
    [SerializeField] private int __WeaponthrowingForce = 100;

    [Range(0.3f,1f)]
    [SerializeField] private float __takingThrowingDelayLimit = 0.6f; // 0.45f
    private float __delayBetweenThrowsOrPickUps = 0f;
    private bool __isTakingOrThrowingItem = false;

    private RigController __rigController;
    private Fists _fists;

    private bool __isTimeAccelerated = false;
    private bool __isAttacking = false;
    public bool IsAttacking { get { return __isAttacking; }}

    private HeroCamera _heroCamera;
    private void Awake()
    {
        _heroCamera = FindObjectOfType<HeroCamera>();
        __rigController = FindObjectOfType<RigController>();
        _camera = FindObjectOfType<HeroCamera>();
        _fists = GetComponentInChildren<Fists>();
        _meleeGun = _fists;
    }
    private void Update()
    {
        if (ButtonsManager.IsLeftMousePressed())
            __isAttacking = true;

        // ��������� ������� ��� �����.
        if (__isAttacking)
            __isAttacking = TimeManager.TimeAcceleration(__isAttacking);

        // �������� ����� �������� ��������
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
        // ��������� ������� ��� �������� ��������.
        if ((__isTakingOrThrowingItem || __isTimeAccelerated) )//&& GetComponent<Rigidbody>().velocity.magnitude < 0.1f)
            __isTimeAccelerated = TimeManager.TimeAcceleration(__isTakingOrThrowingItem);


        if (ButtonsManager.IsRightMousePressed() && !__isTakingOrThrowingItem)
        {
            ThrowWeapon();
        }
        if (ButtonsManager.IsRightMousePressed() && !__isTakingOrThrowingItem)
        {
            PickUpGun();
        }            
    }
    public void ThrowWeapon()
    {        
        if (_fireArmGun)
        {
            for (int i = 0; i < _tempGun.transform.childCount; i++)
            {
                _tempGun.transform.GetChild(i).gameObject.layer = 7;
            }
            _tempGun.layer = 7;

            _fireArmGun.gameObject.layer = 7;
            _fireArmGun.UnParent();
            _fireArmGun.GetComponent<BoxCollider>().enabled = true;
            _fireArmGun.BeingThrown(__WeaponthrowingForce, transform.forward);
            _fireArmGun = null;
            _meleeGun = _fists;
            __rigController.UpdatewoBoneIKConstraint(__rigController.transform);
            __isTakingOrThrowingItem = true;
        }
        if (_meleeGun && _meleeGun != _fists)
        {
            if (_meleeGun.GetComponents<BoxCollider>()[1])
                _meleeGun.GetComponents<BoxCollider>()[1].enabled = true;
            _meleeGun.SetParent(null);
            _meleeGun.UnParent();
            _meleeGun.BeingThrown(__WeaponthrowingForce, transform.forward);
            _meleeGun = _fists;
            __rigController.UpdatewoBoneIKConstraint(__rigController.transform);
            __isTakingOrThrowingItem = true;
        }
        if (_activeProp)
        {
            _activeProp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            _activeProp.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _activeProp.IsPropDestroyed = true;
            Vector3 _throwDirection = (_heroCamera.GetHitPoint(500) - _activeProp.transform.position).normalized * __PropthrowingForce;

            _activeProp.BeingThrown(_throwDirection, Vector3.zero, __PropthrowingForce); //transform.forward
            _activeProp.gameObject.transform.parent = null;
            _activeProp = null;

            _meleeGun = _fists;
            __isTakingOrThrowingItem = true;
        }
    }

    private GameObject _tempGun;
    private HeroCamera _camera;
    private void PickUpGun()
    {
        _tempGun = _camera.GetHittedGameObject(3);
        if (ButtonsManager.IsRightMousePressed() && _tempGun && _tempGun.GetComponent<Prop>() && GetMeleeWeapon == _fists)
        {
            _tempGun.GetComponent<Prop>().PickUp();
            _tempGun.transform.SetParent(transform);
            SetProp(_tempGun.GetComponent<Prop>());
            _activeProp = _tempGun.GetComponent<Prop>();
            __isTakingOrThrowingItem = true;
        }

        if (ButtonsManager.IsRightMousePressed() && _tempGun && _tempGun.GetComponent<Gun>() && GetMeleeWeapon == _fists)
        {
            _tempGun.transform.SetParent(_arm.transform);

            if (_tempGun.GetComponent<Melee>())
            {
                if (_tempGun.gameObject.GetComponents<BoxCollider>()[1])
                    _tempGun.gameObject.GetComponents<BoxCollider>()[1].enabled = false;

                _meleeGun = _tempGun.GetComponent<Melee>();

                _meleeGun.SetParent(gameObject);
                __rigController.UpdatewoBoneIKConstraint(_meleeGun.transform.GetChild(0).transform);
            }
            else if (_tempGun.GetComponent<FireArmWeapon>())
            {
                for (int i = 0; i < _tempGun.transform.childCount; i++)
                {
                    _tempGun.transform.GetChild(i).gameObject.layer = 11;
                }
                _tempGun.layer = 11;

                _tempGun.GetComponent<BoxCollider>().enabled = false;
                _fireArmGun = _tempGun.GetComponent<FireArmWeapon>();
                _meleeGun = null;
                __rigController.UpdatewoBoneIKConstraint(_fireArmGun.transform.GetChild(0).transform);
            }
            __isTakingOrThrowingItem = true;
        }
    }
    public void SetProp(Prop _prop)
    {
        _activeProp = _prop;
    }
}

/*
 *     [Range(0.3f, 1f)]
    [SerializeField] private float __maxAnimationTime = 0.3f;
 *     private float _delay = 0f;
 *         private bool _isAttacking = false;
 *                 //_isAttacking = _animationsController.IsAttacking();
if ((ButtonsManager.IsLeftMousePressed() || _isAttacking) && _activeGun.GetName() == "Fists")
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
Prop prop = _activeProp;
_activeProp.gameObject.transform.parent = null;

prop.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
prop.GetComponent<Rigidbody>().velocity = Vector3.zero;
prop.GetComponent<Prop>().IsPropDestroyed = true;
Vector3 _throwDirection = Camera.main.GetComponent<HeroCamera>().GetHitPointOn500Meters() - _activeGun.transform.position;
prop.GetComponent<ObjectTimeScale>().InitialVelocityAndAngularVelocity(_throwDirection.normalized * 25, Vector3.zero);
//prop.gameObject.transform.parent = null;
_activeProp = null;
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
*/

/*
if (_delay > _delayBetweenShots && ButtonsManager.IsLeftMousePressed())
{
    _meleeGun.Attack();
    _delay = 0f;
    __isAttacking = true;
    //Debug.Log("Fire " + _activeGun);
}
*/

/*
if (!_fireArmGun)
    MeleeAttack();
if (_fireArmGun)
    FireArmAttack();

private void MeleeAttack()
{
    if (ButtonsManager.IsLeftMousePressed())
        __isAttacking = true;
}
private void FireArmAttack()
{
    if (ButtonsManager.IsLeftMousePressed())
        __isAttacking = true;
}
*/

/*
if (_activeProp)
    _activeProp.transform.localPosition = Vector3.Lerp(_propInitialPosition, _propPosition, 0.01f);//new Vector3(0.15f, 1.35f, 0.54f), 0.1f);
*/