using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Arrow Settings")]
    [SerializeField] private float minSpeed = 10f;
    [SerializeField] private float maxSpeed = 50f;
    [SerializeField] private float maxChargeTime = 2f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private int damage = 10;
    [SerializeField] private string enemyTag = "Enemy";

    private Vector3 _velocity;
    private bool _hasHit;

    public float MinSpeed => minSpeed;
    public float MaxSpeed => maxSpeed;
    public float MaxChargeTime => maxChargeTime;

    private void Start()
    {
        Collider arrowCol = GetComponent<Collider>();
        Collider playerCol = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Collider>();
        if (arrowCol != null && playerCol != null)
        {
            Physics.IgnoreCollision(arrowCol, playerCol);
        }
    }

    private void Update()
    {
        if (_hasHit) return;

        _velocity += Vector3.down * (gravity * Time.deltaTime);
        transform.position += _velocity * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(_velocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasHit) return;
        if (other.CompareTag("Player")) return;

        _hasHit = true;
        transform.position += transform.forward * 0.2f;

        if (other.CompareTag(enemyTag))
        {
            other.GetComponent<EnemyHealth>()?.EnemyTakeDamage(damage);
            transform.SetParent(other.transform);
        }

        this.enabled = false;
    }

    public void SetInitialSpeed(float chargeTime)
    {
        float chargeValue = Mathf.Clamp01(chargeTime / maxChargeTime);
        float finalSpeed = Mathf.Lerp(minSpeed, maxSpeed, chargeValue);
        _velocity = transform.forward * finalSpeed;
    }
}