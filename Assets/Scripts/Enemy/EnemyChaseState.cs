using UnityEngine;

public class EnemyChaseState : IState
{
    private EnemyStateManager _enemyStateManager;

    public EnemyChaseState(EnemyStateManager enemyStateManager)
    {
        _enemyStateManager = enemyStateManager;
    }

    public void Enter()
    {
        _enemyStateManager.EnemyController.ResumeMoving();
        _enemyStateManager.EnemyController.SetSpeed(_enemyStateManager.ChaseSpeed);
    }

    public void Exit() { }

    public void Update()
    {
        if (_enemyStateManager.TargetChecker.IsBaseInRange())
        {
            if (_enemyStateManager.TargetChecker.IsBaseInAttackRange(_enemyStateManager.AttackRange))
            {
                _enemyStateManager.ChangeState(_enemyStateManager.EnemyAttackState);
                return;
            }

            _enemyStateManager.EnemyController.MoveTo(_enemyStateManager.TargetChecker.GetBasePosition());
            return;
        }

        if (_enemyStateManager.TargetChecker.IsPlayerInRange())
        {
            if (_enemyStateManager.TargetChecker.IsPlayerInAttackRange(_enemyStateManager.AttackRange))
            {
                _enemyStateManager.ChangeState(_enemyStateManager.EnemyAttackState);
                return;
            }

            _enemyStateManager.EnemyController.MoveTo(_enemyStateManager.TargetChecker.GetPlayerPosition());
            return;
        }

        _enemyStateManager.ChangeState(_enemyStateManager.EnemyPatrolState);
    }

    public void FixedUpdate() { }
}