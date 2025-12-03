using UnityEngine;
using UnityEngine.AI;

public class TargetChecker : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask barrierMask;
    [SerializeField] private float viewRadius = 10f;
    [SerializeField] private float viewAngle = 60f;

    private Transform playerTransform;
    private Transform baseTransform;

    private readonly Collider[] _barrierResults = new Collider[20];

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

    public bool IsPathToBaseBlocked()
    {
        if (baseTransform == null) return false;

        NavMeshPath path = new NavMeshPath();
        bool found = NavMesh.CalculatePath(transform.position, baseTransform.position, NavMesh.AllAreas, path);

        if (!found) return true;
        return path.status != NavMeshPathStatus.PathComplete;
    }

    public bool TryGetBlockingBarrier(out Transform barrier)
    {
        barrier = null;

        if (baseTransform == null) return false;

        NavMeshPath path = new NavMeshPath();
        if (!NavMesh.CalculatePath(transform.position, baseTransform.position, NavMesh.AllAreas, path))
            return false;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Vector3 start = path.corners[i];
            Vector3 end = path.corners[i + 1];

            Vector3 dir = (end - start).normalized;
            float dist = Vector3.Distance(start, end);

            if (Physics.Raycast(start, dir, out RaycastHit hit, dist, barrierMask))
            {
                barrier = hit.transform;
                return true;
            }
        }

        return false;
    }

    public bool TryGetNearestBarrier(out Transform barrierTransform)
    {
        barrierTransform = null;

        int count = Physics.OverlapSphereNonAlloc(transform.position, viewRadius, _barrierResults, barrierMask);

        float best = Mathf.Infinity;

        for (int i = 0; i < count; i++)
        {
            float dist = Vector3.Distance(transform.position, _barrierResults[i].transform.position);

            if (dist < best)
            {
                best = dist;
                barrierTransform = _barrierResults[i].transform;
            }
        }

        return barrierTransform != null;
    }
}
