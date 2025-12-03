using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float swingSpeed = 200f;
    [SerializeField] private float upAngle = 30f;
    [SerializeField] private float downAngle = -60f;
    [SerializeField] private float attackRadius = 2f; 
    [SerializeField] private LayerMask targetMask;

    private Transform _pivot;
    private bool _isAttacking;
    private int _phase;
    private float _startAngle;
    
    private readonly Collider[] _results = new Collider[10];

    void Start()
    {
        _pivot = transform.parent;
        _startAngle = _pivot.localEulerAngles.x;
    }

    public void StartAttack()
    {
        if (_isAttacking) return;

        _isAttacking = true;
        _phase = 0;

        ApplyDamage();
    }

    void Update()
    {
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

    private void ApplyDamage()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, attackRadius, _results, targetMask);

        Transform barrierTransform = null;
        PlayerHealth playerHealth = null;
        BaseHealth baseHealth = null;

        for (int i = 0; i < count; i++)
        {
            var hit = _results[i];

            if (hit.CompareTag("Barrier"))
            {
                barrierTransform = hit.transform;
            }
            else if (hit.CompareTag("Player"))
            {
                playerHealth = hit.GetComponent<PlayerHealth>();
            }
            else if (hit.CompareTag("Base"))
            {
                baseHealth = hit.GetComponent<BaseHealth>();
            }
        }

        if (barrierTransform != null)
        {
            barrierTransform.GetComponent<Barrier>().TakeDamage(damageAmount);
            return;
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
            return;
        }

        if (baseHealth != null)
        {
            baseHealth.TakeDamage(damageAmount);
            return;
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
