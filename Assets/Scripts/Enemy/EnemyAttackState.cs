using UnityEngine;

public class EnemyAttackState : IState
{
    private EnemyStateManager _enemyStateManager;
    private EnemyAttack _enemyAttack;
    private float _attackTimer;
    private float _lastDebugLogTime;
    public float AttackRadius => _enemyAttack != null ? _enemyAttack.AttackRadius : 0f;

    public EnemyAttackState(EnemyStateManager enemyStateManager)
    {
        _enemyStateManager = enemyStateManager;
        _enemyAttack = enemyStateManager.GetComponentInChildren<EnemyAttack>();
    }

    public void Enter()
    {
        _attackTimer = 0f;
        _enemyStateManager.EnemyController.StopMoving();
        _lastDebugLogTime = -999f;

        Transform barrier = _enemyStateManager.CurrentBarrierTarget;
        if (barrier != null)
        {
            Vector3 targetPos = barrier.position;

            Vector3 dir = targetPos - _enemyStateManager.transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.0001f)
            {
                _enemyStateManager.transform.rotation = Quaternion.LookRotation(dir.normalized);
            }
        }
    }

    public void Exit()
    {
        _enemyStateManager.EnemyController.ResetStoppingDistance();
    }

    public void Update()
    {
        bool pathBlocked = _enemyStateManager.TargetChecker.IsPathToBaseBlocked();
        Transform barrier = _enemyStateManager.CurrentBarrierTarget;

        if (pathBlocked)
        {
            if (barrier != null && barrier.gameObject.activeInHierarchy)
            {
                Vector3 targetPos = barrier.position;
                if (_enemyStateManager.TargetChecker.TryGetReachablePointNearBarrier(barrier, _enemyStateManager.BarrierApproachRadius, _enemyStateManager.transform.position, out Vector3 navPoint))
                    targetPos = navPoint;

                FaceTarget(targetPos);

                float attackDistance = _enemyStateManager.AttackRange;
                if (_enemyAttack != null)
                {
                    attackDistance = Mathf.Max(attackDistance, _enemyAttack.AttackRadius + 0.5f);
                }

                float dist = Vector3.Distance(targetPos, _enemyStateManager.transform.position);
                if (Time.time - _lastDebugLogTime > 0.5f)
                {
                    float attackRadius = _enemyAttack != null ? _enemyAttack.AttackRadius : -1f;
                    Debug.Log($"{_enemyStateManager.name} AttackState barrier target {barrier.name} dist {dist:F2} attackRange {_enemyStateManager.AttackRange} attackRadius {attackRadius} attackDistance {attackDistance}");
                    _lastDebugLogTime = Time.time;
                }

                if (dist <= attackDistance)
                {
                    HandleAttack();
                    return;
                }

                _enemyStateManager.EnemyController.ResumeMoving();
                _enemyStateManager.EnemyController.SetStoppingDistance(attackDistance * 0.8f);
                _enemyStateManager.EnemyController.MoveTo(targetPos);
                return;
            }

            _enemyStateManager.ChangeState(_enemyStateManager.EnemyBarrierState);
            return;
        }
        else
        {
            if (barrier != null)
            {
                _enemyStateManager.SetBarrierTarget(null);
            }
        }

        bool baseInRange = _enemyStateManager.TargetChecker.IsBaseInRange() || _enemyStateManager.InBaseTrigger;
        bool baseInAttack = _enemyStateManager.InBaseTrigger || _enemyStateManager.TargetChecker.IsBaseInAttackRange(_enemyStateManager.AttackRange);
        bool playerInRange = _enemyStateManager.TargetChecker.IsPlayerInRange();
        bool playerInAttack = _enemyStateManager.TargetChecker.IsPlayerInAttackRange(_enemyStateManager.AttackRange);

        if (baseInRange)
        {
            if (baseInAttack)
            {
                FaceTarget(_enemyStateManager.TargetChecker.GetBasePosition());
                HandleAttack();
                return;
            }

            _enemyStateManager.ChangeState(_enemyStateManager.EnemyChaseState);
            return;
        }

        if (playerInRange)
        {
            if (playerInAttack)
            {
                FaceTarget(_enemyStateManager.TargetChecker.GetPlayerPosition());
                HandleAttack();
                return;
            }

            _enemyStateManager.ChangeState(_enemyStateManager.EnemyChaseState);
            return;
        }

        _enemyStateManager.ChangeState(_enemyStateManager.EnemyPatrolState);
    }

    private void HandleAttack()
    {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0f)
        {
            Debug.Log($"{_enemyStateManager.name} HandleAttack() executing");
            _enemyAttack?.StartAttack();
            _attackTimer = _enemyStateManager.AttackCooldown;
        }
    }

    public void FixedUpdate() { }

    private void FaceTarget(Vector3 targetPos)
    {
        Vector3 direction = targetPos - _enemyStateManager.transform.position;
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(direction.normalized);
        _enemyStateManager.transform.rotation = Quaternion.Slerp(
            _enemyStateManager.transform.rotation,
            targetRot,
            10f * Time.deltaTime
        );
    }
}
