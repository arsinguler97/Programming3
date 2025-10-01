using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public event System.Action<int> OnHealthChanged;
    public event System.Action OnBaseDestroyed;

    private int _currentHealth;
    [SerializeField] private int _maxHealth = 200;

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
            OnBaseDestroyed?.Invoke();
            Debug.Log("Base destroyed!");
        }
    }
}