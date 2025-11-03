using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Slider waveProgressSlider;

    private void Start()
    {
        if (waveText != null)
            waveText.text = "Wave: 0";

        if (waveProgressSlider != null)
            waveProgressSlider.value = 1f;
    }

    public void UpdateWave(int waveNumber, int totalEnemies)
    {
        if (waveText != null)
            waveText.text = $"Wave: {waveNumber}";

        if (waveProgressSlider != null)
        {
            waveProgressSlider.maxValue = totalEnemies;
            waveProgressSlider.value = totalEnemies;
        }
    }

    public void UpdateProgress(int remainingEnemies)
    {
        if (waveProgressSlider != null)
            waveProgressSlider.value = remainingEnemies;
    }
}