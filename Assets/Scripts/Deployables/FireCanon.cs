using UnityEngine;

public class FireCannon : DeployableBase
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float damagePerSecond = 10f;
    [SerializeField] private float range = 6f;
    [SerializeField] private float width = 3f;
    [SerializeField] private ParticleSystem fireVFXPrefab;

    private ParticleSystem _activeVFX;
    private float _damageBuffer;

    private void Start()
    {
        if (fireVFXPrefab != null && firePoint != null)
        {
            _activeVFX = Instantiate(fireVFXPrefab, firePoint.position, firePoint.rotation, firePoint);
            _activeVFX.Play();
        }
    }

    void Update()
    {
        if (firePoint == null) return;

        _damageBuffer += damagePerSecond * Time.deltaTime;

        if (_damageBuffer >= 1f)
        {
            int finalDamage = Mathf.FloorToInt(_damageBuffer);
            _damageBuffer -= finalDamage;
            DealDamage(finalDamage);
        }
    }

    private void DealDamage(int amount)
    {
        Collider[] hits = Physics.OverlapBox(
            firePoint.position + firePoint.forward * (range / 2f),
            new Vector3(width / 2f, 1.5f, range / 2f),
            firePoint.rotation,
            ~0,
            QueryTriggerInteraction.Collide
        );

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyHealth e = hit.GetComponent<EnemyHealth>();
                if (e != null)
                    e.EnemyTakeDamage(amount);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint == null) return;

        Gizmos.color = Color.red;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(
            firePoint.position + firePoint.forward * (range / 2f),
            firePoint.rotation,
            Vector3.one
        );

        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, 2f, range));
    }
}
