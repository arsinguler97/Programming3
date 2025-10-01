using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerAttack meleeAttack;
    [SerializeField] private BowController bowController;
    [SerializeField] private GameObject crosshair;

    private bool _isAiming;
    private PlayerMovement _playerMovement;

    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();

        if (crosshair != null)
            crosshair.SetActive(false);
    }

    public void SetAim(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _isAiming = true;
            _playerMovement?.SetAiming(true);
            bowController.EnterAimMode();
            if (crosshair != null) crosshair.SetActive(true);
        }
        else if (ctx.canceled)
        {
            _isAiming = false;
            _playerMovement?.SetAiming(false);
            bowController.ExitAimMode();
            if (crosshair != null) crosshair.SetActive(false);
        }
    }

    public void SetMeleeAttack(InputAction.CallbackContext ctx)
    {
        if (!_isAiming && ctx.performed)
        {
            meleeAttack.TryAttack();
        }
    }

    public void SetArrowCharge(InputAction.CallbackContext ctx)
    {
        if (_isAiming)
        {
            if (ctx.started)
            {
                bowController.StartCharging();
            }
            else if (ctx.canceled)
            {
                bowController.ReleaseArrow();
            }
        }
    }
}