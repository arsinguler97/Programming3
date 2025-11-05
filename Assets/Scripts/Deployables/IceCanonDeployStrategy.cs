using UnityEngine;

[CreateAssetMenu(fileName = "IceCannonStrategy", menuName = "Deployables/Ice Cannon")]
public class IceCannonDeployStrategy : DeployStrategy
{
    public override void Deploy(Vector3 position, Quaternion rotation)
    {
        Instantiate(deployablePrefab, position, rotation);
    }
}