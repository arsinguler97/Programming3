using UnityEngine;

public class DeployManager : MonoBehaviour
{
    [Header("Deploy Strategies")]
    [SerializeField] private DeployStrategy fireCannonStrategy;
    [SerializeField] private DeployStrategy iceCannonStrategy;
    [SerializeField] private DeployStrategy spikeTrapStrategy;
    [SerializeField] private DeployStrategy barrierStrategy;

    [Header("Placement Settings")]
    [SerializeField] private Transform placementOrigin;
    [SerializeField] private float placementDistance = 4f;
    [SerializeField] private LayerMask groundMask;

    private ScoreManager _scoreManager;

    void Start()
    {
        _scoreManager = FindFirstObjectByType<ScoreManager>();

        if (placementOrigin == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) placementOrigin = player.transform;
        }
    }

    public void DeployFireCannon()
    {
        TryDeploy(fireCannonStrategy);
    }

    public void DeployIceCannon()
    {
        TryDeploy(iceCannonStrategy);
    }

    public void DeploySpikeTrap()
    {
        TryDeploy(spikeTrapStrategy);
    }
    
    public void DeployBarrier()
    {
        TryDeploy(barrierStrategy);
    }

    private void TryDeploy(DeployStrategy strategy)
    {
        if (strategy == null || strategy.deployablePrefab == null) return;

        var deployable = strategy.deployablePrefab.GetComponent<DeployableBase>();
        if (deployable == null) return;

        if (_scoreManager == null) return;

        if (!_scoreManager.TrySpendGold(deployable.Cost)) return;

        Vector3 pos = GetPlacementPosition();
        Quaternion rot = GetSnappedRotation();

        strategy.Deploy(pos, rot);
    }

    private Quaternion GetSnappedRotation()
    {
        float y = placementOrigin.eulerAngles.y;

        float[] angles = { 0f, 90f, 180f, 270f };
        float closest = 0f;
        float best = 999f;

        foreach (float a in angles)
        {
            float diff = Mathf.Abs(Mathf.DeltaAngle(y, a));
            if (diff < best)
            {
                best = diff;
                closest = a;
            }
        }

        return Quaternion.Euler(0f, closest, 0f);
    }

    private Vector3 GetPlacementPosition()
    {
        Vector3 origin = placementOrigin.position + placementOrigin.forward * placementDistance;

        if (Physics.Raycast(origin + Vector3.up * 5f, Vector3.down, out RaycastHit hit, 10f, groundMask))
            return hit.point;

        return origin;
    }
}