using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text itemsText;

    private int _score;
    private int _items;

    private void Start()
    {
        UpdateUI();
    }

    public void HandlePointsAwarded(int amount)
    {
        _score += amount;
        UpdateUI();
    }

    public void HandleItemCollected()
    {
        _items++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + _score;

        if (itemsText != null)
            itemsText.text = "Collected Items: " + _items;
    }
}