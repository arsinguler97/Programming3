using UnityEngine;

public class BarrierHealth : MonoBehaviour
{
    public event System.Action<int> OnHealthChanged;
    public event System.Action OnBarrierDestroyed;

    [SerializeField] private int maxHealth = 200;

    private int _currentHealth;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => maxHealth;

    private void OnEnable()
    {
        _currentHealth = maxHealth;
        OnHealthChanged?.Invoke(_currentHealth);
    }

    public void TakeDamage(int amount)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - amount);
        OnHealthChanged?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            OnBarrierDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}