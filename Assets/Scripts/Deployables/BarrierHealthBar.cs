using UnityEngine;
using UnityEngine.UI;

public class BarrierHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private BarrierHealth _barrierHealth;
    private Camera _camera;

    private void Start()
    {
        _barrierHealth = GetComponentInParent<BarrierHealth>();
        _camera = Camera.main;

        healthSlider.maxValue = _barrierHealth.MaxHealth;
        healthSlider.value = _barrierHealth.CurrentHealth;

        _barrierHealth.OnHealthChanged += UpdateHealth;
        _barrierHealth.OnBarrierDestroyed += HideBar;
    }

    private void LateUpdate()
    {
        if (_camera != null)
            transform.LookAt(_camera.transform.position);
    }

    private void OnDestroy()
    {
        if (_barrierHealth != null)
        {
            _barrierHealth.OnHealthChanged -= UpdateHealth;
            _barrierHealth.OnBarrierDestroyed -= HideBar;
        }
    }

    private void UpdateHealth(int value)
    {
        healthSlider.value = value;
    }

    private void HideBar()
    {
        healthSlider.value = 0;
    }

    private void OnEnable()
    {
        if (_barrierHealth != null)
        {
            healthSlider.maxValue = _barrierHealth.MaxHealth;
            healthSlider.value = _barrierHealth.CurrentHealth;
        }
    }
}