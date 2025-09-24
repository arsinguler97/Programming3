using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float swingSpeed = 200f;
    [SerializeField] private float upAngle = 30f;
    [SerializeField] private float downAngle = -60f;
    [SerializeField] private float attackCooldown = 1f;

    private Transform _pivot;
    private bool _isAttacking;
    private int _phase;
    private float _startAngle;
    private float _cooldownTimer;

    void Start()
    {
        _pivot = transform.parent;
        _startAngle = _pivot.localEulerAngles.x;
    }

    public void TryAttack()
    {
        if (_cooldownTimer <= 0f && !_isAttacking)
        {
            _isAttacking = true;
            _phase = 0;
            _cooldownTimer = attackCooldown;
        }
    }

    void Update()
    {
        if (_cooldownTimer > 0f)
            _cooldownTimer -= Time.deltaTime;

        if (!_isAttacking) return;

        float step = swingSpeed * Time.deltaTime;
        Vector3 euler = _pivot.localEulerAngles;

        if (_phase == 0)
        {
            float target = upAngle;
            euler.x = Mathf.MoveTowardsAngle(euler.x, target, step);
            _pivot.localEulerAngles = euler;
            if (Mathf.Approximately(Mathf.DeltaAngle(euler.x, target), 0))
                _phase = 1;
        }
        else if (_phase == 1)
        {
            float target = downAngle;
            euler.x = Mathf.MoveTowardsAngle(euler.x, target, step);
            _pivot.localEulerAngles = euler;
            if (Mathf.Approximately(Mathf.DeltaAngle(euler.x, target), 0))
                _phase = 2;
        }
        else if (_phase == 2)
        {
            float target = _startAngle;
            euler.x = Mathf.MoveTowardsAngle(euler.x, target, step);
            _pivot.localEulerAngles = euler;
            if (Mathf.Approximately(Mathf.DeltaAngle(euler.x, target), 0))
                _isAttacking = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isAttacking && other.CompareTag("Enemy"))
        {
            EnemyHealth health = other.GetComponent<EnemyHealth>();
            if (health != null)
                health.EnemyTakeDamage(damageAmount);
        }
    }
}
