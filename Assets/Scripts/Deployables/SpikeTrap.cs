using UnityEngine;

public class SpikeTrap : DeployableBase
{
    [SerializeField] private int damage = 25;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth e = other.GetComponent<EnemyHealth>();
            if (e != null)
            {
                e.EnemyTakeDamage(damage);
            }
        }
    }
}