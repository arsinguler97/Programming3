using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    
    public float AgentSpeed => agent.speed;
    private float _defaultStoppingDistance;

    private void Awake()
    {
        if (agent != null)
            _defaultStoppingDistance = agent.stoppingDistance;
    }
    
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

    public void SetStoppingDistance(float distance)
    {
        agent.stoppingDistance = Mathf.Max(0f, distance);
    }

    public void ResetStoppingDistance()
    {
        agent.stoppingDistance = _defaultStoppingDistance;
    }

    public bool ReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }

    public float AgentRemainingDistance()
    {
        return agent.remainingDistance;
    }
}
