using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Range(1,15)]
    [SerializeField] private float _delayBetweenSpawn = 5f;
    [SerializeField] private GameObject[] _spawnPosition;

    private float _delay = 0f;
    private EnemyCounter _enemyCounter;
    [SerializeField]GameObject _enemyPrefab;
    private void Awake()
    {
        _enemyCounter = FindObjectOfType<EnemyCounter>();
    }
    private void FixedUpdate()
    {
        if (_delay >= _delayBetweenSpawn && _enemyCounter.GetAmountEnemies() < _enemyCounter.EnemyToDefeat)
        {
            _delay = 0f;
            GameObject _tempEnemy = Instantiate(_enemyPrefab);
            _tempEnemy.transform.position = _spawnPosition[Random.Range(0, 2)].transform.position;
            _tempEnemy.GetComponent<MeleeEnemy>().InitialEnemy();

            _enemyCounter.IncreaseEnemyCount();
        }
        else
        {
            _delay += Time.fixedDeltaTime * TimeManager.GetTimeScale();
        }
    }
}
