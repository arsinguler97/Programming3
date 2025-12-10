using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    private EnemyPatrolState _enemyPatrolState;
    private EnemyChaseState _enemyChaseState;
    private EnemyAttackState _enemyAttackState;
    private EnemyBarrierState _enemyBarrierState;

    // State Properties
    public EnemyPatrolState EnemyPatrolState => _enemyPatrolState;
    public EnemyAttackState EnemyAttackState => _enemyAttackState;
    public EnemyChaseState EnemyChaseState => _enemyChaseState;
    public EnemyBarrierState EnemyBarrierState => _enemyBarrierState;

    // References to components
    [SerializeField] private TargetChecker targetChecker;
    public TargetChecker TargetChecker => targetChecker;

    [SerializeField] private EnemyController enemyController;
    public EnemyController EnemyController => enemyController;
    
    [SerializeField] private float attackRange = 2f;
    public float AttackRange => attackRange;
    
    [SerializeField] private float attackCooldown = 1.5f;
    public float AttackCooldown => attackCooldown;
    
    [SerializeField] private float chaseSpeed;
    public float ChaseSpeed => chaseSpeed;

    [SerializeField] private float barrierApproachRadius = 2f;
    public float BarrierApproachRadius => barrierApproachRadius;
    
    private IState _currentState;
    private Transform _currentBarrierTarget;
    private Vector3 _currentBarrierAttackPoint;
    private bool _hasBarrierAttackPoint;
    private bool _inBaseTrigger;

    public Transform CurrentBarrierTarget => _currentBarrierTarget;
    public Vector3 CurrentBarrierAttackPoint => _currentBarrierAttackPoint;
    public bool HasBarrierAttackPoint => _hasBarrierAttackPoint;
    public bool InBaseTrigger => _inBaseTrigger;

    private void Awake()
    {
        _enemyPatrolState = new EnemyPatrolState(this);
        _enemyChaseState = new EnemyChaseState(this);
        _enemyAttackState = new EnemyAttackState(this);
        _enemyBarrierState = new EnemyBarrierState(this);
    }

    public void ChangeState(IState newState)
    {
        if (_currentState == newState) return;

        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();

        if (newState != null)
            Debug.Log($"{name} -> {_currentState.GetType().Name}");
    }

    void Start()
    {
        ChangeState(_enemyPatrolState);
    }

    void Update()
    {
        _currentState?.Update();
    }

    private void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }

    public void SetBarrierTarget(Transform target)
    {
        SetBarrierTarget(target, Vector3.zero, false);
    }

    public void SetBarrierTarget(Transform target, Vector3 attackPoint, bool hasAttackPoint)
    {
        if (_currentBarrierTarget == target && hasAttackPoint == _hasBarrierAttackPoint && (!hasAttackPoint || Vector3.Distance(_currentBarrierAttackPoint, attackPoint) < 0.01f))
            return;

        _currentBarrierTarget = target;
        _currentBarrierAttackPoint = attackPoint;
        _hasBarrierAttackPoint = hasAttackPoint;

        if (_currentBarrierTarget != null)
        {
            string msg = hasAttackPoint
                ? $"{name}: targeting barrier {_currentBarrierTarget.name} attackPoint {_currentBarrierAttackPoint}"
                : $"{name}: targeting barrier {_currentBarrierTarget.name}";
            Debug.Log(msg);
        }
    }

    public void LogCannotReachBarrier()
    {
        if (_currentBarrierTarget == null) return;
        Debug.Log($"{name}: cannot reach barrier {_currentBarrierTarget.name}");
    }

    public void SetInBaseTrigger(bool value)
    {
        _inBaseTrigger = value;
    }
}
