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
        Debug.Log("Entered MoveToBase State");
        _enemyStateManager.EnemyController.ResumeMoving();
        _enemyStateManager.EnemyController.MoveTo(_enemyStateManager.TargetChecker.GetBasePosition());
    }

    public void Exit()
    {
        Debug.Log("Exiting MoveToBase State");
    }

    public void Update()
    {
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
