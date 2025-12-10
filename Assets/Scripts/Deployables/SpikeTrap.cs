using UnityEngine;

public class SpikeTrap : DeployableBase
{
    [SerializeField] private int damage = 25;
    [SerializeField] private ParticleSystem hitVfxPrefab;
    [SerializeField] private float vfxHeight = 0.25f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth e = other.GetComponent<EnemyHealth>() ?? other.GetComponentInParent<EnemyHealth>();
            if (e != null)
            {
                e.EnemyTakeDamage(damage);
            }

            PlayHitVfx(other);
        }
    }

    private void PlayHitVfx(Collider other)
    {
        Vector3 spawnPos = transform.position + Vector3.up * vfxHeight;

        if (hitVfxPrefab != null)
        {
            ParticleSystem vfx = Instantiate(hitVfxPrefab, spawnPos, Quaternion.identity);
            vfx.Play(true);

            var main = vfx.main;
            float lifetime = main.duration + main.startLifetime.constantMax;
            Destroy(vfx.gameObject, lifetime);
        }
    }
}
