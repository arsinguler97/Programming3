using UnityEngine;

[CreateAssetMenu(fileName = "SpikeTrapStrategy", menuName = "Deployables/Spike Trap")]
public class SpikeTrapDeployStrategy : DeployStrategy
{
    public override void Deploy(Vector3 position)
    {
        Instantiate(deployablePrefab, position, Quaternion.identity);
    }
}