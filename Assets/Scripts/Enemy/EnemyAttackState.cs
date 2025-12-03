using UnityEngine;

public class EnemyAttackState : IState
{
    private EnemyStateManager _enemyStateManager;
    private EnemyAttack _enemyAttack;
    private float _attackTimer;

    public EnemyAttackState(EnemyStateManager enemyStateManager)
    {
        _enemyStateManager = enemyStateManager;
        _enemyAttack = enemyStateManager.GetComponentInChildren<EnemyAttack>();
    }

    public void Enter()
    {
        _attackTimer = 0f;
        _enemyStateManager.EnemyController.StopMoving();
    }

    public void Exit() { }

    public void Update()
    {
        if (_enemyStateManager.TargetChecker.IsPathToBaseBlocked())
        {
            if (_enemyStateManager.TargetChecker.TryGetBlockingBarrier(out Transform blockingBarrier))
            {
                float dist = Vector3.Distance(blockingBarrier.position, _enemyStateManager.transform.position);

                if (dist <= _enemyStateManager.AttackRange)
                {
                    HandleAttack();
                    return;
                }

                _enemyStateManager.ChangeState(_enemyStateManager.EnemyChaseState);
                return;
            }

            _enemyStateManager.ChangeState(_enemyStateManager.EnemyPatrolState);
            return;
        }

        bool baseInRange = _enemyStateManager.TargetChecker.IsBaseInRange();
        bool baseInAttack = _enemyStateManager.TargetChecker.IsBaseInAttackRange(_enemyStateManager.AttackRange);
        bool playerInRange = _enemyStateManager.TargetChecker.IsPlayerInRange();
        bool playerInAttack = _enemyStateManager.TargetChecker.IsPlayerInAttackRange(_enemyStateManager.AttackRange);

        if (baseInRange)
        {
            if (baseInAttack)
            {
                HandleAttack();
                return;
            }

            _enemyStateManager.ChangeState(_enemyStateManager.EnemyChaseState);
            return;
        }

        if (playerInRange)
        {
            if (playerInAttack)
            {
                HandleAttack();
                return;
            }

            _enemyStateManager.ChangeState(_enemyStateManager.EnemyChaseState);
            return;
        }

        _enemyStateManager.ChangeState(_enemyStateManager.EnemyPatrolState);
    }

    private void HandleAttack()
    {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0f)
        {
            _enemyAttack?.StartAttack();
            _attackTimer = _enemyStateManager.AttackCooldown;
        }
    }

    public void FixedUpdate() { }
}
