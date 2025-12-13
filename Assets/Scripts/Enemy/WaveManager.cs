using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
    [SerializeField] private WaveUI waveUI; // Optional manual assignment to avoid timing issues

    private int _currentWave;
    private int _spawnedEnemies;
    private int _aliveEnemies;
    private int _initialTotalEnemiesToSpawn;
    private float _initialSpawnInterval;
    private WaveUI _waveUI;
    private float _spawnRateMultiplier = 1f;

    private void Start()
    {
        _initialTotalEnemiesToSpawn = totalEnemiesToSpawn;
        _initialSpawnInterval = spawnInterval;

        ResolveWaveUI();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        ResolveWaveUI();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResolveWaveUI();
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

            ResolveWaveUI();
            if (_waveUI != null)
            {
                _waveUI.UpdateWave(_currentWave, waveCount);
                UpdateRemainingEnemiesUI();
            }

            while (_spawnedEnemies < totalEnemiesToSpawn)
            {
                SpawnEnemy();
                _spawnedEnemies++;
                _aliveEnemies++;

                if (_waveUI != null)
                    UpdateRemainingEnemiesUI();

                yield return new WaitForSeconds(GetCurrentSpawnDelay());
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
        ResolveWaveUI();

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
        UpdateRemainingEnemiesUI();
    }

    public void ResetWaves()
    {
        StopAllCoroutines();
        _currentWave = 0;
        totalEnemiesToSpawn = _initialTotalEnemiesToSpawn;
        spawnInterval = _initialSpawnInterval;

        if (_waveUI != null)
        {
            _waveUI.UpdateWave(0, waveCount);
            _waveUI.UpdateRemainingEnemies(0);
        }
    }

    private void UpdateRemainingEnemiesUI()
    {
        int remainingEnemies = totalEnemiesToSpawn - _spawnedEnemies + _aliveEnemies;
        remainingEnemies = Mathf.Max(remainingEnemies, 0);

        _waveUI?.UpdateRemainingEnemies(remainingEnemies);
    }

    private float GetCurrentSpawnDelay()
    {
        float delay = spawnInterval * _spawnRateMultiplier;
        return Mathf.Max(minSpawnInterval, delay);
    }

    public void SetSpawnRateMultiplier(float multiplier)
    {
        // Prevent zero or negative multipliers.
        _spawnRateMultiplier = Mathf.Max(0.01f, multiplier);
    }

    public void SetSpawnDelaySlower50() => SetSpawnRateMultiplier(1.5f);   // +50%
    public void SetSpawnDelaySlower100() => SetSpawnRateMultiplier(2f);   // +100%
    public void SetSpawnDelayNormal() => SetSpawnRateMultiplier(1f);      // default
    public void SetSpawnDelayFaster50() => SetSpawnRateMultiplier(0.5f);  // -50%

    private void ResolveWaveUI()
    {
        if (_waveUI != null)
            return;

        _waveUI = waveUI != null ? waveUI : FindFirstObjectByType<WaveUI>();
        if (_waveUI != null)
        {
            _waveUI.UpdateWave(_currentWave, waveCount);
            _waveUI.UpdateRemainingEnemies(totalEnemiesToSpawn - _spawnedEnemies + _aliveEnemies);
        }
    }
}
