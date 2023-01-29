using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigController : MonoBehaviour
{
    [SerializeField] private GameObject __rigLayer_RightHand;
    [SerializeField] private RigBuilder __rigBuilder;
    private TwoBoneIKConstraint __twoBoneIKConstraint;
    private Animator __playerAnimator;

    private HeroControllerWithAnimations _hero;
    private void Start()
    {
        _hero = FindObjectOfType<HeroControllerWithAnimations>();
        __playerAnimator = _hero.GetComponent<Animator>();

        __twoBoneIKConstraint = __rigLayer_RightHand.GetComponent<TwoBoneIKConstraint>();
        __twoBoneIKConstraint.data.target = __twoBoneIKConstraint.transform;
        __rigBuilder.Build();
        __twoBoneIKConstraint.weight = 0f;
    }
    private void FixedUpdate()
    {
        if (_hero.GetWeapon().GetName() == "Fists")
            __twoBoneIKConstraint.weight = 0f;
        else
            __twoBoneIKConstraint.weight = 1f;
    }
    public void UpdatewoBoneIKConstraint(Transform __target)
    {
        //
        __rigBuilder.enabled = false;
        //

        __twoBoneIKConstraint.data.target = __target;        
        __rigBuilder.Build();
        __playerAnimator.Rebind();
        __twoBoneIKConstraint.weight = 1f;
        //
        __rigBuilder.enabled = true;
        //
    }
}
