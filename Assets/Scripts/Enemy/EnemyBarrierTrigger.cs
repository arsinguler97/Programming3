using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyBarrierTrigger : MonoBehaviour
{
    [SerializeField] private Transform barrierRoot;

    private void Awake()
    {
        if (barrierRoot == null)
            barrierRoot = transform.root;
    }

    private void OnTriggerEnter(Collider other)
    {
        var stateManager = other.GetComponentInParent<EnemyStateManager>();
        if (stateManager == null) return;

        if (stateManager.TargetChecker.IsPathToBaseBlocked())
        {
            stateManager.SetBarrierTarget(barrierRoot);
            stateManager.ChangeState(stateManager.EnemyAttackState);
        }
    }
}
