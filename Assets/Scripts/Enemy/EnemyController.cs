using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    
    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }
    
    public void MoveTo(Transform targetTransform)
    {
        agent.isStopped = false;
        agent.SetDestination(targetTransform.position);
    }

    public void MoveTo(Vector3 targetPosition)
    {
        agent.isStopped = false;
        agent.SetDestination(targetPosition);
    }

    public void StopMoving()
    {
        agent.isStopped = true;
    }
    
    public void ResumeMoving()
    {
        agent.isStopped = false;
    }

    public bool ReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }
}