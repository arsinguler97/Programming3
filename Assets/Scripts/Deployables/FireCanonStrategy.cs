using UnityEngine;

[CreateAssetMenu(fileName = "Fire Cannon Deploy", menuName = "Deployables/Fire Cannon")]
public class FireCannonDeployStrategy : DeployStrategy
{
    public override void Deploy(Vector3 position, Quaternion rotation)
    {
        Instantiate(deployablePrefab, position, rotation);
    }
}