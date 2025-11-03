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
        bool baseInSight = _enemyStateManager.TargetChecker.IsBaseInRange();
        bool baseInAttackRange = _enemyStateManager.TargetChecker.IsBaseInAttackRange(_enemyStateManager.AttackRange);
        bool playerInSight = _enemyStateManager.TargetChecker.IsPlayerInRange();
        bool playerInAttackRange = _enemyStateManager.TargetChecker.IsPlayerInAttackRange(_enemyStateManager.AttackRange);

        if (baseInSight)
        {
            if (baseInAttackRange)
            {
                HandleAttack();
                return;
            }

            _enemyStateManager.ChangeState(_enemyStateManager.EnemyChaseState);
            return;
        }

        if (playerInSight)
        {
            if (playerInAttackRange)
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