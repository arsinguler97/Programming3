using UnityEngine;

public class EnemyPatrolState : IState
{
    private EnemyStateManager _enemyStateManager;
    private int _currentWaypointIndex;

    public EnemyPatrolState(EnemyStateManager enemyStateManager)
    {
        _enemyStateManager = enemyStateManager;
    }

    public void Enter()
    {
        Debug.Log("Entered Patrol State");
        _currentWaypointIndex = 0;
        if (_enemyStateManager.PatrolWaypoints.Length > 0)
            _enemyStateManager.EnemyController.MoveTo(_enemyStateManager.PatrolWaypoints[_currentWaypointIndex]);
    }

    public void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }

    public void Update()
    {
        if (_enemyStateManager.PlayerChecker.IsPlayerInRange())
        {
            _enemyStateManager.ChangeState(_enemyStateManager.EnemyChaseState);
            return;
        }

        if (_enemyStateManager.PatrolWaypoints.Length == 0)
            return;

        if (_enemyStateManager.EnemyController.ReachedDestination())
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _enemyStateManager.PatrolWaypoints.Length;
            _enemyStateManager.EnemyController.MoveTo(_enemyStateManager.PatrolWaypoints[_currentWaypointIndex]);
        }
    }

    public void FixedUpdate() { }
}