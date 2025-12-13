using UnityEngine;
using UnityEngine.UI;

public class SpawnRateButtonsUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private Button slower50Button;
    [SerializeField] private Button slower100Button;
    [SerializeField] private Button normalButton;
    [SerializeField] private Button faster50Button;

    [Header("Visuals")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.green;

    private Image[] _images;
    private Color[] _defaultColors;

    private void Awake()
    {
        if (waveManager == null)
            waveManager = WaveManager.Instance;

        CacheImages();
        CacheDefaultColors();
        DisableTransitions();
    }

    private void Start()
    {
        ResetAllToDefault();
    }

    public void OnSlower50()
    {
        waveManager?.SetSpawnDelaySlower50();
        SetSelected(slower50Button != null ? slower50Button.image : null);
    }

    public void OnSlower100()
    {
        waveManager?.SetSpawnDelaySlower100();
        SetSelected(slower100Button != null ? slower100Button.image : null);
    }

    public void OnNormal()
    {
        waveManager?.SetSpawnDelayNormal();
        SetSelected(normalButton != null ? normalButton.image : null);
    }

    public void OnFaster50()
    {
        waveManager?.SetSpawnDelayFaster50();
        SetSelected(faster50Button != null ? faster50Button.image : null);
    }

    private void SetSelected(Image img)
    {
        ResetAllToDefault();

        if (img != null)
            ApplyVisual(img, selectedColor);
    }

    private void ApplyVisual(Image img, Color color)
    {
        if (img == null) return;
        img.color = color;
    }

    private void CacheImages()
    {
        _images = new[]
        {
            GetImage(slower50Button),
            GetImage(slower100Button),
            GetImage(normalButton),
            GetImage(faster50Button)
        };

    }

    private Image GetImage(Button btn)
    {
        if (btn == null) return null;
        // Prefer the button's target graphic; fall back to Image on the same GO.
        if (btn.targetGraphic is Image targetImage)
            return targetImage;
        return btn.GetComponent<Image>();
    }

    private void CacheDefaultColors()
    {
        _defaultColors = new Color[_images.Length];
        for (int i = 0; i < _images.Length; i++)
            _defaultColors[i] = _images[i] != null ? _images[i].color : normalColor;
    }

    private void ResetAllToDefault()
    {
        if (_images == null || _defaultColors == null) return;
        for (int i = 0; i < _images.Length; i++)
        {
            if (_images[i] != null)
                _images[i].color = _defaultColors[i];
        }
    }

    private void DisableTransitions()
    {
        SetTransitionNone(slower50Button);
        SetTransitionNone(slower100Button);
        SetTransitionNone(normalButton);
        SetTransitionNone(faster50Button);
    }

    private void SetTransitionNone(Button btn)
    {
        if (btn == null) return;
        btn.transition = Selectable.Transition.None;
    }
}
