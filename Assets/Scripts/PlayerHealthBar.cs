using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private PlayerHealth _playerHealth;
    
    private void Start()
    {
        _playerHealth = FindFirstObjectByType<PlayerHealth>();
        healthSlider.maxValue = _playerHealth.MaxHealth;
        healthSlider.value = _playerHealth.CurrentHealth;
        _playerHealth.OnHealthChanged += UpdateHealthDisplay;
    }
    
    private void OnDestroy()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthChanged -= UpdateHealthDisplay;
        }
    }
    
    private void UpdateHealthDisplay(int newHealth)
    {
        healthSlider.value = newHealth;
    }
}