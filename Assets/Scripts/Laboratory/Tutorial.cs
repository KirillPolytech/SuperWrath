using UnityEngine;
using UnityEngine.AI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private MeleeEnemy _enemy;

    private HeroController _player;
    private void Start()
    {
        _player = FindObjectOfType<HeroController>();
        _player.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        _enemy.gameObject.GetComponent<NavMeshAgent>().enabled = false;
    }
    private void Update()
    {
        if (_enemy.GetDeadStatement)
        {
            _player.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
// не выключать игрока