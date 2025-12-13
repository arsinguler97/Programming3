using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float regenRate = 15f;
    [SerializeField] private float sprintDrainPerSecond = 20f;
    [SerializeField] private float aimDrainPerSecond = 8f;
    [SerializeField] private float meleeCost = 15f;
    [SerializeField] private float jumpCost = 20f;
    [SerializeField] private float aimStartCost = 5f;
    [SerializeField] private float regenDelay = 2f;

    [Header("UI")]
    [SerializeField] private Slider staminaSlider;

    private float _current;
    private bool _isSprinting;
    private bool _isAiming;
    private float _lastConsumeTime;

    public bool HasStamina => _current > 0.01f;

    private void Awake()
    {
        _current = maxStamina;
        _lastConsumeTime = -regenDelay;
        UpdateUI();
    }

    private void Update()
    {
        float drain = 0f;
        if (_isSprinting) drain += sprintDrainPerSecond;
        if (_isAiming) drain += aimDrainPerSecond;

        if (drain > 0f)
        {
            _current = Mathf.Max(0f, _current - drain * Time.deltaTime);
            _lastConsumeTime = Time.time;
        }
        else
        {
            if (Time.time - _lastConsumeTime >= regenDelay)
                _current = Mathf.Min(maxStamina, _current + regenRate * Time.deltaTime);
        }

        UpdateUI();
    }

    public bool TryConsumeMelee()
    {
        return TryConsume(meleeCost);
    }

    public bool TryConsumeJump()
    {
        return TryConsume(jumpCost);
    }

    public bool TryConsumeAimStart()
    {
        return TryConsume(aimStartCost);
    }

    public void SetSprinting(bool sprinting)
    {
        _isSprinting = sprinting && HasStamina;
    }

    public void SetAiming(bool aiming)
    {
        _isAiming = aiming && HasStamina;
    }

    public bool CanStartSprint()
    {
        return HasStamina;
    }

    private bool TryConsume(float cost)
    {
        if (_current < cost)
            return false;

        _current -= cost;
        _lastConsumeTime = Time.time;
        UpdateUI();
        return true;
    }

    private void UpdateUI()
    {
        if (staminaSlider == null)
            return;

        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = _current;
    }
}
