using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class TargetChecker : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask barrierMask;
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

    public bool IsPathToBaseBlocked()
    {
        if (baseTransform == null) return false;

        NavMeshPath path = new NavMeshPath();
        bool found = NavMesh.CalculatePath(transform.position, baseTransform.position, NavMesh.AllAreas, path);

        if (!found) return true;
        return path.status != NavMeshPathStatus.PathComplete;
    }

    public bool TryGetClosestBlockingBarrier(out Transform barrier)
    {
        barrier = null;

        if (baseTransform == null) return false;

        return TryGetNearestBarrier(out barrier);
    }

    public bool HasPathTo(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        bool found = NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
        return found && path.status == NavMeshPathStatus.PathComplete;
    }

    public bool TryGetReachablePointNearBarrier(Transform barrier, float radius, Vector3 fromPosition, out Vector3 point)
    {
        point = barrier.position;
        if (barrier == null) return false;

        float r = Mathf.Max(radius, 0.1f);

        if (NavMesh.SamplePosition(barrier.position, out NavMeshHit hit, r, NavMesh.AllAreas))
        {
            if (Vector3.Distance(hit.position, barrier.position) <= r && HasPath(fromPosition, hit.position))
            {
                point = hit.position;
                Debug.Log($"{name}: nav point near barrier {barrier.name} found at {point} (r={r})");
                return true;
            }
        }

        Vector3[] offsets = new Vector3[]
        {
            Vector3.forward, Vector3.back, Vector3.left, Vector3.right
        };

        for (int i = 0; i < offsets.Length; i++)
        {
            Vector3 samplePos = barrier.position + offsets[i] * r;
            if (NavMesh.SamplePosition(samplePos, out NavMeshHit offsetHit, r, NavMesh.AllAreas))
            {
                if (Vector3.Distance(offsetHit.position, barrier.position) <= r && HasPath(fromPosition, offsetHit.position))
                {
                    point = offsetHit.position;
                    Debug.Log($"{name}: nav point offset near barrier {barrier.name} found at {point}");
                    return true;
                }
            }
        }

        Debug.Log($"{name}: no reachable nav point near barrier {barrier.name}");
        return false;
    }

    public bool TryGetNearestBarrier(out Transform barrier)
    {
        barrier = null;
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("Barrier");
        float best = Mathf.Infinity;

        foreach (var go in barriers)
        {
            float dist = Vector3.Distance(transform.position, go.transform.position);
            if (dist < best)
            {
                best = dist;
                barrier = go.transform;
            }
        }

        if (barrier != null)
            Debug.Log($"{name}: nearest barrier fallback {barrier.name} at dist {Vector3.Distance(transform.position, barrier.position):F1}");
        else
            Debug.Log($"{name}: no barrier found with tag Barrier");

        return barrier != null;
    }

    private bool HasPath(Vector3 from, Vector3 to)
    {
        NavMeshPath path = new NavMeshPath();
        if (!NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path))
            return false;
        return path.status == NavMeshPathStatus.PathComplete;
    }
}
