using UnityEngine;

public class EnemyIdleState : IState
{
    private EnemyStateManager _enemyStateManager;
    private float _idleTimer;

    public EnemyIdleState(EnemyStateManager enemyStateManager)
    {
        _enemyStateManager = enemyStateManager;
    }
    
    public void Enter()
    {
        Debug.Log("Entered Idle State");
        _idleTimer = _enemyStateManager.IdleDuration;
        _enemyStateManager.EnemyController.StopMoving();
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }

    public void Update()
    {
        _idleTimer -= Time.deltaTime;
        if (_idleTimer <= 0f)
        {
            _enemyStateManager.ChangeState(_enemyStateManager.EnemyPatrolState);
        }

        if (_enemyStateManager.TargetChecker.IsPlayerInRange())
        {
            _enemyStateManager.ChangeState(_enemyStateManager.EnemyAttackState);
        }
    }

    public void FixedUpdate() { }
}