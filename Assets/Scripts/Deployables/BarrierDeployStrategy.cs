using UnityEngine;

[CreateAssetMenu(fileName = "BarrierStrategy", menuName = "Deployables/Barrier")]
public class BarrierDeployStrategy : DeployStrategy
{
    public override void Deploy(Vector3 position, Quaternion rotation)
    {
        Instantiate(deployablePrefab, position, rotation);
    }
}