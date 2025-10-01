using UnityEngine;
using System.Collections.Generic;

public class EnemyPoolManager : Singleton<EnemyPoolManager>
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 20;

    private Queue<GameObject> _enemyPool = new Queue<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);

            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.OnEnemyDeath += () => ReturnToPool(enemy);
            }

            _enemyPool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemy(Vector3 position)
    {
        GameObject enemy;
        if (_enemyPool.Count > 0)
        {
            enemy = _enemyPool.Dequeue();
            enemy.transform.position = position;
            enemy.SetActive(true);
        }
        else
        {
            enemy = Instantiate(enemyPrefab, position, Quaternion.identity);

            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.OnEnemyDeath += () => ReturnToPool(enemy);
            }
        }

        return enemy;
    }

    private void ReturnToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        _enemyPool.Enqueue(enemy);
    }
}