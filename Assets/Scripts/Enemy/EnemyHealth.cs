using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public event System.Action<int> OnHealthChanged;
    public event System.Action OnEnemyDeath;

    [Header("Visuals")]
    [SerializeField] private Renderer enemyRenderer;
    [SerializeField] private Material secondaryBaseMaterial;
    [SerializeField] private Material fireMaterial;
    [SerializeField] private Material iceMaterial;
    [SerializeField] private float fireEffectDuration = 0.35f;
    [SerializeField] private float iceEffectDuration = 1.5f;

    [SerializeField] private int enemyMaxHealth = 100;
    [SerializeField] private GameObject collectablePrefab;
    [SerializeField] private Vector3 dropOffset = new Vector3(0, 0.5f, 0);

    private int _enemyCurrentHealth;
    private Material _defaultSecondaryMaterial;
    private Coroutine _materialRoutine;

    public int EnemyCurrentHealth => _enemyCurrentHealth;
    public int EnemyMaxHealth => enemyMaxHealth;

    private void Awake()
    {
        SetupRenderer();
    }

    private void Start()
    {
        _enemyCurrentHealth = enemyMaxHealth;
        OnHealthChanged?.Invoke(_enemyCurrentHealth);
    }

    private void OnEnable()
    {
        _enemyCurrentHealth = enemyMaxHealth;
        OnHealthChanged?.Invoke(_enemyCurrentHealth);
    }

    public void EnemyTakeDamage(int damage)
    {
        _enemyCurrentHealth = Mathf.Max(0, _enemyCurrentHealth - damage);
        OnHealthChanged?.Invoke(_enemyCurrentHealth);

        if (_enemyCurrentHealth <= 0)
        {
            Die();
        }
    }

    public void ShowFireEffect()
    {
        if (fireMaterial == null) return;
        StartMaterialRoutine(fireMaterial, fireEffectDuration);
    }

    public void ShowIceEffect(float duration)
    {
        if (iceMaterial == null) return;
        float finalDuration = Mathf.Max(duration, iceEffectDuration);
        StartMaterialRoutine(iceMaterial, finalDuration);
    }

    public void ResetSecondaryMaterial()
    {
        ApplySecondaryMaterial(_defaultSecondaryMaterial);
    }

    private void Die()
    {
        OnEnemyDeath?.Invoke();

        if (collectablePrefab != null)
        {
            Instantiate(collectablePrefab, transform.position + dropOffset, Quaternion.identity);
        }

        gameObject.SetActive(false);
    }

    private void SetupRenderer()
    {
        if (enemyRenderer == null)
            enemyRenderer = GetComponentInChildren<Renderer>();

        if (enemyRenderer == null) return;

        Material[] mats = enemyRenderer.materials;

        if (mats.Length < 2)
        {
            Material baseMat = mats.Length > 0 ? mats[0] : secondaryBaseMaterial;
            Material secondMat = secondaryBaseMaterial != null ? secondaryBaseMaterial : baseMat;

            mats = new Material[2];
            mats[0] = baseMat;
            mats[1] = secondMat;
            enemyRenderer.materials = mats;
        }
        else
        {
            if (secondaryBaseMaterial != null)
            {
                mats[1] = secondaryBaseMaterial;
                enemyRenderer.materials = mats;
            }
            else if (mats[1] == null)
            {
                mats[1] = mats[0];
                enemyRenderer.materials = mats;
            }
        }

        _defaultSecondaryMaterial = enemyRenderer.materials.Length > 1 ? enemyRenderer.materials[1] : null;
    }

    private void StartMaterialRoutine(Material mat, float duration)
    {
        if (enemyRenderer == null) return;

        if (_materialRoutine != null)
            StopCoroutine(_materialRoutine);

        _materialRoutine = StartCoroutine(ApplyMaterialForDuration(mat, duration));
    }

    private IEnumerator ApplyMaterialForDuration(Material mat, float duration)
    {
        ApplySecondaryMaterial(mat);
        yield return new WaitForSeconds(duration);
        ApplySecondaryMaterial(_defaultSecondaryMaterial);
        _materialRoutine = null;
    }

    private void ApplySecondaryMaterial(Material mat)
    {
        if (enemyRenderer == null) return;

        Material[] mats = enemyRenderer.materials;
        if (mats.Length < 2)
            SetupRenderer();

        mats = enemyRenderer.materials;
        if (mats.Length < 2) return;

        mats[1] = mat != null ? mat : ( _defaultSecondaryMaterial != null ? _defaultSecondaryMaterial : mats[0]);
        enemyRenderer.materials = mats;
    }
}
