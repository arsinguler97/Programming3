using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneController : MonoBehaviour
{
    private static string _lastSceneName;

    private static SettingsData _appliedSettings = new SettingsData();

    [Header("SettingsMenu UI")]
    public TMP_Dropdown windowModeDropdown;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown framerateDropdown;
    public TMP_Dropdown fovDropdown;

    [Header("GameScene UI")]
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject resumeButton;

    private bool _isPaused;

    void Start()
    {
        Time.timeScale = 1f;
        _isPaused = false;

        if (pauseButton != null) pauseButton.SetActive(true);
        if (resumeButton != null) resumeButton.SetActive(false);

        if (SceneManager.GetActiveScene().name == "SettingsMenu")
            LoadAppliedSettingsToDropdowns();
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape) && !Input.GetKeyDown(KeyCode.P))
            return;

        var scene = SceneManager.GetActiveScene().name;

        if (scene == "GameScene")
        {
            if (_isPaused) ResumeGame();
            else PauseGame();
        }
        else if (scene == "SettingsMenu")
        {
            CloseSettings();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void OpenSettings()
    {
        _lastSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("SettingsMenu");
    }

    public void CloseSettings()
    {
        if (string.IsNullOrEmpty(_lastSceneName))
            SceneManager.LoadScene("MenuScene");
        else
            SceneManager.LoadScene(_lastSceneName);
    }

    public void ApplySettings()
    {
        if (windowModeDropdown == null) return;

        _appliedSettings.windowMode = windowModeDropdown.value;
        _appliedSettings.quality = qualityDropdown.value;
        _appliedSettings.framerate = framerateDropdown.value;
        _appliedSettings.fov = fovDropdown.value;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        _isPaused = true;

        if (pauseButton != null) pauseButton.SetActive(false);
        if (resumeButton != null) resumeButton.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        _isPaused = false;

        if (pauseButton != null) pauseButton.SetActive(true);
        if (resumeButton != null) resumeButton.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LoadAppliedSettingsToDropdowns()
    {
        if (windowModeDropdown == null) return;

        windowModeDropdown.value = _appliedSettings.windowMode;
        qualityDropdown.value = _appliedSettings.quality;
        framerateDropdown.value = _appliedSettings.framerate;
        fovDropdown.value = _appliedSettings.fov;

        windowModeDropdown.RefreshShownValue();
        qualityDropdown.RefreshShownValue();
        framerateDropdown.RefreshShownValue();
        fovDropdown.RefreshShownValue();
    }

    [System.Serializable]
    private class SettingsData
    {
        public int windowMode;
        public int quality;
        public int framerate;
        public int fov;
    }
}
