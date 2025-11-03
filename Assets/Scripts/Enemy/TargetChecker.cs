using UnityEngine;

public class TargetChecker : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float viewRadius = 10f;
    [SerializeField] private float viewAngle = 60f;
    
    private Transform playerTransform;
    private Transform baseTransform;
    
    public void SetTargets(Transform player, Transform baseT)
    {
        playerTransform = player;
        baseTransform = baseT;
    }

    public bool IsPlayerInRange()
    {
        if (playerTransform == null) return false;

        Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;
        float dist = Vector3.Distance(transform.position, playerTransform.position);

        if (dist <= viewRadius)
        {
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2f)
            {
                if (!Physics.Raycast(transform.position, dirToPlayer, dist, obstacleMask))
                    return true;
            }
        }
        return false;
    }

    public bool IsBaseInRange()
    {
        if (baseTransform == null) return false;

        Vector3 dirToBase = (baseTransform.position - transform.position).normalized;
        float dist = Vector3.Distance(transform.position, baseTransform.position);

        if (dist <= viewRadius)
        {
            if (Vector3.Angle(transform.forward, dirToBase) < viewAngle / 2f)
            {
                if (!Physics.Raycast(transform.position, dirToBase, dist, obstacleMask))
                    return true;
            }
        }
        return false;
    }

    public bool IsPlayerInAttackRange(float attackRange)
    {
        if (playerTransform == null) return false;
        return Vector3.Distance(playerTransform.position, transform.position) <= attackRange;
    }

    public bool IsBaseInAttackRange(float attackRange)
    {
        if (baseTransform == null) return false;
        return Vector3.Distance(baseTransform.position, transform.position) <= attackRange;
    }

    public Vector3 GetPlayerPosition()
    {
        return playerTransform != null ? playerTransform.position : transform.position;
    }

    public Vector3 GetBasePosition()
    {
        return baseTransform != null ? baseTransform.position : transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);

        if (baseTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, baseTransform.position);
        }
    }
}
