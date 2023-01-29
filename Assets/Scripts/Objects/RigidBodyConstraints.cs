using UnityEngine;

public class RigidBodyConstraints : MonoBehaviour
{
    private Rigidbody _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (TimeManager.GetTimeScale() == 0f)
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            _rb.constraints = RigidbodyConstraints.None;
        }
    }
}
