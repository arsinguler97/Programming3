using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public event System.Action<int> OnHealthChanged;
    public event System.Action OnEnemyDeath;

    [SerializeField] private int enemyMaxHealth = 100;
    [SerializeField] private GameObject collectablePrefab;
    [SerializeField] private Vector3 dropOffset = new Vector3(0, 0.5f, 0);

    private int _enemyCurrentHealth;

    public int EnemyCurrentHealth => _enemyCurrentHealth;
    public int EnemyMaxHealth => enemyMaxHealth;

    private void Start()
    {
        _enemyCurrentHealth = enemyMaxHealth;
        OnHealthChanged?.Invoke(_enemyCurrentHealth);
    }

    private void OnEnable()
    {
        _enemyCurrentHealth = enemyMaxHealth;
        OnHealthChanged?.Invoke(_enemyCurrentHealth);
    }

    public void EnemyTakeDamage(int damage)
    {
        _enemyCurrentHealth = Mathf.Max(0, _enemyCurrentHealth - damage);
        OnHealthChanged?.Invoke(_enemyCurrentHealth);

        if (_enemyCurrentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        EnemyTakeDamage(damage);
    }

    private void Die()
    {
        OnEnemyDeath?.Invoke();

        if (collectablePrefab != null)
        {
            Instantiate(collectablePrefab, transform.position + dropOffset, Quaternion.identity);
        }

        gameObject.SetActive(false);
    }
}