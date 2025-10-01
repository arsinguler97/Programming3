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
        Debug.Log("Entered Chase State");
        _enemyStateManager.EnemyController.ResumeMoving();
        _enemyStateManager.EnemyController.SetSpeed(_enemyStateManager.ChaseSpeed);
    }

    public void Exit()
    {
        Debug.Log("Exiting Chase State");
    }

    public void Update()
    {
        if (_enemyStateManager.TargetChecker.IsPlayerInAttackRange(_enemyStateManager.AttackRange))
        {
            _enemyStateManager.ChangeState(_enemyStateManager.EnemyAttackState);
            return;
        }

        if (_enemyStateManager.TargetChecker.IsPlayerInRange())
        {
            _enemyStateManager.EnemyController.MoveTo(_enemyStateManager.TargetChecker.GetPlayerPosition());
        }
        else
        {
            _enemyStateManager.ChangeState(_enemyStateManager.EnemyPatrolState);
        }
    }
    
    /*public void Update()
    {
        if (_enemyStateManager.TargetChecker.IsBaseInAttackRange(_enemyStateManager.AttackRange))
        {
            _enemyStateManager.ChangeState(_enemyStateManager.EnemyAttackState);
            return;
        }

=        if (_enemyStateManager.TargetChecker.IsPlayerInAttackRange(_enemyStateManager.AttackRange))
        {
            _enemyStateManager.ChangeState(_enemyStateManager.EnemyAttackState);
            return;
        }

=        if (_enemyStateManager.TargetChecker.IsPlayerInRange())
        {
            _enemyStateManager.EnemyController.MoveTo(_enemyStateManager.TargetChecker.GetPlayerPosition());
        }
        else
        {
=            _enemyStateManager.ChangeState(_enemyStateManager.EnemyPatrolState);
        }
    }*/

    public void FixedUpdate() { }
}