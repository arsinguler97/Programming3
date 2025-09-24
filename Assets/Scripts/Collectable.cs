using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    // UnityEvents visible in Inspector
    public UnityEvent onItemCollected;
    public UnityEvent<int> onPointsAwarded;
    
    private int _pointValue;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Invoke events
            onItemCollected?.Invoke();
            onPointsAwarded?.Invoke(_pointValue);
            
            Destroy(gameObject);
        }
    }
}