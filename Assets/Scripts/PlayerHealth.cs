using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event System.Action<int> OnHealthChanged;
    public event System.Action OnPlayerDeath;
    
    private int _currentHealth;
    private int _maxHealth = 100;
    
    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    
    private void Start()
    {
        _currentHealth = _maxHealth;
        OnHealthChanged?.Invoke(_currentHealth);
    }
    
    public void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - damage);
        OnHealthChanged?.Invoke(_currentHealth);
        
        if (_currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
    }
}