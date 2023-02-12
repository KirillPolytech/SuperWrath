using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigController : MonoBehaviour
{
    [SerializeField] private GameObject __rigLayer_RightHand;
    [SerializeField] private RigBuilder __rigBuilder;
    private TwoBoneIKConstraint __twoBoneIKConstraint;
    private Animator __playerAnimator;

    private HeroController _hero;
    private HeroGunsController _gunsController;
    private void Awake()
    {
        _hero = FindObjectOfType<HeroController>();
        _gunsController = FindObjectOfType<HeroGunsController>();
        __playerAnimator = _hero.GetComponent<Animator>();
    }
    private void Start()
    {
        __twoBoneIKConstraint = __rigLayer_RightHand.GetComponent<TwoBoneIKConstraint>();
        __twoBoneIKConstraint.data.target = __twoBoneIKConstraint.transform;
        __rigBuilder.Build();
        __twoBoneIKConstraint.weight = 0f;
    }
    private void FixedUpdate()
    {
        if (_gunsController.GetProp || _gunsController.GetFireArmWeapon) // || _gunsController.GetMeleeWeapon.GetName() != "Fists")
            __twoBoneIKConstraint.weight = 1f;
        else
            __twoBoneIKConstraint.weight = 0f;
    }
    public void UpdatewoBoneIKConstraint(Transform __target)
    {
        //
        __rigBuilder.enabled = false;
        //

        //__twoBoneIKConstraint.data.target = __target;
        __rigBuilder.Build();
        __playerAnimator.Rebind();
        __twoBoneIKConstraint.weight = 1f;
        //
        __rigBuilder.enabled = true;
        //
    }
}
