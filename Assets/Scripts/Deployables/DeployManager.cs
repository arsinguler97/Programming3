using UnityEngine;

public class DeployManager : MonoBehaviour
{
    [Header("Deploy Strategies")]
    [SerializeField] private DeployStrategy[] deployStrategies;

    [Header("Placement Settings")]
    [SerializeField] private Transform placementOrigin;
    [SerializeField] private float placementDistance = 4f;
    [SerializeField] private LayerMask groundMask;

    private ScoreManager _scoreManager;

    private void Start()
    {
        _scoreManager = FindFirstObjectByType<ScoreManager>();

        if (placementOrigin == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) placementOrigin = player.transform;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) TryDeploy(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TryDeploy(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) TryDeploy(2);
    }

    private void TryDeploy(int index)
    {
        if (deployStrategies == null || index < 0 || index >= deployStrategies.Length)
        {
            Debug.LogWarning("DeployManager: Invalid index!");
            return;
        }

        var strategy = deployStrategies[index];
        if (strategy == null || strategy.deployablePrefab == null)
        {
            Debug.LogWarning("DeployManager: Strategy or prefab missing!");
            return;
        }

        var deployable = strategy.deployablePrefab.GetComponent<DeployableBase>();
        if (deployable == null)
        {
            Debug.LogWarning("DeployManager: Prefab missing DeployableBase!");
            return;
        }

        if (_scoreManager == null)
        {
            Debug.LogError("DeployManager: ScoreManager not found!");
            return;
        }

        if (!_scoreManager.TrySpendGold(deployable.Cost))
        {
            Debug.Log("Not enough gold to deploy " + strategy.name);
            return;
        }

        Vector3 deployPos = GetPlacementPosition();
        strategy.Deploy(deployPos);
    }

    private Vector3 GetPlacementPosition()
    {
        if (placementOrigin == null)
            return Vector3.zero;

        Vector3 origin = placementOrigin.position + placementOrigin.forward * placementDistance;

        if (Physics.Raycast(origin + Vector3.up * 5f, Vector3.down, out RaycastHit hit, 10f, groundMask))
            return hit.point;

        return origin;
    }
}
