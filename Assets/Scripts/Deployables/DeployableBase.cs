using UnityEngine;

public abstract class DeployableBase : MonoBehaviour
{
    [SerializeField] protected int cost;
    public int Cost => cost;
}