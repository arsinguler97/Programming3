using UnityEngine;

public class FireCannon : DeployableBase
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float damagePerSecond = 10f;
    [SerializeField] private float range = 6f;
    [SerializeField] private float width = 3f;
    [SerializeField] private ParticleSystem fireVFX;

    private void Start()
    {
        if (fireVFX != null && !fireVFX.isPlaying)
            fireVFX.Play();
    }

    void Update()
    {
        if (firePoint == null) return;

        Collider[] hits = Physics.OverlapBox(
            firePoint.position + firePoint.forward * (range / 2f),
            new Vector3(width / 2f, 1f, range / 2f),
            firePoint.rotation
        );

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                var e = hit.GetComponent<EnemyHealth>();
                if (e != null)
                    e.EnemyTakeDamage(Mathf.RoundToInt(damagePerSecond * Time.deltaTime));
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