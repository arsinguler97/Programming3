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
        if (!_enemyStateManager.PlayerChecker.IsPlayerInAttackRange(_enemyStateManager.AttackRange))
        {
            _enemyStateManager.ChangeState(_enemyStateManager.EnemyChaseState);
            return;
        }

        _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0f)
        {
            _enemyAttack?.StartAttack();
            _attackTimer = _enemyStateManager.AttackCooldown;
        }
    }

    public void FixedUpdate() { }
}