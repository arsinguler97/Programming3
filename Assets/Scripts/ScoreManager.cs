using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private int passiveGoldPerMinute = 10;
    private float _passiveTimer;

    private int _score;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        HandlePassiveGold();
    }

    private void HandlePassiveGold()
    {
        if (passiveGoldPerMinute <= 0) return;

        float interval = 60f / passiveGoldPerMinute;
        _passiveTimer += Time.deltaTime;

        if (_passiveTimer >= interval)
        {
            _passiveTimer = 0f;
            _score++;
            UpdateUI();
        }
    }

    public void HandlePointsAwarded(int amount)
    {
        _score += amount;
        UpdateUI();
    }

    public bool TrySpendGold(int amount)
    {
        if (_score < amount) return false;
        _score -= amount;
        UpdateUI();
        return true;
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "" + _score;
    }
}