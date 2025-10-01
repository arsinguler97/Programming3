using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    private EnemyIdleState _enemyIdleState;
    private EnemyPatrolState _enemyPatrolState;
    private EnemyChaseState _enemyChaseState;
    private EnemyAttackState _enemyAttackState;

    // State Properties
    public EnemyPatrolState EnemyPatrolState => _enemyPatrolState;
    public EnemyAttackState EnemyAttackState => _enemyAttackState;
    public EnemyIdleState EnemyIdleState => _enemyIdleState;
    public EnemyChaseState EnemyChaseState => _enemyChaseState;

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
    
    [SerializeField] private float idleDuration;
    public float IdleDuration => idleDuration;
    
    private IState _currentState;

    private void Awake()
    {
        _enemyPatrolState = new EnemyPatrolState(this);
        _enemyChaseState = new EnemyChaseState(this);
        _enemyIdleState = new EnemyIdleState(this);
        _enemyAttackState = new EnemyAttackState(this);
    }

    // Change the state
    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
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
}