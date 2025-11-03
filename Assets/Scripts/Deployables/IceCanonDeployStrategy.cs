using UnityEngine;

[CreateAssetMenu(fileName = "IceCannonStrategy", menuName = "Deployables/Ice Cannon")]
public class IceCannonDeployStrategy : DeployStrategy
{
    public override void Deploy(Vector3 position)
    {
        Instantiate(deployablePrefab, position, Quaternion.identity);
    }
}