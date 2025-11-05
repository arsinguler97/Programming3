using UnityEngine;

[CreateAssetMenu(fileName = "Deploy Strategy", menuName = "Deployables/Base Strategy")]
public abstract class DeployStrategy : ScriptableObject
{
    public string strategyName;
    public GameObject deployablePrefab;

    public abstract void Deploy(Vector3 position, Quaternion rotation);
}