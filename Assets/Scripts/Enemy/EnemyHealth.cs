using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public event System.Action<int> OnHealthChanged;
    public event System.Action OnEnemyDeath;
    
    private int _enemyCurrentHealth;
    [SerializeField] private int enemyMaxHealth = 100;
    
    public int EnemyCurrentHealth => _enemyCurrentHealth;
    public int EnemyMaxHealth => enemyMaxHealth;
    
    private void Start()
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
            OnEnemyDeath?.Invoke();
            gameObject.SetActive(false);
        }
    }
}