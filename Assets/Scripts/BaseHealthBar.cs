using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BaseHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private BaseHealth _baseHealth;

    private void Start()
    {
        _baseHealth = FindFirstObjectByType<BaseHealth>();

        if (_baseHealth != null)
        {
            healthSlider.maxValue = _baseHealth.MaxHealth;
            healthSlider.value = _baseHealth.CurrentHealth;

            _baseHealth.OnHealthChanged += UpdateHealthDisplay;
            _baseHealth.OnBaseDestroyed += HandleBaseDestroyed;
        }
    }

    private void OnDestroy()
    {
        if (_baseHealth != null)
        {
            _baseHealth.OnHealthChanged -= UpdateHealthDisplay;
            _baseHealth.OnBaseDestroyed -= HandleBaseDestroyed;
        }
    }

    private void UpdateHealthDisplay(int newHealth)
    {
        healthSlider.value = newHealth;
    }

    private void HandleBaseDestroyed()
    {
        healthSlider.value = 0;
    }
}