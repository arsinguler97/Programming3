using UnityEngine;
using System.Collections;

public class WaveManager : Singleton<WaveManager>
{
    [Header("Wave Settings")]
    [SerializeField] private int waveCount = 3;
    [SerializeField] private int totalEnemiesToSpawn = 10;
    [SerializeField] private float spawnInterval = 5f;

    [Header("Wave Scaling")]
    [SerializeField] private int enemiesIncrementPerWave = 10;
    [SerializeField] private float spawnIntervalDecrease = 1f;
    [SerializeField] private float minSpawnInterval = 1f;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    private int _currentWave;
    private int _spawnedEnemies;
    private int _aliveEnemies;
    private int _initialTotalEnemiesToSpawn;
    private float _initialSpawnInterval;

    private void Start()
    {
        _initialTotalEnemiesToSpawn = totalEnemiesToSpawn;
        _initialSpawnInterval = spawnInterval;
    }

    public void StartWaves()
    {
        Debug.Log("WaveManager started");
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        for (_currentWave = 1; _currentWave <= waveCount; _currentWave++)
        {
            _spawnedEnemies = 0;
            _aliveEnemies = 0;

            WaveUI ui = FindFirstObjectByType<WaveUI>();
            if (ui != null)
                ui.UpdateWave(_currentWave, totalEnemiesToSpawn);

            while (_spawnedEnemies < totalEnemiesToSpawn)
            {
                SpawnEnemy();
                _spawnedEnemies++;
                _aliveEnemies++;

                if (ui != null)
                    ui.UpdateProgress(totalEnemiesToSpawn - _spawnedEnemies);

                yield return new WaitForSeconds(spawnInterval);
            }

            yield return new WaitUntil(() => _aliveEnemies <= 0);

            Debug.Log($"Wave {_currentWave} finished!");

            totalEnemiesToSpawn += enemiesIncrementPerWave;
            spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - spawnIntervalDecrease);
            yield return new WaitForSeconds(3f);
        }

        Debug.Log("All waves finished!");
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemyObj = EnemyPoolManager.Instance.GetEnemy(spawnPoint.position);

        TargetChecker checker = enemyObj.GetComponent<TargetChecker>();
        if (checker != null)
        {
            checker.SetTargets(GameManager.Instance.PlayerTransform, GameManager.Instance.BaseTransform);
        }

        EnemyHealth health = enemyObj.GetComponent<EnemyHealth>();
        if (health != null)
            health.OnEnemyDeath += HandleEnemyDeath;
    }


    private void HandleEnemyDeath()
    {
        _aliveEnemies--;
    }

    public void ResetWaves()
    {
        StopAllCoroutines();
        _currentWave = 0;
        totalEnemiesToSpawn = _initialTotalEnemiesToSpawn;
        spawnInterval = _initialSpawnInterval;
    }
}
