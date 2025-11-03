using UnityEngine;
using System.Collections;

public class IceCannon : DeployableBase
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float range = 6f;
    [SerializeField] private float width = 3f;
    [SerializeField] private float slowAmount = 0.5f;
    [SerializeField] private float slowDuration = 1.5f;
    [SerializeField] private ParticleSystem iceVFX;

    private void Start()
    {
        if (iceVFX != null && !iceVFX.isPlaying)
            iceVFX.Play();
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
                var enemy = hit.GetComponent<EnemyController>();
                if (enemy != null)
                    StartCoroutine(ApplySlow(enemy));
            }
        }
    }

    private IEnumerator ApplySlow(EnemyController enemy)
    {
        float originalSpeed = enemy.AgentSpeed;
        enemy.SetSpeed(originalSpeed * slowAmount);
        yield return new WaitForSeconds(slowDuration);
        enemy.SetSpeed(originalSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint == null) return;

        Gizmos.color = Color.cyan;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(
            firePoint.position + firePoint.forward * (range / 2f),
            firePoint.rotation,
            Vector3.one
        );

        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, 2f, range));
    }
}