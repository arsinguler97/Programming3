using UnityEngine;

public class EnemyBarrierState : IState
{
    private readonly EnemyStateManager _enemyStateManager;
    private float _lastMoveLogTime;

    public EnemyBarrierState(EnemyStateManager enemyStateManager)
    {
        _enemyStateManager = enemyStateManager;
    }

    public void Enter()
    {
        _enemyStateManager.EnemyController.ResumeMoving();
        TrySetBarrierTarget();
        _lastMoveLogTime = -999f;
    }

    public void Exit()
    {
        _enemyStateManager.EnemyController.ResetStoppingDistance();
    }

    public void Update()
    {
        if (!_enemyStateManager.TargetChecker.IsPathToBaseBlocked())
        {
            _enemyStateManager.SetBarrierTarget(null);
            _enemyStateManager.ChangeState(_enemyStateManager.EnemyPatrolState);
            return;
        }

        if (!EnsureBarrierTarget()) return;

        Transform barrier = _enemyStateManager.CurrentBarrierTarget;
        if (barrier == null) return;

        Vector3 targetPos = barrier.position;

        FaceTarget(targetPos);

        float attackDistance = Mathf.Max(_enemyStateManager.AttackRange, _enemyStateManager.EnemyAttackState.AttackRadius + 0.5f);

        float dist = Vector3.Distance(targetPos, _enemyStateManager.transform.position);
        if (dist <= attackDistance)
        {
            _enemyStateManager.ChangeState(_enemyStateManager.EnemyAttackState);
            return;
        }

        if (_enemyStateManager.TargetChecker.TryGetReachablePointNearBarrier(barrier, _enemyStateManager.BarrierApproachRadius, _enemyStateManager.transform.position, out Vector3 navPoint))
        {
            _enemyStateManager.EnemyController.SetStoppingDistance(attackDistance * 0.8f);
            _enemyStateManager.EnemyController.MoveTo(navPoint);

            if (Time.time - _lastMoveLogTime > 0.5f)
            {
                Debug.Log($"{_enemyStateManager.name} BarrierState moving to {navPoint} remainingDist {_enemyStateManager.EnemyController.AgentRemainingDistance():F2}");
                _lastMoveLogTime = Time.time;
            }
        }
        else
        {
            // Navmesh point not found; try moving directly to barrier position
            _enemyStateManager.EnemyController.SetStoppingDistance(attackDistance * 0.8f);
            _enemyStateManager.EnemyController.MoveTo(targetPos);
            if (Time.time - _lastMoveLogTime > 0.5f)
            {
                Debug.Log($"{_enemyStateManager.name} BarrierState moving directly to barrier {targetPos} remainingDist {_enemyStateManager.EnemyController.AgentRemainingDistance():F2}");
                _lastMoveLogTime = Time.time;
            }
        }
    }

    public void FixedUpdate() { }

    private bool EnsureBarrierTarget()
    {
        if (_enemyStateManager.CurrentBarrierTarget != null) return true;
        return TrySetBarrierTarget();
    }

    private bool TrySetBarrierTarget()
    {
        if (_enemyStateManager.TargetChecker.TryGetClosestBlockingBarrier(out Transform blockingBarrier))
        {
            _enemyStateManager.SetBarrierTarget(blockingBarrier, Vector3.zero, false);
            return true;
        }

        return false;
    }

    private void FaceTarget(Vector3 targetPos)
    {
        Vector3 dir = targetPos - _enemyStateManager.transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir.normalized);
        _enemyStateManager.transform.rotation = Quaternion.Slerp(
            _enemyStateManager.transform.rotation,
            targetRot,
            10f * Time.deltaTime
        );
    }
}
