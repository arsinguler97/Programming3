using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    public UnityEvent onItemCollected;
    public UnityEvent<int> onPointsAwarded;

    [SerializeField] private int pointValue = 20;

    private void Start()
    {
        ScoreManager sm = FindFirstObjectByType<ScoreManager>();
        if (sm != null)
            onPointsAwarded.AddListener(sm.HandlePointsAwarded);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onItemCollected?.Invoke();
            onPointsAwarded?.Invoke(pointValue);
            Destroy(gameObject);
        }
    }
}