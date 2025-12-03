using UnityEngine;

public class Barrier : DeployableBase
{
    [SerializeField] private int maxHealth = 100;
    private int _currentHealth;

    void OnEnable()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - amount);
        if (_currentHealth <= 0)
            Destroy(gameObject);
    }
}