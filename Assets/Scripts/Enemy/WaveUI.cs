using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI remainingEnemiesText;

    private void Start()
    {
        // Only initialize if nothing is set yet, to avoid overwriting WaveManager's early updates.
        if (waveText != null && string.IsNullOrEmpty(waveText.text))
            waveText.text = "0/0";

        if (remainingEnemiesText != null && string.IsNullOrEmpty(remainingEnemiesText.text))
            remainingEnemiesText.text = "0";
    }

    public void UpdateWave(int currentWave, int totalWaves)
    {
        if (waveText != null)
            waveText.text = $"{currentWave}/{totalWaves}";
    }

    public void UpdateRemainingEnemies(int remainingEnemies)
    {
        if (remainingEnemiesText != null)
            remainingEnemiesText.text = remainingEnemies.ToString();
    }
}
