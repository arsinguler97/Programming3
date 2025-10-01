using UnityEngine;
using Unity.Cinemachine;

public class BowController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private CinemachineCamera defaultCamera;
    [SerializeField] private CinemachineCamera aimCamera;
    [SerializeField] private CinemachineBrain cinemachineBrain;

    private float _chargeTime;
    private bool _isCharging;

    private void Start()
    {
        defaultCamera.Priority = 20;
        aimCamera.Priority = 10;
    }

    public void EnterAimMode()
    {
        cinemachineBrain.DefaultBlend.Time = 2f;
        aimCamera.Priority = 20;
        defaultCamera.Priority = 10;
    }

    public void ExitAimMode()
    {
        cinemachineBrain.DefaultBlend.Time = 0.75f;
        aimCamera.Priority = 10;
        defaultCamera.Priority = 20;
    }

    public void StartCharging()
    {
        _isCharging = true;
        _chargeTime = 0f;
    }

    public void ReleaseArrow()
    {
        if (!_isCharging) return;
        _isCharging = false;

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);

        Vector3 direction = firePoint.forward;
        if (Camera.main != null && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 1000f))
        {
            direction = (hit.point - firePoint.position).normalized;
        }
        arrow.transform.rotation = Quaternion.LookRotation(direction);

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.SetInitialSpeed(_chargeTime);
        }
    }

    private void Update()
    {
        if (_isCharging)
            _chargeTime += Time.deltaTime;
    }
}