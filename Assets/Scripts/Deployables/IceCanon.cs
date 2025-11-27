using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceCannon : DeployableBase
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float range = 6f;
    [SerializeField] private float width = 3f;
    [SerializeField] private float slowAmount = 0.5f;
    [SerializeField] private float slowDuration = 1.5f;
    [SerializeField] private ParticleSystem iceVFXPrefab;

    private readonly Collider[] _results = new Collider[20];
    private ParticleSystem _activeVFX;
    private readonly Dictionary<EnemyController, Coroutine> _activeSlows = new();

    private void Start()
    {
        if (iceVFXPrefab != null && firePoint != null)
        {
            _activeVFX = Instantiate(iceVFXPrefab, firePoint.position, firePoint.rotation, firePoint);
            _activeVFX.Play();
        }
    }

    void Update()
    {
        if (firePoint == null) return;

        int count = Physics.OverlapBoxNonAlloc(
            firePoint.position + firePoint.forward * (range / 2f),
            new Vector3(width / 2f, 1.5f, range / 2f),
            _results,
            firePoint.rotation,
            ~0,
            QueryTriggerInteraction.Collide
        );

        for (int i = 0; i < count; i++)
        {
            var hit = _results[i];
            if (!hit.CompareTag("Enemy")) continue;

            var enemy = hit.GetComponent<EnemyController>();
            if (enemy == null) continue;

            if (!_activeSlows.ContainsKey(enemy))
            {
                Coroutine c = StartCoroutine(ApplySlow(enemy));
                _activeSlows.Add(enemy, c);
            }
        }
    }

    private IEnumerator ApplySlow(EnemyController enemy)
    {
        if (enemy == null)
        {
            yield break;
        }

        float originalSpeed = enemy.AgentSpeed;
        enemy.SetSpeed(originalSpeed * slowAmount);

        yield return new WaitForSeconds(slowDuration);

        if (enemy != null)
            enemy.SetSpeed(originalSpeed);

        _activeSlows.Remove(enemy);
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint == null) return;

        Gizmos.color = Color.cyan;
        Matrix4x4 matrix = Matrix4x4.TRS(
            firePoint.position + firePoint.forward * (range / 2f),
            firePoint.rotation,
            Vector3.one
        );

        Gizmos.matrix = matrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, 2f, range));
    }
}
