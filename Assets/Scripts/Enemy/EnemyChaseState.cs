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

    public void Exit() { _enemyStateManager.EnemyController.ResetStoppingDistance(); }

    public void Update()
    {
        if (_enemyStateManager.TargetChecker.IsPathToBaseBlocked())
        {
            _enemyStateManager.ChangeState(_enemyStateManager.EnemyBarrierState);
            return;
        }

        if (_enemyStateManager.TargetChecker.IsBaseInRange())
        {
            if (_enemyStateManager.TargetChecker.IsBaseInAttackRange(_enemyStateManager.AttackRange))
            {
                _enemyStateManager.ChangeState(_enemyStateManager.EnemyAttackState);
                return;
            }

            _enemyStateManager.EnemyController.SetStoppingDistance(_enemyStateManager.AttackRange * 0.8f);
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

            _enemyStateManager.EnemyController.SetStoppingDistance(_enemyStateManager.AttackRange * 0.5f);
            _enemyStateManager.EnemyController.MoveTo(_enemyStateManager.TargetChecker.GetPlayerPosition());
            return;
        }

        _enemyStateManager.ChangeState(_enemyStateManager.EnemyPatrolState);
    }

    public void FixedUpdate() { }
}
