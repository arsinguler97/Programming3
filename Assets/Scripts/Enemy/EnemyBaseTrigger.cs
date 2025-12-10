using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyBaseTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var stateManager = other.GetComponentInParent<EnemyStateManager>();
        if (stateManager == null) return;

        stateManager.SetInBaseTrigger(true);
        stateManager.ChangeState(stateManager.EnemyAttackState);
    }

    private void OnTriggerExit(Collider other)
    {
        var stateManager = other.GetComponentInParent<EnemyStateManager>();
        if (stateManager == null) return;

        stateManager.SetInBaseTrigger(false);
    }
}
