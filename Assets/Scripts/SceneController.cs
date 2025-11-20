using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneController : MonoBehaviour
{
    private static string _lastSceneName;
    private static SettingsData _tempSettings = new SettingsData();
    private static SettingsData _savedSettings = new SettingsData();

    [Header("Optional (only in SettingsMenu)")]
    public TMP_Dropdown windowModeDropdown;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown framerateDropdown;
    public TMP_Dropdown fovDropdown;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "SettingsMenu")
        {
            LoadTempSettings();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("InGameUI");
    }

    public void OpenSettings()
    {
        _lastSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("SettingsMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ApplySettings()
    {
        if (windowModeDropdown == null) return;
        SaveDropdownValues();
        _savedSettings.CopyFrom(_tempSettings);
    }

    public void CloseSettings()
    {
        _tempSettings.CopyFrom(_savedSettings);
        SceneManager.LoadScene(_lastSceneName);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void SaveDropdownValues()
    {
        if (windowModeDropdown == null) return;

        _tempSettings.windowMode = windowModeDropdown.value;
        _tempSettings.quality = qualityDropdown.value;
        _tempSettings.framerate = framerateDropdown.value;
        _tempSettings.fov = fovDropdown.value;
    }

    private void LoadTempSettings()
    {
        if (windowModeDropdown == null) return;

        windowModeDropdown.value = _tempSettings.windowMode;
        qualityDropdown.value = _tempSettings.quality;
        framerateDropdown.value = _tempSettings.framerate;
        fovDropdown.value = _tempSettings.fov;

        windowModeDropdown.RefreshShownValue();
        qualityDropdown.RefreshShownValue();
        framerateDropdown.RefreshShownValue();
        fovDropdown.RefreshShownValue();
    }

    [System.Serializable]
    public class SettingsData
    {
        public int windowMode;
        public int quality;
        public int framerate;
        public int fov;

        public void CopyFrom(SettingsData other)
        {
            windowMode = other.windowMode;
            quality = other.quality;
            framerate = other.framerate;
            fov = other.fov;
        }
    }
}
