using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Transform PlayerTransform { get; private set; }
    public Transform BaseTransform { get; private set; }

    protected override void Initialize()
    {
        base.Initialize();

        PlayerTransform = FindFirstObjectByType<PlayerHealth>().transform;
        BaseTransform = FindFirstObjectByType<BaseHealth>().transform;

        var playerHealth = PlayerTransform.GetComponent<PlayerHealth>();
        playerHealth.OnPlayerDeath += HandlePlayerDeath;

        var baseHealth = BaseTransform.GetComponent<BaseHealth>();
        baseHealth.OnBaseDestroyed += HandleBaseDestroyed;
    }

    private void HandlePlayerDeath()
    {
        RestartGame();
    }

    private void HandleBaseDestroyed()
    {
        RestartGame();
    }

    private void RestartGame()
    {
        WaveManager.Instance.ResetWaves();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}