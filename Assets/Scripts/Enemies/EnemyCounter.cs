using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    [SerializeField] private int _enemyToDefeat = 25;   
    public int EnemyToDefeat { get { return _enemyToDefeat; } }
    private GameObject[] _enemies;
    private int _enemiesCounter = 0;
    private int _defeatedEnemies = 0;
    private void Start()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        _enemiesCounter = _enemies.Length;
    }
    public void IncreaseDefeatedEnemy()
    {
        _defeatedEnemies++;
        //Debug.Log("_defeatedEnemies " + _defeatedEnemies + "   _enemiesCounter " + _enemiesCounter);
    }
    public int GetNumberOfDefeatedEnemies()  { return _defeatedEnemies;}
    public int GetAmountEnemies() { return _enemiesCounter; }
    public void IncreaseEnemyCount() { _enemiesCounter++; }
}
