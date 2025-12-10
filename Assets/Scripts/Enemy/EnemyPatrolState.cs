using UnityEngine;

public class EnemyPatrolState : IState
{
    private EnemyStateManager _enemyStateManager;

    public EnemyPatrolState(EnemyStateManager enemyStateManager)
    {
        _enemyStateManager = enemyStateManager;
    }

    public void Enter()
    {
        _enemyStateManager.EnemyController.ResumeMoving();
        _enemyStateManager.EnemyController.MoveTo(_enemyStateManager.TargetChecker.GetBasePosition());
    }

    public void Exit()
    {
        _enemyStateManager.EnemyController.ResetStoppingDistance();
    }

    public void Update()
    {
        if (_enemyStateManager.TargetChecker.IsPathToBaseBlocked())
        {
            _enemyStateManager.ChangeState(_enemyStateManager.EnemyBarrierState);
            return;
        }
        
        if (_enemyStateManager.TargetChecker.IsPlayerInRange())
        {
            _enemyStateManager.ChangeState(_enemyStateManager.EnemyChaseState);
            return;
        }

        if (_enemyStateManager.EnemyController.ReachedDestination())
        {
            _enemyStateManager.ChangeState(_enemyStateManager.EnemyAttackState);
        }
    }

    public void FixedUpdate() { }
}
