using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private Transform _enemyContainer;

    [SerializeField] private GameObject[] _powerups;

    private bool _stopSpawning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 randomPos = new Vector3(Random.Range(-9, 9), 7.28f, 0);
            GameObject enemy = Instantiate(_enemyPrefab, randomPos, Quaternion.identity);
            enemy.transform.parent = _enemyContainer;
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (_stopSpawning == false)
        {
            int randomSec = Random.Range(3, 7);
            Vector3 spawnPos = new Vector3(Random.Range(-8f, 8f), 7.28f, 0);

            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powerups[randomPowerUp], spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(randomSec);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
