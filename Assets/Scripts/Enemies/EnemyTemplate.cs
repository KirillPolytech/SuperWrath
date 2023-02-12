using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyTemplate : MonoBehaviour
{
    [SerializeField] protected float _delayBetweenAttacks = 0.5f;
    [SerializeField] protected float _maxSpeed = 1f;
    [SerializeField] protected float _fallVelocity = 10f;
    protected int _hp = 1;
    protected NavMeshAgent _agent;
    protected bool _isDead = false;
    protected abstract void Attack();
    protected abstract void Walk();
    public abstract void GetDamage(int value);
}
