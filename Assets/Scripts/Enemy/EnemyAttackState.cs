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
        bool baseInRange = _enemyStateManager.TargetChecker.IsBaseInAttackRange(_enemyStateManager.AttackRange);
        bool playerInRange = _enemyStateManager.TargetChecker.IsPlayerInAttackRange(_enemyStateManager.AttackRange);

        if (baseInRange)
        {
            HandleAttack();
            return;
        }

        if (playerInRange)
        {
            HandleAttack();
            return;
        }

        _enemyStateManager.ChangeState(_enemyStateManager.EnemyChaseState);
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