using UnityEngine;

public class PlayerChecker : MonoBehaviour
{
    [SerializeField] private float viewRadius = 10f;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask obstacleMask;

    public bool IsPlayerInRange()
    {
        Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;

        if (Vector3.Distance(transform.position, playerTransform.position) <= viewRadius)
        {
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2f)
            {
                if (!Physics.Raycast(transform.position, dirToPlayer, 
                        Vector3.Distance(transform.position, playerTransform.position), obstacleMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsPlayerInAttackRange(float attackRange)
    {
        return Vector3.Distance(playerTransform.position, transform.position) <= attackRange;
    }

    public Vector3 GetPlayerPosition()
    {
        return playerTransform.position;
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
    }
}