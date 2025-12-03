using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private EnemyHealth _enemyHealth;
    private Camera _camera;

    private void Start()
    {
        _enemyHealth = GetComponentInParent<EnemyHealth>();
        _camera = Camera.main;

        healthSlider.maxValue = _enemyHealth.EnemyMaxHealth;
        healthSlider.value = _enemyHealth.EnemyCurrentHealth;

        _enemyHealth.OnHealthChanged += UpdateHealthDisplay;
        _enemyHealth.OnEnemyDeath += HandleEnemyDeath;
    }

    private void LateUpdate()
    {
        if (_camera != null)
        {
            transform.LookAt(_camera.transform.position);
        }
    }

    private void OnDestroy()
    {
        if (_enemyHealth != null)
        {
            _enemyHealth.OnHealthChanged -= UpdateHealthDisplay;
            _enemyHealth.OnEnemyDeath -= HandleEnemyDeath;
        }
    }

    private void UpdateHealthDisplay(int newHealth)
    {
        healthSlider.value = newHealth;
    }

    private void HandleEnemyDeath()
    {
        healthSlider.value = 0;
    }
    private void OnEnable()
    {
        if (_enemyHealth != null)
        {
            healthSlider.maxValue = _enemyHealth.EnemyMaxHealth;
            healthSlider.value = _enemyHealth.EnemyCurrentHealth;
        }
    }
}